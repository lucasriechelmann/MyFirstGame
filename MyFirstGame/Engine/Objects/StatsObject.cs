using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;

namespace MyFirstGame.Engine.Objects;

public class StatsObject : BaseTextObject
{
    public const int ROLLING_SIZE = 60;
    private Queue<float> _rollingFPS = new Queue<float>();

    public float FPS { get; set; }
    public float MinFPS { get; set; }
    public float MaxFPS { get; set; }
    public float AverageFPS { get; set; }
    public bool IsRunningSlowly { get; set; }
    public int NumberUpdateCalled { get; set; }
    public int NumberDrawCalled { get; set; }
    public StatsObject(SpriteFont font) : base(font)
    {
        NumberUpdateCalled = 0;
        NumberDrawCalled = 0;
    }
    public void Update(GameTime gameTime)
    {
        IsRunningSlowly = gameTime.IsRunningSlowly;
        NumberUpdateCalled++;
        FPS = 1.0f / (float)gameTime.ElapsedGameTime.TotalSeconds;
        _rollingFPS.Enqueue(FPS);
        if (_rollingFPS.Count > ROLLING_SIZE)
        {
            _rollingFPS.Dequeue();
            var sum = 0.0f;
            MaxFPS = int.MinValue;
            MinFPS = int.MaxValue;
            foreach (var fps in _rollingFPS.ToArray())
            {
                sum += fps;
                MaxFPS = Math.Max(MaxFPS, fps);
                MinFPS = Math.Min(MinFPS, fps);
            }
            AverageFPS = sum / _rollingFPS.Count;
            return;
        }

        AverageFPS = FPS;
        MaxFPS = FPS;
        MinFPS = FPS;
    }
    public override void Render(SpriteBatch spriteBatch)
    {
        NumberDrawCalled++;
        var sb = new StringBuilder();
        sb.AppendLine($"FPS: {FPS:0.00}");
        sb.AppendLine($"Min FPS: {MinFPS:0.00}");
        sb.AppendLine($"Max FPS: {MaxFPS:0.00}");
        sb.AppendLine($"Avg FPS: {AverageFPS:0.00}");
        sb.AppendLine($"Is Running Slowly: {IsRunningSlowly}");
        sb.AppendLine($"Nb Update Called: {NumberUpdateCalled}");
        sb.AppendLine($"Nb DrawCalled: {NumberDrawCalled}");
        Text = sb.ToString();
        base.Render(spriteBatch);
    }
}
