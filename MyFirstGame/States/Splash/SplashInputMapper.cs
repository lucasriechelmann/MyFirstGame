using Microsoft.Xna.Framework.Input;
using MyFirstGame.Engine.Input;
using System.Collections.Generic;

namespace MyFirstGame.States.Splash;

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
