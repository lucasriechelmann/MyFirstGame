using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MyFirstGame.Engine.Objects;

namespace MyFirstGame.Objects;

public class SplashImage : BaseGameObject
{
    int _width;
    int _height;
    public override void Render(SpriteBatch spriteBatch)
    {
        spriteBatch.Draw(_texture, new Rectangle(0, 0, _width, _height), Color.White);
    }
    public SplashImage(Texture2D texture, int width, int height)
    {
        _texture = texture;        
        _width = width;
        _height = height;
    }
}
