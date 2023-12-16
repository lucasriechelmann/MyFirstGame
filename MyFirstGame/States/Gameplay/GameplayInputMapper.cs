using Microsoft.Xna.Framework.Input;
using MyFirstGame.Engine.Input;
using System.Collections.Generic;

namespace MyFirstGame.States.Gameplay;

public class GameplayInputMapper : BaseInputMapper
{
    public override IEnumerable<BaseInputCommand> GetKeyboardState(KeyboardState state)
    {
        var commands = new List<GameplayInputCommand>();

        if (state.IsKeyDown(Keys.Escape))
        {
            commands.Add(new GameplayInputCommand.GameExit());
        }

        if (state.IsKeyDown(Keys.Up))
        {
            commands.Add(new GameplayInputCommand.PlayerMoveUp());
        }

        if (state.IsKeyDown(Keys.Down))
        {
            commands.Add(new GameplayInputCommand.PlayerMoveDown());
        }

        if (state.IsKeyDown(Keys.Left))
        {
            commands.Add(new GameplayInputCommand.PlayerMoveLeft());
        }

        if (state.IsKeyDown(Keys.Right))
        {
            commands.Add(new GameplayInputCommand.PlayerMoveRight());
        }

        if (state.IsKeyDown(Keys.Space))
        {
            commands.Add(new GameplayInputCommand.PlayerShoots());
        }

        if(!commands.Contains(new GameplayInputCommand.PlayerMoveUp()) && !commands.Contains(new GameplayInputCommand.PlayerMoveDown()))
        {
            commands.Add(new GameplayInputCommand.PlayerStopsMoving());
        }

        return commands;
    }
    public override IEnumerable<BaseInputCommand> GetGamePadState(GamePadState state)
    {
        var commands = new List<GameplayInputCommand>();

        if (state.Buttons.Back == ButtonState.Pressed)
        {
            commands.Add(new GameplayInputCommand.GameExit());
        }

        if (state.DPad.Up == ButtonState.Pressed)
        {
            commands.Add(new GameplayInputCommand.PlayerMoveUp());
        }

        if (state.DPad.Down == ButtonState.Pressed)
        {
            commands.Add(new GameplayInputCommand.PlayerMoveDown());
        }

        if (state.DPad.Left == ButtonState.Pressed)
        {
            commands.Add(new GameplayInputCommand.PlayerMoveLeft());
        }

        if (state.DPad.Right == ButtonState.Pressed)
        {
            commands.Add(new GameplayInputCommand.PlayerMoveRight());
        }

        if (state.Buttons.A == ButtonState.Pressed)
        {
            commands.Add(new GameplayInputCommand.PlayerShoots());
        }

        if (!commands.Contains(new GameplayInputCommand.PlayerMoveUp()) && !commands.Contains(new GameplayInputCommand.PlayerMoveDown()))
        {
            commands.Add(new GameplayInputCommand.PlayerStopsMoving());
        }

        return commands;
    }
}
