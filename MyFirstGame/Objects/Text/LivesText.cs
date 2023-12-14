using Microsoft.Xna.Framework.Graphics;
using MyFirstGame.Engine.Objects;

namespace MyFirstGame.Objects.Text;

public class LivesText : BaseTextObject
{
    private int _nbLives = -1;

    public int NbLives
    {
        get
        {
            return _nbLives;
        }
        set
        {
            _nbLives = value;
            Text = $"Lives: {_nbLives}";
        }
    }

    public LivesText(SpriteFont font)
    {
        _font = font;
    }
}
