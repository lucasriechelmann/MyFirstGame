using Microsoft.Xna.Framework.Input;
using MyFirstGame.Input.Base;
using System.Collections.Generic;

namespace MyFirstGame.Input;

public class SplashInputMapper : BaseInputMapper
{
    public override IEnumerable<BaseInputCommand> GetKeyboardState(KeyboardState state)
    {
        var commands = new List<SplashInputCommand>();

        if (state.IsKeyDown(Keys.Enter))
        {
            commands.Add(new SplashInputCommand.GameSelect());
        }

        return commands;
    }
}
