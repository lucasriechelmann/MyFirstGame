using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MyFirstGame.Engine.Particles;
using MyFirstGame.Engine.Particles.EmitterTypes;

namespace MyFirstGame.Particles;

public class ExplosionEmitter : Emitter
{
    private const int NbParticles = 2;
    private const int MaxParticles = 200;
    private const float Radius = 50f;

    public ExplosionEmitter(Texture2D texture, Vector2 position) :
        base(texture, position, new ExplosionParticleState(), new CircleEmitterType(Radius), NbParticles, MaxParticles)
    { }
}
