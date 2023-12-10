﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MyFirstGame.Enums;

namespace MyFirstGame.Objects.Base;

public abstract class BaseGameObject
{
    protected Texture2D _texture;

    protected Vector2 _position = Vector2.One;
    public int zIndex { get; set; }
    public int Width => _texture.Width;
    public int Height => _texture.Height;
    public Vector2 Position
    {
        get { return _position; }
        set { _position = value; }
    }
    public virtual void OnNotify(Events eventType)
    {

    }
    public virtual void Render(SpriteBatch spriteBatch) => spriteBatch.Draw(_texture, _position, Color.White);
}
