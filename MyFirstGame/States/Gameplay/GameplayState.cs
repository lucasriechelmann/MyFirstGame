﻿// Ignore Spelling: Gameplay

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using MyFirstGame.Engine.Input;
using MyFirstGame.Engine.Objects;
using MyFirstGame.Engine.Objects.Colisions;
using MyFirstGame.Engine.States;
using MyFirstGame.Levels;
using MyFirstGame.Objects;
using MyFirstGame.Objects.Text;
using MyFirstGame.Particles;
using System;
using System.Collections.Generic;
using System.Reflection.Emit;
using System.Threading.Tasks;

namespace MyFirstGame.States.Gameplay;

public class GameplayState : BaseGameState
{
    private const float SCOLLING_SPEED = 2.0f;
    #region Textures paths
    private const string BackgroundTexture = "png\\Barren";
    private const string PlayerFighter = "png\\animations\\FighterSpriteSheet";
    
    private const string BulletTexture = "png\\bullet";
    private const string ExhaustTexture = "png\\Cloud001";
    private const string MissileTexture = "png\\Missile05";
    private const string ChopperTexture = "png\\chopper-44x99";
    private const string ExplosionTexture = "png\\explosion";
    private const string TurretTexture = "png\\Tower";
    private const string TurretMG2Texture = "png\\MG2";
    private const string TurretBulletTexture = "png\\Bullet_MG";

    private const string TextFont = "font\\Lives";
    private const string GameOverFont = "font\\GameOver";

    private const string BulletSound = "sound\\bullet";
    private const string MissileSound = "sound\\missile";

    private const string Soundtrack1 = "music\\FutureAmbient_1";
    private const string Soundtrack2 = "music\\FutureAmbient_2";

    private const int StartingPlayerLives = 3;
    private int _playerLives = StartingPlayerLives;

    #endregion
    private const int MaxExplosionAge = 600; // 10 seconds
    private const int ExplosionActiveLength = 75; // emit particles for 1.2 seconds and let them fade out for 10 seconds

    private Texture2D _missileTexture;
    private Texture2D _exhaustTexture;
    private Texture2D _bulletTexture;
    private Texture2D _explosionTexture;
    private Texture2D _chopperTexture;
    private Texture2D _screenBoxTexture;

    private LivesText _livesText;
    private GameOverText _levelStartEndText;
    private PlayerSprite _playerSprite;
    private bool _playerDead;
    private bool _gameOver = false;

    private bool _isShootingBullets;
    private bool _isShootingMissile;
    private TimeSpan _lastBulletShotAt;
    private TimeSpan _lastMissileShotAt;

    private List<BulletSprite> _bulletList = new List<BulletSprite>();
    private List<MissileSprite> _missileList = new List<MissileSprite>();
    private List<ExplosionEmitter> _explosionList = new List<ExplosionEmitter>();
    private List<ChopperSprite> _enemyList = new List<ChopperSprite>();
    private List<TurretBulletSprite> _turretBulletList = new List<TurretBulletSprite>();
    private List<TurretSprite> _turretList = new List<TurretSprite>();
    private ChopperGenerator _chopperGenerator;

    private Level _level;

