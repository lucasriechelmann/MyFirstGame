using Microsoft.Xna.Framework;
using MyFirstGame.Engine.Input;
using MyFirstGame.Engine.States;
using MyFirstGame.Objects;
using MyFirstGame.States.Gameplay;

namespace MyFirstGame.States.Splash;

public class SplashState : BaseGameState
{
    public override void LoadContent()
    {
        // TODO: Add Content Loading

        AddGameObject(new SplashImage(LoadTexture("png\\splash"), _viewPortWidth, _viewPortHeight));
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
    public override void UpdateGameState(GameTime _) { }
    protected override void SetInputManager()
    {
        InputManager = new InputManager(new SplashInputMapper());
    }
}
