using Engine2D.Particles;
using Engine2D.Particles.EmitterTypes;
using Microsoft.Xna.Framework.Graphics;

namespace Game.Particles;

public class ExplosionEmitter : Emitter
{
    private const int NbParticles = 2;
    private const int MaxParticles = 200;
    private const float Radius = 50f;

    public ExplosionEmitter(Texture2D texture) :
        base(texture, new ExplosionParticleState(), new CircleEmitterType(Radius), NbParticles, MaxParticles)
    { }
}