    public override void LoadContent()
    {
        base.LoadContent();
        _missileTexture = LoadTexture(MissileTexture);
        _exhaustTexture = LoadTexture(ExhaustTexture);
        _bulletTexture = LoadTexture(BulletTexture);
        _explosionTexture = LoadTexture(ExplosionTexture);
        _chopperTexture = LoadTexture(ChopperTexture);

        _playerSprite = new PlayerSprite(LoadTexture(PlayerFighter));
        _livesText = new LivesText(LoadFont(TextFont));
        _livesText.NbLives = StartingPlayerLives;
        _livesText.Position = new Vector2(10.0f, _viewPortHeight - 30);

        _levelStartEndText = new GameOverText(LoadFont(GameOverFont));

        var background = new TerrainBackground(LoadTexture(BackgroundTexture), SCOLLING_SPEED);
        background.zIndex = -100;
        AddGameObject(background);
        AddGameObject(_livesText);

        // load sound effects and register in the sound manager
        var bulletSound = LoadSound(BulletSound);
        var missileSound = LoadSound(MissileSound);
        _soundManager.RegisterSound(new GameplayEvents.PlayerShootsBullets(), bulletSound);
        _soundManager.RegisterSound(new GameplayEvents.PlayerShootsMissile(), missileSound, 0.4f, -0.2f, 0.0f);

        // load soundtracks into sound manager
        var track1 = LoadSound("music\\FutureAmbient_1").CreateInstance();
        var track2 = LoadSound("music\\FutureAmbient_2").CreateInstance();
        _soundManager.SetSoundtrack(track1, track2);

        _chopperGenerator = new ChopperGenerator(_chopperTexture, _viewPortWidth, AddChopper);

        var levelReader = new LevelReader(_viewPortWidth);
        _level = new Level(levelReader);

        _level.OnGenerateEnemies += _level_OnGenerateEnemies;
        _level.OnGenerateTurret += _level_OnGenerateTurret;
        _level.OnLevelStart += _level_OnLevelStart;
        _level.OnLevelEnd += _level_OnLevelEnd;
        _level.OnLevelNoRowEvent += _level_OnLevelNoRowEvent;

        ResetGame();
    }

    public override void HandleInput(GameTime gameTime)
    {
        InputManager.GetCommands(cmd =>
        {
            if (cmd is GameplayInputCommand.GameExit)
            {
                NotifyEvent(new BaseGameStateEvent.GameQuit());
            }

            if (_playerDead)
                return;

            if (cmd is GameplayInputCommand.PlayerMoveUp)
            {
                _playerSprite.MoveUp(gameTime);
                KeepPlayerInBounds();
            }

            if (cmd is GameplayInputCommand.PlayerMoveDown)
            {
                _playerSprite.MoveDown(gameTime);
                KeepPlayerInBounds();
            }

            if (cmd is GameplayInputCommand.PlayerMoveLeft)
            {
                _playerSprite.MoveLeft(gameTime);
                KeepPlayerInBounds();
            }

            if (cmd is GameplayInputCommand.PlayerMoveRight)
            {
                _playerSprite.MoveRight(gameTime);
                KeepPlayerInBounds();
            }

            if (cmd is GameplayInputCommand.PlayerStopsMoving)
            {
                _playerSprite.StopMoving();
            }

            if (cmd is GameplayInputCommand.PlayerShoots)
            {
                Shoot(gameTime);
            }
        });
    }
    public override void UpdateGameState(GameTime gameTime)
    {
        _playerSprite.Update(gameTime);

        _level.GenerateLevelEvents(gameTime);

        foreach (var bullet in _bulletList)
        {
            bullet.MoveUp(gameTime);
        }

        foreach (var missile in _missileList)
        {
            missile.Update(gameTime);
        }

        foreach (var chopper in _enemyList)
        {
            chopper.Update(gameTime);
        }

        foreach (var turret in _turretList)
        {
            turret.Update(gameTime, _playerSprite.CenterPosition);
            turret.Active = turret.Position.Y > 0 && turret.Position.Y < _viewPortHeight;
        }

        foreach (var bullet in _turretBulletList)
        {
            bullet.Update(gameTime);
        }

        UpdateExplosions(gameTime);
        RegulateShootingRate(gameTime);
        DetectCollisions();

        // get rid of bullets and missiles that have gone out of view
        _bulletList = CleanObjects(_bulletList);
        _missileList = CleanObjects(_missileList);
        _enemyList = CleanObjects(_enemyList);
        _turretBulletList = CleanObjects(_turretBulletList);
        _turretList = CleanObjects(_turretList, turret => turret.Position.Y > _viewPortHeight + 200);
    }
    public override void Render(SpriteBatch spriteBatch)
    {
        base.Render(spriteBatch);

        if (_gameOver)
        {
            // draw black rectangle at 30% transparency
            var screenBoxTexture = GetScreenBoxTexture(spriteBatch.GraphicsDevice);
            var viewportRectangle = new Rectangle(0, 0, _viewPortWidth, _viewPortHeight);
            spriteBatch.Draw(screenBoxTexture, viewportRectangle, Color.Black * 0.3f);
        }
    }
    private Texture2D GetScreenBoxTexture(GraphicsDevice graphicsDevice)
    {
        if (_screenBoxTexture == null)
        {
            _screenBoxTexture = new Texture2D(graphicsDevice, 1, 1);
            _screenBoxTexture.SetData<Color>(new Color[] { Color.White });
        }

        return _screenBoxTexture;
    }
    private void _level_OnLevelStart(object sender, LevelEvents.StartLevel e)
    {
        _levelStartEndText.Text = "Good luck, Player 1!";
        _levelStartEndText.Position = new Vector2(
            GetCenterXPositionOfGameObject(_levelStartEndText),
            GetCenterYPositionOfGameObject(_levelStartEndText));
        AddGameObject(_levelStartEndText);
    }

