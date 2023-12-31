﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MyFirstGame.Engine.Objects;
using System;

namespace MyFirstGame.Objects
{
    public class TurretBulletSprite : BaseGameObject
    {
        private const float BULLET_SPEED = 18.0f;
        private Vector2 _bulletCenterPosition;

        public Segment CollisionSegment
        {
            get
            {
                var segment = _direction * _texture.Height;
                return new Segment(_position, Vector2.Add(_position, segment));
            }
        }

        public TurretBulletSprite(Texture2D texture, Vector2 direction, float angle)
        {
            _texture = texture;
            _direction = direction;
            _direction.Normalize();

            _bulletCenterPosition = new Vector2(_texture.Width / 2, _texture.Height / 2);
            _angle = angle;
        }

        public void Update(GameTime gameTime) => Position = Position + _direction * GetAdjustedSpeed(BULLET_SPEED, gameTime);
        public override void Render(SpriteBatch spriteBatch) =>
            spriteBatch.Draw(_texture, 
                _position, 
                _texture.Bounds, 
                Color.White, 
                _angle, 
                _bulletCenterPosition, 
                1f, 
                SpriteEffects.None, 
                0f);
    }
}
