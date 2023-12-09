// Ignore Spelling: Gameplay

using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework;
using MyFirstGame.Enums;
using MyFirstGame.States.Base;

namespace MyFirstGame.States;

public class GameplayState : BaseGameState
{
    public override void LoadContent(ContentManager contentManager)
    {

    }

    public override void UnloadContent(ContentManager contentManager)
    {
        contentManager.Unload();
    }

    public override void HandleInput()
    {
        if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed ||
            Keyboard.GetState().IsKeyDown(Keys.Enter))
        {
            NotifyEvent(Events.GAME_QUIT);
        }
    }
}