    private void _level_OnLevelEnd(object sender, LevelEvents.EndLevel e)
    {
        _levelStartEndText.Text = "You escaped. Congrats!";
        _levelStartEndText.Position = new Vector2(
            GetCenterXPositionOfGameObject(_levelStartEndText),
            GetCenterYPositionOfGameObject(_levelStartEndText));
        AddGameObject(_levelStartEndText);
    }
    int GetCenterXPositionOfGameObject(BaseGameObject gameObject)
    {
        if (gameObject is GameOverText textGameObject)
            return (_viewPortWidth / 2 - textGameObject.FontWidth / 2);

        return (_viewPortWidth / 2 - gameObject.Width / 2);
    }
    int GetCenterYPositionOfGameObject(BaseGameObject gameObject)
    {
        if(gameObject is GameOverText textGameObject)
            return (_viewPortHeight / 2 - textGameObject.FontHeight / 2);

        return (_viewPortHeight / 2 - gameObject.Height / 2);
    }
    private void _level_OnLevelNoRowEvent(object sender, LevelEvents.NoRowEvent e)
    {
        RemoveGameObject(_levelStartEndText);
    }

    private void _level_OnGenerateTurret(object sender, LevelEvents.GenerateTurret e)
    {
        var turret = new TurretSprite(LoadTexture(TurretTexture), LoadTexture(TurretMG2Texture), SCOLLING_SPEED);

        // position the turret offscreen at the top
        turret.Position = new Vector2(e.XPosition, -100);

        turret.OnTurretShoots += _turret_OnTurretShoots;
        turret.OnObjectChanged += _onObjectChanged;
        AddGameObject(turret);

        _turretList.Add(turret);
    }

    private void _turret_OnTurretShoots(object sender, GameplayEvents.TurretShoots e)
    {
        var bullet1 = new TurretBulletSprite(LoadTexture(TurretBulletTexture), e.Direction, e.Angle);
        bullet1.Position = e.Bullet1Position;
        bullet1.zIndex = -10;

        var bullet2 = new TurretBulletSprite(LoadTexture(TurretBulletTexture), e.Direction, e.Angle);
        bullet2.Position = e.Bullet2Position;
        bullet2.zIndex = -10;

        AddGameObject(bullet1);
        AddGameObject(bullet2);

        _turretBulletList.Add(bullet1);
        _turretBulletList.Add(bullet2);
    }

    private void _level_OnGenerateEnemies(object sender, LevelEvents.GenerateEnemies e)
    {
        _chopperGenerator.GenerateChoppers(e.NbEnemies);
    }

