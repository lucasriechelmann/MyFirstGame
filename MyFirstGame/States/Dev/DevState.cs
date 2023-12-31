﻿using Microsoft.Xna.Framework;
using MyFirstGame.Engine.Input;
using MyFirstGame.Engine.States;
using MyFirstGame.Objects;
using MyFirstGame.Particles;
using System;

namespace MyFirstGame.States.Dev;

/// <summary>
/// Used to test out new things, like particle engines and shooting missiles
/// </summary>
public class DevState : BaseGameState
{
    private const string CloudTexture = "png\\explosion";
    private const string ChopperTexture = "png\\chopper-44x99";

    private ChopperSprite _chopper;
    private ExplosionEmitter _explosion;
    private TimeSpan _explodeAt;

    public override void LoadContent()
    {
        _chopper = new ChopperSprite(LoadTexture(ChopperTexture), new System.Collections.Generic.List<(int, Vector2)>());
        _chopper.Position = new Vector2(300, 100);
        AddGameObject(_chopper);
    }

    public override void HandleInput(GameTime gameTime)
    {
        InputManager.GetCommands(cmd =>
        {
            if (cmd is DevInputCommand.DevQuit)
            {
                NotifyEvent(new BaseGameStateEvent.GameQuit());
            }
        });
    }

    public override void UpdateGameState(GameTime gameTime)
    {
        if (_explosion == null && gameTime.TotalGameTime > TimeSpan.FromSeconds(2))
        {
            _explosion = new ExplosionEmitter(LoadTexture(CloudTexture), new Vector2(260, 60));
            AddGameObject(_explosion);
            _explodeAt = gameTime.TotalGameTime;
        }

        if (_explosion != null && gameTime.TotalGameTime - _explodeAt > TimeSpan.FromSeconds(1.2))
        {
            _explosion.Deactivate();
        }

        if (_explosion != null && gameTime.TotalGameTime - _explodeAt > TimeSpan.FromSeconds(0.5))
        {
            RemoveGameObject(_chopper);
        }

        if (_explosion != null && gameTime.TotalGameTime > TimeSpan.FromSeconds(10))
        {
            RemoveGameObject(_explosion);
        }

        if (_explosion != null)
        {
            _explosion.Update(gameTime);
        }
    }

    protected override void SetInputManager()
    {
        InputManager = new InputManager(new DevInputMapper());
    }
}
