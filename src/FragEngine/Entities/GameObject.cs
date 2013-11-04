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

        private IDictionary<string, string> _settings = new Dictionary<string, string>();

        protected GameObject()
            : this( Vector2.Zero )
        {
        }

        protected GameObject( Vector2 initialLocation, Vector2? initialVelocity = null, ICollisionService collisionService = null )
        {
            Position = initialLocation;
            
            IsAlive = true;

            Offset = Vector2.Zero;

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
        public IDictionary<string, string> Settings
        {
            get { return _settings; }
            set { _settings = value; }
        }

        [IgnoreDataMember]
        public bool IsAlive { get; set; }

        [IgnoreDataMember]
        public bool IsStanding { get; set; } 

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

        // Collision Properties

        [IgnoreDataMember]
        public CollisionType Collision { get; set; }

        [IgnoreDataMember]
        public HitBox BoundingBox { get; set; }

        [IgnoreDataMember]
        public Vector2 Offset { get; set; }

        
        
        public virtual void Kill()
        {
            IsAlive = false;
        }

        public float DistanceTo( GameObject gameObject )
        {
            var xd = ( BoundingBox.Width / 2 ) - ( gameObject.BoundingBox.Width / 2 );
            var yd = ( BoundingBox.Height / 2 ) - ( gameObject.BoundingBox.Height / 2 );
            return (float)Math.Sqrt( xd * xd + yd * yd );
        }

        public override string ToString()
        {
            return string.Format("{0} ( {1} )", this.GetType().Name, this.GetType().Namespace);
        }

        public virtual void Update( GameTime gameTime )
        {
            CalculateVelocity( gameTime );

            var partialVelocity = Velocity * gameTime.GetGameTick();

            // ask the collision system if we're going to have a collision at that co-ord
            var result = CollisionService.Check( Position, partialVelocity, BoundingBox );

            UpdateEntityState( result );
        }

        public virtual void Draw( SpriteBatch batch )
        {
            string drawBox = null;
            if( Settings.TryGetValue("_feDrawBox", out drawBox) && drawBox == "true" )
            {
                var whiteTexture = new Texture2D( batch.GraphicsDevice, 1, 1 );
                whiteTexture.SetData( new Color[] { Color.White } );

                Rectangle rect = BoundingBox;

                rect.X = (int)Position.X;
                rect.Y = (int)Position.Y;

                batch.Draw( whiteTexture, new Rectangle( rect.Left, rect.Top, rect.Width, 1 ), Color.White );
                batch.Draw( whiteTexture, new Rectangle( rect.Left, rect.Bottom, rect.Width, 1 ), Color.White );
                batch.Draw( whiteTexture, new Rectangle( rect.Left, rect.Top, 1, rect.Height ), Color.White );
                batch.Draw( whiteTexture, new Rectangle( rect.Right, rect.Top, 1, rect.Height + 1 ), Color.White );
            }
        }

        protected virtual void UpdateEntityState( CollisionCheckResult result )
        {
            IsStanding = false;
            if( result.YAxis )
            {
                IsStanding = Velocity.Y > 0;
                Velocity = new Vector2( Velocity.X, 0 );
            }

            if( result.XAxis )
                Velocity = new Vector2( 0, Velocity.Y );

            Position = result.Position;
        }

        private void CalculateVelocity( GameTime gameTime )
        {
            var tick = gameTime.GetGameTick();

            // apply gravity
            var gravityVector = new Vector2( 0, FragEngineGame.Gravity * tick * GravityFactor );

            Velocity += gravityVector;

            // are we speeding up, or slowing down?
            if( Acceleration != Vector2.Zero )
                Velocity = ApplyAcceleration( gameTime );
            else if( Friction != Vector2.Zero )
                Velocity = ApplyFriction( gameTime );
        }

        private Vector2 ApplyFriction( GameTime gameTime )
        {
            var frictionDelta = Friction * gameTime.GetGameTick();

            var newVelocity = Velocity;

            if( Velocity.X - frictionDelta.X > 0 ) newVelocity.X -= frictionDelta.X;
            else if( Velocity.X + frictionDelta.X < 0 ) newVelocity.X += frictionDelta.X;
            else newVelocity.X = 0;

            if( Velocity.Y - frictionDelta.Y > 0 ) newVelocity.Y -= frictionDelta.Y;
            else if( Velocity.Y + frictionDelta.Y < 0 ) newVelocity.Y += frictionDelta.Y;
            else newVelocity.Y = 0;

            return Utility.Limit( newVelocity, MaxVelocity );
        }

        private Vector2 ApplyAcceleration( GameTime gameTime )
        {
            return Velocity = Utility.Limit( Velocity + Acceleration * gameTime.GetGameTick(), MaxVelocity );
        }
    }
}