    private void RegulateShootingRate(GameTime gameTime)
    {
        // can't shoot bullets more than every 0.2 second
        if (_lastBulletShotAt != null && gameTime.TotalGameTime - _lastBulletShotAt > TimeSpan.FromSeconds(0.2))
            _isShootingBullets = false;

        // can't shoot missiles more than every 1 second
        if (_lastMissileShotAt != null && gameTime.TotalGameTime - _lastMissileShotAt > TimeSpan.FromSeconds(1.0))
            _isShootingMissile = false;
    }
    private void DetectCollisions()
    {
        var bulletCollisionDetector = new AABBCollisionDetector<BulletSprite, BaseGameObject>(_bulletList);
        var missileCollisionDetector = new AABBCollisionDetector<MissileSprite, BaseGameObject>(_missileList);
        var playerCollisionDetector = new AABBCollisionDetector<ChopperSprite, PlayerSprite>(_enemyList);
        var turretBulletCollisionDetector = new SegmentAABBCollisionDetector<PlayerSprite>(_playerSprite);

        bulletCollisionDetector.DetectCollisions(_enemyList, (bullet, chopper) =>
        {
            var hitEvent = new GameplayEvents.ObjectHitBy(bullet);
            chopper.OnNotify(hitEvent);
            _soundManager.OnNotify(hitEvent);
            bullet.Destroy();
        });

        missileCollisionDetector.DetectCollisions(_enemyList, (missile, chopper) =>
        {
            var hitEvent = new GameplayEvents.ObjectHitBy(missile);
            chopper.OnNotify(hitEvent);
            _soundManager.OnNotify(hitEvent);
            missile.Destroy();
        });

        bulletCollisionDetector.DetectCollisions(_turretList, (bullet, turret) =>
        {
            var hitEvent = new GameplayEvents.ObjectHitBy(bullet);
            turret.OnNotify(hitEvent);
            _soundManager.OnNotify(hitEvent);
            bullet.Destroy();
        });

        missileCollisionDetector.DetectCollisions(_turretList, (missile, turret) =>
        {
            var hitEvent = new GameplayEvents.ObjectHitBy(missile);
            turret.OnNotify(hitEvent);
            _soundManager.OnNotify(hitEvent);
            missile.Destroy();
        });

        if (!_playerDead)
        {
            var segments = new List<Segment>();
            foreach (var bullet in _turretBulletList)
            {
                segments.Add(bullet.CollisionSegment);
            }

            turretBulletCollisionDetector.DetectCollisions(segments, _ =>
            {
                KillPlayer();
            });

            playerCollisionDetector.DetectCollisions(_playerSprite, (chopper, player) =>
            {
                KillPlayer();
            });
        }
    }

    private void ResetGame()
    {
        if (_chopperGenerator != null)
        {
            _chopperGenerator.StopGenerating();
        }

        foreach (var bullet in _bulletList)
        {
            RemoveGameObject(bullet);
        }

        foreach (var missile in _missileList)
        {
            RemoveGameObject(missile);
        }

        foreach (var chopper in _enemyList)
        {
            RemoveGameObject(chopper);
        }

        foreach (var explosion in _explosionList)
        {
            RemoveGameObject(explosion);
        }

        foreach (var bullet in _turretBulletList)
        {
            RemoveGameObject(bullet);
        }

        foreach (var turret in _turretList)
        {
            RemoveGameObject(turret);
        }

        _bulletList = new List<BulletSprite>();
        _turretBulletList = new List<TurretBulletSprite>();
        _turretList = new List<TurretSprite>();
        _missileList = new List<MissileSprite>();
        _explosionList = new List<ExplosionEmitter>();
        _enemyList = new List<ChopperSprite>();

        AddGameObject(_playerSprite);

        // position the player in the middle of the screen, at the bottom, leaving a slight gap at the bottom
        var playerXPos = _viewPortWidth / 2 - _playerSprite.Width / 2;
        var playerYPos = _viewPortHeight - _playerSprite.Height - 30;
        _playerSprite.Position = new Vector2(playerXPos, playerYPos);

        _playerDead = false;
        _level.Reset();
    }

