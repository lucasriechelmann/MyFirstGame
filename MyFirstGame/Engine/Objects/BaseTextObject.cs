using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MyFirstGame.Engine.Objects;

public class BaseTextObject : BaseGameObject
{
    protected SpriteFont _font;
    public BaseTextObject(SpriteFont font)
    {
        _font = font;
    }
    public int FontWidth => _font?.Texture?.Width ?? 0;
    public int FontHeight => _font?.Texture?.Height ?? 0;
    public string Text { get; set; }
    public override void Render(SpriteBatch spriteBatch) => spriteBatch.DrawString(_font, Text, _position, Color.White);
}
