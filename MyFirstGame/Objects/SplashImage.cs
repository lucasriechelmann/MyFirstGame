using Microsoft.Xna.Framework.Graphics;
using MyFirstGame.Objects.Base;

namespace MyFirstGame.Objects;

public class SplashImage : BaseGameObject
{
    public SplashImage(Texture2D texture)
    {
        _texture = texture;
    }
}
