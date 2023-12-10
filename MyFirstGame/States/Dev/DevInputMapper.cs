using Microsoft.Xna.Framework.Input;
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

        if (state.IsKeyDown(Keys.Space))
        {
            commands.Add(new DevInputCommand.DevShoot());
        }

        return commands;
    }
}
