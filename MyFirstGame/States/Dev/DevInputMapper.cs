﻿using Microsoft.Xna.Framework.Input;
using MyFirstGame.Engine.Input;
using System.Collections.Generic;

namespace MyFirstGame.States.Dev;

public class DevInputMapper : BaseInputMapper
{
    public override IEnumerable<BaseInputCommand> GetKeyboardState(KeyboardState state)
    {
        var commands = new List<DevInputCommand>();

        if (state.IsKeyDown(Keys.Escape))
        {
            commands.Add(new DevInputCommand.DevQuit());
        }

        if (state.IsKeyDown(Keys.Z))
        {
            commands.Add(new DevInputCommand.DevBulletSparks());
        }

        if (state.IsKeyDown(Keys.X))
        {
            commands.Add(new DevInputCommand.DevMissileExplode());
        }

        if (state.IsKeyDown(Keys.C))
        {
            commands.Add(new DevInputCommand.DevExplode());
        }

        return commands;
    }
}
