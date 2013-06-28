using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using FragEngine.Animation;

namespace FragEngine.Entities
{
    [DataContract]
    public abstract class EntityBase
    {
        private bool _initialized;

        protected EntityBase()
            : this( Vector2.Zero, Vector2.Zero )
        {
        }

        protected EntityBase( Vector2 initialLocation, Vector2 initialVelocity )
        {
            Position = initialLocation;
            Velocity = initialVelocity;

            TintColor = Color.White;

            IsAlive = true;

            Settings = new Dictionary<string, string>();
        }

        [DataMember]
        public Vector2 Position { get; set; }

        [DataMember]
        public virtual Dictionary<string, string> Settings { get; set; }

        [IgnoreDataMember]
        public float GravityFactor { get; set; }

        [IgnoreDataMember]
        public bool IsAlive { get; set; }

        [IgnoreDataMember]
        public Rectangle BoundingBox { get; protected set; }

        [IgnoreDataMember]
        public CollisionType Collision { get; set; }

        [IgnoreDataMember]
        public AnimationSheet Animations { get; set; }

        [IgnoreDataMember]
        public Vector2 Velocity { get; set; }

        [IgnoreDataMember]
        public Vector2 MaxVelocity { get; set; }

        [IgnoreDataMember]
        public float Acceleration { get; set; }

        [IgnoreDataMember]
        public Color TintColor { get; set; }

        [IgnoreDataMember]
        public float Alpha { get; set; }

        [IgnoreDataMember]
        public int Health { get; set; }

        [IgnoreDataMember]
        public string CurrentAnimation
        {
            get
            {
                if( Animations.CurrentAnimation != null )
                {
                    return Animations.CurrentAnimation.Name;
                }
                else
                {
                    return String.Empty;
                }
            }
            set
            {
                Animations.SetCurrentAnimation(value);
            }
        }

        public virtual void Update( GameTime time )
        {
            if (IsAlive)
            {
                if( !_initialized )
                {
                    Initialize();
                    _initialized = true;
                }

                Velocity = new Vector2(
                    Velocity.X,
                    (float)( Velocity.Y + FragEngineGame.Gravity * time.ElapsedGameTime.TotalMilliseconds * GravityFactor )
                );

                Position += Velocity;
                Animations.CurrentAnimation.Update( time );
            }
        }

        protected virtual void Initialize()
        {
            // can be overriden in children to perform any one-time setup
        }

        public virtual void Draw( SpriteBatch batch )
        {
            if (IsAlive)
            {
                if( !_initialized )
                {
                    Initialize();
                    _initialized = true;
                }
                Animations.CurrentAnimation.Draw( batch, Position, Alpha );
            }
        }

        public virtual void ReceiveDamage( EntityBase from, int amount )
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

        public virtual bool CollidesWith( EntityBase other )
        {
            var otherRect = new Rectangle( (int)other.Position.X, (int)other.Position.Y, other.BoundingBox.Width, other.BoundingBox.Height );

            return CollidesWith( otherRect );
        }

        public virtual bool CollidesWith( Rectangle other )
        {
            if (!IsAlive)
            {
                return false;
            }

            if( BoundingBox.IsEmpty )
            {
                BoundingBox = new Rectangle( 0, 0, (int)Animations.FrameSize.X, (int)Animations.FrameSize.Y );
            }

            var currentRect = new Rectangle( (int)Position.X, (int)Position.Y, BoundingBox.Width, BoundingBox.Height );

            return currentRect.Intersects( other );
        }

        public virtual void Kill()
        {
            IsAlive = false;
        }

        public override string ToString()
        {
            return string.Format("{0} ( {1} )", this.GetType().Name, this.GetType().Namespace);
        }
    }
}
