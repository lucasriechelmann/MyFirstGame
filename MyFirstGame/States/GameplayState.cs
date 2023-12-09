// Ignore Spelling: Gameplay

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using MyFirstGame.Enums;
using MyFirstGame.Objects;
using MyFirstGame.States.Base;

namespace MyFirstGame.States;

public class GameplayState : BaseGameState
{
    private const string PlayerFighter = "fighter";
    private const string BackgroundTexture = "Barren";
    public override void LoadContent()
    {
        AddGameObject(new SplashImage(LoadTexture(BackgroundTexture)));
        AddGameObject(new PlayerSprite(LoadTexture(PlayerFighter)));
    }

    public override void HandleInput()
    {
        var state = Keyboard.GetState();
        if (state.IsKeyDown(Keys.Escape))
        {
            NotifyEvent(Events.GAME_QUIT);
        }
    }
}
