// Ignore Spelling: Gameplay

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using MyFirstGame.Engine.Input;
using MyFirstGame.Engine.Objects;
using MyFirstGame.Engine.States;
using MyFirstGame.Objects;
using System;
using System.Collections.Generic;

namespace MyFirstGame.States.Gameplay;

public class GameplayState : BaseGameState
{
    private const string PlayerFighter = "png\\fighter";
    private const string PlayerFighterSeagull = "Seagull";
    private const string BackgroundTexture = "png\\Barren";
    private const string BulletTexture = "png\\bullet";
    private const string ExhaustTexture = "png\\Cloud001";
    private const string MissileTexture = "png\\Missile05";
    private PlayerSprite _playerSprite;
    private Texture2D _missileTexture;
    private Texture2D _exhaustTexture;
    private Texture2D _bulletTexture;
    private bool _isShootingBullets;
    private bool _isShootingMissile;
    private TimeSpan _lastBulletShotAt;
    private TimeSpan _lastMissileShotAt;

    private List<BulletSprite> _bulletList;
    private List<MissileSprite> _missileList;
    public override void LoadContent()
    {
        _missileTexture = LoadTexture(MissileTexture);
        _exhaustTexture = LoadTexture(ExhaustTexture);
        _bulletTexture = LoadTexture(BulletTexture);
        
        _bulletList = new List<BulletSprite>();
        _missileList = new List<MissileSprite>();

        _playerSprite = new PlayerSprite(LoadTexture(PlayerFighter));
        
        AddGameObject(new TerrainBackground(LoadTexture(BackgroundTexture)));
        AddGameObject(_playerSprite);

        // position the player in the middle of the screen, at the bottom, leaving a slight gap at the bottom
        var playerXPos = _viewPortWidth / 2 - _playerSprite.Width / 2;
        var playerYPos = _viewPortHeight - _playerSprite.Height - 30;
        _playerSprite.Position = new Vector2(playerXPos, playerYPos);

        // load sound effects and register in the sound manager
        var bulletSound = LoadSound("sound\\bullet");
        _soundManager.RegisterSound(new GameplayEvents.PlayerShoots(), bulletSound);

        // load soundtracks into sound manager
        var track1 = LoadSound("music\\FutureAmbient_1").CreateInstance();
        var track2 = LoadSound("music\\FutureAmbient_2").CreateInstance();
        _soundManager.SetSoundtrack(track1, track2);
    }

    public override void HandleInput(GameTime gameTime)
    {
        InputManager.GetCommands(cmd =>
        {
            if (cmd is GameplayInputCommand.GameExit)
            {
                NotifyEvent(new BaseGameStateEvent.GameQuit());
            }

            if (cmd is GameplayInputCommand.PlayerMoveUp)
            {
                _playerSprite.MoveUp();
                KeepPlayerInBounds();
            }

            if (cmd is GameplayInputCommand.PlayerMoveDown)
            {
                _playerSprite.MoveDown();
                KeepPlayerInBounds();
            }

            if (cmd is GameplayInputCommand.PlayerMoveLeft)
            {
                _playerSprite.MoveLeft();
                KeepPlayerInBounds();
            }

            if (cmd is GameplayInputCommand.PlayerMoveRight)
            {
                _playerSprite.MoveRight();
                KeepPlayerInBounds();
            }

            if (cmd is GameplayInputCommand.PlayerShoots)
            {
                Shoot(gameTime);
            }
        });
    }
    public override void UpdateGameState(GameTime gameTime)
    {
        foreach (var bullet in _bulletList)
        {
            bullet.MoveUp();
        }

        foreach (var missile in _missileList)
        {
            missile.Update(gameTime);
        }

        // can't shoot more than every 0.2 seconds
        if (_lastBulletShotAt != null && gameTime.TotalGameTime - _lastBulletShotAt > TimeSpan.FromSeconds(0.2))
        {
            _isShootingBullets = false;
        }

        // can't shoot missiles more than every 1 second
        if (_lastMissileShotAt != null && gameTime.TotalGameTime - _lastMissileShotAt > TimeSpan.FromSeconds(1.0))
        {
            _isShootingMissile = false;
        }

        // get rid of bullets and missiles that have gone out of view
        _bulletList = CleanObjects(_bulletList);
        _missileList = CleanObjects(_missileList);
    }

    private List<T> CleanObjects<T>(List<T> objectList) where T : BaseGameObject
    {
        List<T> listOfItemsToKeep = new List<T>();
        foreach (T item in objectList)
        {
            var stillOnScreen = item.Position.Y > -50;

            if (stillOnScreen)
            {
                listOfItemsToKeep.Add(item);
            }
            else
            {
                RemoveGameObject(item);
            }
        }

        return listOfItemsToKeep;
    }

    private void Shoot(GameTime gameTime)
    {
        if (!_isShootingBullets)
        {
            CreateBullets();
            _isShootingBullets = true;
            _lastBulletShotAt = gameTime.TotalGameTime;

            NotifyEvent(new GameplayEvents.PlayerShoots());
        }

        if (!_isShootingMissile)
        {
            CreateMissile();
            _isShootingMissile = true;
            _lastMissileShotAt = gameTime.TotalGameTime;

            NotifyEvent(new GameplayEvents.PlayerShootsMissile());
        }
    }

    private void CreateBullets()
    {
        var bulletSpriteLeft = new BulletSprite(_bulletTexture);
        var bulletSpriteRight = new BulletSprite(_bulletTexture);

        var bulletY = _playerSprite.Position.Y + 30;
        var bulletLeftX = _playerSprite.Position.X + _playerSprite.Width / 2 - 40;
        var bulletRightX = _playerSprite.Position.X + _playerSprite.Width / 2 + 10;

        bulletSpriteLeft.Position = new Vector2(bulletLeftX, bulletY);
        bulletSpriteRight.Position = new Vector2(bulletRightX, bulletY);

        _bulletList.Add(bulletSpriteLeft);
        _bulletList.Add(bulletSpriteRight);

        AddGameObject(bulletSpriteLeft);
        AddGameObject(bulletSpriteRight);
    }
    private void CreateMissile()
    {
        var missileSprite = new MissileSprite(_missileTexture, _exhaustTexture);
        missileSprite.Position = new Vector2(_playerSprite.Position.X + 33, _playerSprite.Position.Y - 25);

        _missileList.Add(missileSprite);
        AddGameObject(missileSprite);
    }

    private void KeepPlayerInBounds()
    {
        if (_playerSprite.Position.X < 0)
        {
            _playerSprite.Position = new Vector2(0, _playerSprite.Position.Y);
        }

        if (_playerSprite.Position.X > _viewPortWidth - _playerSprite.Width)
        {
            _playerSprite.Position = new Vector2(_viewPortWidth - _playerSprite.Width, _playerSprite.Position.Y);
        }

        if (_playerSprite.Position.Y < 0)
        {
            _playerSprite.Position = new Vector2(_playerSprite.Position.X, 0);
        }

        if (_playerSprite.Position.Y > _viewPortHeight - _playerSprite.Height)
        {
            _playerSprite.Position = new Vector2(_playerSprite.Position.X, _viewPortHeight - _playerSprite.Height);
        }
    }

    protected override void SetInputManager()
    {
        InputManager = new InputManager(new GameplayInputMapper());
    }
}