    private async void KillPlayer()
    {
        if (_indestructible)
        {
            return;
        }

        _playerDead = true;
        _playerLives -= 1;
        _livesText.NbLives = _playerLives;

        AddExplosion(_playerSprite.Position);
        RemoveGameObject(_playerSprite);

        await Task.Delay(TimeSpan.FromSeconds(2));

        if (_playerLives > 0)
        {
            ResetGame();
        }
        else
        {
            GameOver();
        }
    }
    private void GameOver()
    {
        var font = LoadFont(GameOverFont);
        var gameOverText = new GameOverText(font);
        var textPositionOnScreen = new Vector2(460, 300);

        gameOverText.Position = textPositionOnScreen;
        AddGameObject(gameOverText);
        _gameOver = true;
    }
    private void AddChopper(ChopperSprite chopper)
    {
        chopper.OnObjectChanged += _onObjectChanged;
        _enemyList.Add(chopper);
        AddGameObject(chopper);
    }
    private List<T> CleanObjects<T>(List<T> objectList, Func<T, bool> predicate) where T : BaseGameObject
    {
        List<T> listOfItemsToKeep = new List<T>();
        foreach (T item in objectList)
        {
            var performRemoval = predicate(item);

            if (performRemoval || item.Destroyed)
            {
                RemoveGameObject(item);
            }
            else
            {
                listOfItemsToKeep.Add(item);
            }
        }

        return listOfItemsToKeep;
    }
    private List<T> CleanObjects<T>(List<T> objectList) where T : BaseGameObject
    {
        return CleanObjects(objectList, item => item.Position.Y < -50);
    }
    private void _onObjectChanged(object sender, BaseGameStateEvent e)
    {
        var chopper = (BaseGameObject)sender;
        switch (e)
        {
            case GameplayEvents.ObjectLostLife ge:
                if (ge.CurrentLife <= 0)
                {
                    AddExplosion(new Vector2(chopper.Position.X - 40, chopper.Position.Y - 40));
                    chopper.Destroy();
                }
                break;
        }
    }

    private void AddExplosion(Vector2 position)
    {
        var explosion = new ExplosionEmitter(_explosionTexture, position);
        AddGameObject(explosion);
        _explosionList.Add(explosion);
    }

    private void UpdateExplosions(GameTime gameTime)
    {
        foreach (var explosion in _explosionList)
        {
            explosion.Update(gameTime);

            if (explosion.Age > ExplosionActiveLength)
            {
                explosion.Deactivate();
            }

            if (explosion.Age > MaxExplosionAge)
            {
                RemoveGameObject(explosion);
            }
        }
    }
    

    private void Shoot(GameTime gameTime)
    {
        if (!_isShootingBullets)
        {
            CreateBullets();
            _isShootingBullets = true;
            _lastBulletShotAt = gameTime.TotalGameTime;

            NotifyEvent(new GameplayEvents.PlayerShootsBullets());
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
        var bulletSpriteLeft2 = new BulletSprite(_bulletTexture);
        var bulletSpriteRight = new BulletSprite(_bulletTexture);
        var bulletSpriteRight2 = new BulletSprite(_bulletTexture);

        var bulletY = _playerSprite.Position.Y + 30;
        var bulletLeftX = _playerSprite.Position.X + _playerSprite.Width / 2 - 40;
        var bulletLeftX2 = bulletLeftX - 20;
        var bulletRightX = _playerSprite.Position.X + _playerSprite.Width / 2 + 10;
        var bulletRightX2 = bulletRightX + 20;

        bulletSpriteLeft.Position = new Vector2(bulletLeftX, bulletY);
        bulletSpriteLeft2.Position = new Vector2(bulletLeftX2, bulletY);
        bulletSpriteRight.Position = new Vector2(bulletRightX, bulletY);
        bulletSpriteRight2.Position = new Vector2(bulletRightX2, bulletY);

        _bulletList.Add(bulletSpriteLeft);
        _bulletList.Add(bulletSpriteRight);
        _bulletList.Add(bulletSpriteLeft2);
        _bulletList.Add(bulletSpriteRight2);

        AddGameObject(bulletSpriteLeft);
        AddGameObject(bulletSpriteRight);
        AddGameObject(bulletSpriteLeft2);
        AddGameObject(bulletSpriteRight2);
    }
    private void CreateMissile()
    {
        var missileSprite = new MissileSprite(_missileTexture, _exhaustTexture);
        missileSprite.Position = new Vector2(_playerSprite.Position.X + 33, _playerSprite.Position.Y - 25);
        var missileSprite2 = new MissileSprite(_missileTexture, _exhaustTexture);
        var missileSprite3 = new MissileSprite(_missileTexture, _exhaustTexture);
        missileSprite2.Position = new Vector2(_playerSprite.Position.X, _playerSprite.Position.Y - 25);
        missileSprite3.Position = new Vector2(_playerSprite.Position.X + 66, _playerSprite.Position.Y - 25);

        _missileList.Add(missileSprite);
        _missileList.Add(missileSprite2);
        _missileList.Add(missileSprite3);
        AddGameObject(missileSprite);
        AddGameObject(missileSprite2);
        AddGameObject(missileSprite3);
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
