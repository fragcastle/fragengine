using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using FragEngine.Collisions;
using FragEngine.Services;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using FragEngine.Animation;

namespace FragEngine.Entities
{
    [DataContract]

    // TODO: add distanceTo, angleTo, touches calculations
    // TODO: add bounciness
    // TODO: add support for entities with no animations (logical entities)
    //          thinking that an "entity" with animations is an Actor.
    public abstract class GameObject
    {
        
        protected ICollisionService CollisionService;

        protected GameObject()
            : this( Vector2.Zero )
        {
        }

        protected GameObject( Vector2 initialLocation, Vector2? initialVelocity = null, ICollisionService collisionService = null )
        {
            Position = initialLocation;

            Settings = new Dictionary<string, string>();
            
            IsAlive = true;

            BoundingBoxOffset = Vector2.Zero;

            Velocity = initialVelocity ?? Vector2.Zero;

            // by default, gravity will affect entities
            GravityFactor = 1f;

            // entities are moved by changing the acceleration vector...
            Acceleration = Vector2.Zero;

            CollisionService = collisionService ?? ServiceLocator.Get<ICollisionService>();
        }

        [DataMember]
        public Vector2 Position { get; set; }

        [DataMember]
        public virtual IDictionary<string, string> Settings { get; set; }

        [IgnoreDataMember]
        public bool IsAlive { get; set; }

        [IgnoreDataMember]
        protected bool IsStanding { get; set; }

        [IgnoreDataMember]
        public Rectangle BoundingBox { get; set; }

        [IgnoreDataMember]
        public Vector2 BoundingBoxOffset { get; set; }

        [IgnoreDataMember]
        public CollisionType Collision { get; set; }

        // Physics Properties       

        [IgnoreDataMember]
        public float GravityFactor { get; set; }

        [IgnoreDataMember]
        public Vector2 Velocity { get; set; }

        [IgnoreDataMember]
        public Vector2 MaxVelocity { get; set; }

        [IgnoreDataMember]
        public Vector2 Acceleration { get; set; }

        [IgnoreDataMember]
        public Vector2 Friction { get; set; }

        public virtual void Kill()
        {
            IsAlive = false;
        }

        public float DistanceTo( GameObject gameObject )
        {
            var xd = ( Position.X + BoundingBox.X / 2 ) - ( gameObject.Position.X + gameObject.BoundingBox.X / 2 );
            var yd = ( Position.Y + BoundingBox.Y / 2 ) - ( gameObject.Position.Y + gameObject.BoundingBox.Y / 2 );
            return (float)Math.Sqrt( xd * xd + yd * yd );
        }

        public override string ToString()
        {
            return string.Format("{0} ( {1} )", this.GetType().Name, this.GetType().Namespace);
        }

        public virtual void Update( GameTime gameTime )
        {
            
        }

        public virtual void Draw( SpriteBatch spriteBatch )
        {
            
        }
    }
}
