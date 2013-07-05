using System.Runtime.Serialization;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace FragEngine.Entities
{
    // actors are game objects that can:
    // * respond to use input
    // * play one or more animations
    [DataContract]
    public abstract class Actor : Entity
    {
        public Actor() : this(Vector2.Zero, Vector2.Zero ) {}

        public Actor( Vector2 initialPosition, Vector2 initialVelocity )
            : base( initialPosition, initialVelocity )
        {

        }

        [IgnoreDataMember]
        public bool FlipAnimation { get; set; }

        public override void Draw( SpriteBatch batch )
        {
            Animations.CurrentAnimation.FlipX = FlipAnimation;

            base.Draw( batch );
        }
    }
}
