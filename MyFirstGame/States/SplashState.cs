using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using MyFirstGame.Input.Base;
using MyFirstGame.Input;
using MyFirstGame.Objects;
using MyFirstGame.States.Base;

namespace MyFirstGame.States;

public class SplashState : BaseGameState
{
    public override void LoadContent()
    {
        // TODO: Add Content Loading

        AddGameObject(new SplashImage(LoadTexture("splash")));
    }

    public override void HandleInput(Microsoft.Xna.Framework.GameTime gameTime)
    {
        InputManager.GetCommands(cmd =>
        {
            if (cmd is SplashInputCommand.GameSelect)
            {
                SwitchState(new GameplayState());
            }
        });
    }

    protected override void SetInputManager()
    {
        InputManager = new InputManager(new SplashInputMapper());
    }
}
