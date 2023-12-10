using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MyFirstGame.Engine.Objects;

namespace MyFirstGame.Objects;

public class PlayerSprite : BaseGameObject
{
    private const float PLAYER_SPEED = 10.0f;

    public PlayerSprite(Texture2D texture)
    {
        _texture = texture;
    }

    public void MoveLeft()
    {
        Position = new Vector2(Position.X - PLAYER_SPEED, Position.Y);
    }

    public void MoveRight()
    {
        Position = new Vector2(Position.X + PLAYER_SPEED, Position.Y);
    }
    public void MoveUp()
    {
        Position = new Vector2(Position.X, Position.Y - PLAYER_SPEED);
    }
    public void MoveDown()
    {
        Position = new Vector2(Position.X, Position.Y + PLAYER_SPEED);
    }
}
