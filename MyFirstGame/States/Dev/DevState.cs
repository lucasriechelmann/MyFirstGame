using Microsoft.Xna.Framework;
using MyFirstGame.Engine.Input;
using MyFirstGame.Engine.States;
using MyFirstGame.Objects;
using MyFirstGame.Particles;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyFirstGame.States.Dev;

public class DevState : BaseGameState
{
    private const string ExhaustTexture = "Cloud";
    private const string MissileTexture = "Missile";
    private const string PlayerFighter = "fighter";

    private ExhaustEmitter _exhaustEmitter;
    private MissileSprite _missile;
    private PlayerSprite _player;

    public override void LoadContent()
    {
        var exhaustPosition = new Vector2(_viewPortWidth / 2, _viewPortHeight / 2);
        _exhaustEmitter = new ExhaustEmitter(LoadTexture(ExhaustTexture), exhaustPosition);
        AddGameObject(_exhaustEmitter);

        _player = new PlayerSprite(LoadTexture(PlayerFighter));
        _player.Position = new Vector2(500, 500);
        AddGameObject(_player);
    }

    public override void HandleInput(GameTime gameTime)
    {
        InputManager.GetCommands(cmd =>
        {
            if (cmd is DevInputCommand.DevQuit)
            {
                NotifyEvent(new BaseGameStateEvent.GameQuit());
            }

            if (cmd is DevInputCommand.DevShoot)
            {
                _missile = new MissileSprite(LoadTexture(MissileTexture), LoadTexture(ExhaustTexture));
                _missile.Position = new Vector2(_player.Position.X, _player.Position.Y - 25);
                AddGameObject(_missile);
            }
        });
    }

    public override void UpdateGameState(GameTime gameTime)
    {
        _exhaustEmitter.Position = new Vector2(_exhaustEmitter.Position.X, _exhaustEmitter.Position.Y - 3f);
        _exhaustEmitter.Update(gameTime);

        if (_missile != null)
        {
            _missile.Update(gameTime);

            if (_missile.Position.Y < -100)
            {
                RemoveGameObject(_missile);
            }
        }

        if (_exhaustEmitter.Position.Y < -200)
        {
            RemoveGameObject(_exhaustEmitter);
        }
    }

    protected override void SetInputManager()
    {
        InputManager = new InputManager(new DevInputMapper());
    }
}
