using Microsoft.Xna.Framework.Graphics;
using MyFirstGame.Engine.Objects;

namespace MyFirstGame.Objects.Text;

public class GameOverText : BaseTextObject
{
    public GameOverText(SpriteFont font)
    {
        _font = font;
        Text = "Game Over";
    }
}
