using Microsoft.Xna.Framework.Graphics;
using MyFirstGame.Objects.Base;

namespace MyFirstGame.Objects;

public class PlayerSprite : BaseGameObject
{
    public PlayerSprite(Texture2D texture)
    {
        _texture = texture;
    }
}
