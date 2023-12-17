using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MyFirstGame.Engine.Objects.Colisions;
using MyFirstGame.Engine.States;
using System;
using System.Collections.Generic;

namespace MyFirstGame.Engine.Objects;

public abstract class BaseGameObject
{
    protected Texture2D _texture;
    protected Texture2D _boundingBoxTexture;
    protected Vector2 _position = Vector2.One;
    protected float _angle;
    protected Vector2 _direction;
    protected List<Colisions.BoundingBox> _boundingBoxes = new List<Colisions.BoundingBox>();
    public int zIndex { get; set; }
    public event EventHandler<BaseGameStateEvent> OnObjectChanged;

    public bool Destroyed { get; private set; }
    public virtual int Width => _texture?.Width ?? 0;
    public virtual int Height => _texture?.Height ?? 0;
    public virtual Vector2 Position
    {
        get => _position;
        set
        {
            var deltaX = value.X - _position.X;
            var deltaY = value.Y - _position.Y;
            _position = value;

            foreach (var bb in _boundingBoxes)
            {
                bb.Position = new Vector2(bb.Position.X + deltaX, bb.Position.Y + deltaY);
            }
        }
    }
    public List<Colisions.BoundingBox> BoundingBoxes => _boundingBoxes;
    public virtual void OnNotify(BaseGameStateEvent gameEvent)
    {

    }
    public virtual void Render(SpriteBatch spriteBatch)
    {
        if(Destroyed)
            return;

        spriteBatch.Draw(_texture, _position, Color.White);
    }
    public void RenderBoundingBoxes(SpriteBatch spriteBatch)
    {
        if (Destroyed)
            return;

        if (_boundingBoxTexture == null)
        {
            CreateBoundingBoxTexture(spriteBatch.GraphicsDevice);
        }

        foreach (var bb in _boundingBoxes)
        {
            spriteBatch.Draw(_boundingBoxTexture, bb.Rectangle, Color.Red);
        }
    }
    public void Destroy() => Destroyed = true;
    public void SendEvent(BaseGameStateEvent e) => OnObjectChanged?.Invoke(this, e);

    public void AddBoundingBox(Colisions.BoundingBox bb) => _boundingBoxes.Add(bb);
    protected Vector2 CalculateDirection(float angleOffset = 0.0f)
    {
        _direction = new Vector2((float)Math.Cos(_angle - angleOffset), (float)Math.Sin(_angle - angleOffset));
        _direction.Normalize();

        return _direction;
    }

    private void CreateBoundingBoxTexture(GraphicsDevice graphicsDevice)
    {
        _boundingBoxTexture = new Texture2D(graphicsDevice, 1, 1);
        _boundingBoxTexture.SetData<Color>(new Color[] { Color.White });
    }
}
