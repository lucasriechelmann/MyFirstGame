﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MyFirstGame.Engine.Objects;
using MyFirstGame.Engine.States;
using MyFirstGame.States.Gameplay;
using System.Collections.Generic;

namespace MyFirstGame.Objects;

public class ChopperSprite : BaseGameObject
{
    private const float Speed = 4.0f;
    private const float _bladeSpeed = 0.2f;
    private float BladeSpeed = 0.2f;

    // which chopper do we want from the texture
    private const int ChopperStartX = 0;
    private const int ChopperStartY = 0;
    private const int ChopperWidth = 44;
    private const int ChopperHeight = 98;

    // where are the blades on the texture
    private const int BladesStartX = 133;
    private const int BladesStartY = 98;
    private const int BladesWidth = 94;
    private const int BladesHeight = 94;

    // rotation center of the blades
    private const float BladesCenterX = 47.5f;
    private const float BladesCenterY = 47.5f;

    // positioning of the blades on the chopper
    private const int ChopperBladePosX = ChopperWidth / 2;
    private const int ChopperBladePosY = 34;

    // initial direction and speed of chopper
    private float _angle = 0.0f;
    private Vector2 _direction = new Vector2(0, 0);

    // track life total and age of chopper
    private int _age = 0;
    private int _life = 40;

    // chopper will flash red when hit
    private int _hitAt = 100;

    // bounding box. Note that since this chopper is rotated 180 degrees around its 0,0 origin, 
    // this causes the bounding box to be further to the left and higher than the original texture coordinates
    private int BBPosX = -16;
    private int BBPosY = -63;
    private int BBWidth = 34;
    private int BBHeight = 98;

    private List<(int, Vector2)> _path;

    public ChopperSprite(Texture2D texture, List<(int, Vector2)> path)
    {
        _texture = texture;
        _path = path;
        AddBoundingBox(new Engine.Objects.Colisions.BoundingBox(new Vector2(BBPosX, BBPosY), BBWidth, BBHeight));
    }

    public void Update(GameTime gameTime)
    {
        // Choppers follow a path where the direction changes at a certain frame, which is tracked by the chopper's age
        foreach (var p in _path)
        {
            int pAge = p.Item1;
            Vector2 pDirection = p.Item2;

            if (_age > pAge)
            {
                _direction = pDirection;
            }
        }

        var speed = GetAdjustedSpeed(Speed, gameTime);

        Position = Position + (_direction * speed);

        _age++;
        BladeSpeed = GetAdjustedSpeed(_bladeSpeed, gameTime);
    }

    public override void Render(SpriteBatch spriteBatch)
    {
        var chopperRect = new Rectangle(ChopperStartX, ChopperStartY, ChopperWidth, ChopperHeight);
        var chopperDestRect = new Rectangle(_position.ToPoint(), new Point(ChopperWidth, ChopperHeight));

        var bladesRect = new Rectangle(BladesStartX, BladesStartY, BladesWidth, BladesHeight);
        var bladesDestRect = new Rectangle(_position.ToPoint(), new Point(BladesWidth, BladesHeight));

        // if the chopper was just hit and is flashing, Color should alternate between OrangeRed and White
        var color = GetColor();
        spriteBatch.Draw(_texture, chopperDestRect, chopperRect, color, MathHelper.Pi, new Vector2(ChopperBladePosX, ChopperBladePosY), SpriteEffects.None, 0f);
        spriteBatch.Draw(_texture, bladesDestRect, bladesRect, Color.White, _angle, new Vector2(BladesCenterX, BladesCenterY), SpriteEffects.None, 0f);

        _angle += BladeSpeed;
    }

    public override void OnNotify(BaseGameStateEvent gameEvent)
    {
        switch (gameEvent)
        {
            case GameplayEvents.ObjectHitBy m:
                JustHit(m.HitBy);
                SendEvent(new GameplayEvents.ObjectLostLife(_life));
                break;
        }
    }

    private void JustHit(IGameObjectWithDamage o)
    {
        _hitAt = 0;
        _life -= o.Damage;
    }

    private Color GetColor()
    {
        var color = Color.White;
        foreach (var flashStartEndFrames in GetFlashStartEndFrames())
        {
            if (_hitAt >= flashStartEndFrames.Item1 && _hitAt < flashStartEndFrames.Item2)
            {
                color = Color.OrangeRed;
            }
        }

        _hitAt++;
        return color;
    }

    private List<(int, int)> GetFlashStartEndFrames()
    {
        return new List<(int, int)>
            {
                (0, 3),
                (10, 13)
            };
    }
}
