using System;
using System.Runtime.Serialization;
using FragEngine.Animation;
using FragEngine.Collisions;
using FragEngine.Services;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace FragEngine.Entities
{
    // actors are game objects that can:
    // * respond to use input
    // * play one or more animations
    [DataContract]
    public abstract class Actor : GameObject
    {
        private bool _initialized;

        [IgnoreDataMember]
        public AnimationSheet Animations { get; set; }

        [IgnoreDataMember]
        public Color TintColor { get; set; }

        [IgnoreDataMember]
        public float Alpha { get; set; }

        [IgnoreDataMember]
        public int Health { get; set; }

        [IgnoreDataMember]
        public bool FlipAnimation { get; set; }

        [IgnoreDataMember]
        public string CurrentAnimation
        {
            get
            {
                var val = String.Empty;
                if( Animations.CurrentAnimation != null )
                    val = Animations.CurrentAnimation.Name;
                return val;
            }
            set
            {
                Animations.SetCurrentAnimation( value );
            }
        }

        [IgnoreDataMember]
        public Vector2 Size
        {
            get
            {
                // Returns the FrameSize of the CurrentAnimation
                return Animations == null ? new Vector2( 16, 16 ) : Animations.CurrentAnimation.FrameSize; 
            }
        }

        public Actor() : this( Vector2.Zero ) {}
        public Actor( Vector2 initialPosition, Vector2? initialVelocity = null, ICollisionService collisionService = null )
            : base( initialPosition, initialVelocity, collisionService )
        {
            TintColor = Color.White;

            // FIXES: bug where entities were invisible in fragEd.
            Alpha = 255f; // entities are visible by default
        }        

        public override void Update( GameTime gameTime )
        {            
            Animations.CurrentAnimation.Update( gameTime );

            base.Update( gameTime );
        }

        public override void Draw( SpriteBatch batch )
        {
            Animations.CurrentAnimation.FlipX = FlipAnimation;

            var correctedPosition = Position - Offset;

            Animations.CurrentAnimation.Draw( batch, correctedPosition, Alpha );

            base.Draw( batch );
        }

        public virtual void ReceiveDamage( GameObject from, int amount )
        {
            if( Health < amount )
            {
                Health = 0;
                Kill();
            }
            else
            {
                Health -= amount;
            }
        }   
    }
}
