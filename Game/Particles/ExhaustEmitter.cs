using Engine2D.Particles;
using Engine2D.Particles.EmitterTypes;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Game.Particles;

public class ExhaustEmitter : Emitter
{
    private const int NbParticles = 10;
    private const int MaxParticles = 1000;
    private static Vector2 Direction = new Vector2(0.0f, 1.0f); // pointing downward
    private const float Spread = 1.5f;

    public ExhaustEmitter(Texture2D texture) :
        base(texture, new ExhaustParticleState(), new ConeEmitterType(Direction, Spread), NbParticles, MaxParticles)
    { }
}
