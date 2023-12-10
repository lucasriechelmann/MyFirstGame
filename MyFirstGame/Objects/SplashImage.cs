using Microsoft.Xna.Framework.Graphics;
using MyFirstGame.Engine.Objects;

namespace MyFirstGame.Objects;

public class SplashImage : BaseGameObject
{
    public SplashImage(Texture2D texture)
    {
        _texture = texture;
    }
}
