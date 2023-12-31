﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MyFirstGame.Engine.Objects;

namespace MyFirstGame.Objects;

public class BulletSprite : BaseGameObject, IGameObjectWithDamage
{
    private const float BULLET_SPEED = 10.0f;

    private const int BBPosX = 9;
    private const int BBPosY = 4;
    private const int BBWidth = 10;
    private const int BBHeight = 22;

    public int Damage => 10;

    public BulletSprite(Texture2D texture)
    {
        _texture = texture;
        AddBoundingBox(new Engine.Objects.Colisions.BoundingBox(new Vector2(BBPosX, BBPosY), BBWidth, BBHeight));
    }

    public void MoveUp(GameTime gameTime) => 
        Position = new Vector2(Position.X, 
            Position.Y - GetAdjustedSpeed(BULLET_SPEED, gameTime));
}
