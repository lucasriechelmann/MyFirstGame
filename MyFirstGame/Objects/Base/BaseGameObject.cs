using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MyFirstGame.Enums;

namespace MyFirstGame.Objects.Base;

public abstract class BaseGameObject
{
    protected Texture2D _texture;

    private Vector2 _position;
    public int zIndex { get; set; }
    public void OnNotify(Events eventType)
    {

    }
    public void Render(SpriteBatch spriteBatch)
    {
        spriteBatch.Draw(_texture, Vector2.One, Color.White);
    }
}
