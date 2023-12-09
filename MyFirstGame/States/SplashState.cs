using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
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

    public override void HandleInput()
    {
        var state = Keyboard.GetState();

        if (state.IsKeyDown(Keys.Enter))
        {
            SwitchState(new GameplayState());
        }
    }
}
