using System;
using System.Runtime.Serialization;
using FragEngine.Animation;
using FragEngine.Mapping;
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
        protected bool Standing { get; set; }

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

        public Actor() : this(Vector2.Zero, Vector2.Zero ) {}

        public Actor( Vector2 initialPosition, Vector2 initialVelocity, ICollisionService collisionSvc = null )
            : base( initialPosition )
        {
            TintColor = Color.White;

            // FIXES: bug where entities were invisible in fragEd.
            Alpha = 255f; // entities are visible by default

            Velocity = initialVelocity;

            // by default, gravity will affect entities
            GravityFactor = 1f;

            // entities are moved by changing the acceleration vector...
            Acceleration = Vector2.Zero;

            CollisionService = collisionSvc ?? ServiceLocator.Get<ICollisionService>();
        }        

        public override void Update( GameTime gameTime )
        {            
            CalculateVelocity( gameTime );

            var partialVelocity = Velocity * gameTime.GetGameTick();

            // ask the collision system if we're going to have a collision at that co-ord
            var result = CollisionService.Check( Position, partialVelocity, Animations.CurrentAnimation.FrameSize );

            UpdateEntityState( result );

            Animations.CurrentAnimation.Update( gameTime );

            BoundingBox = new Rectangle( (int)( Position.X + BoundingBoxOffset.X ), (int)( Position.Y + BoundingBoxOffset.Y ), BoundingBox.Width, BoundingBox.Height );

            base.Update( gameTime );
        }

        public override void Draw( SpriteBatch batch )
        {
            Animations.CurrentAnimation.FlipX = FlipAnimation;

            var correctedPosition = Position + BoundingBoxOffset;

            Animations.CurrentAnimation.Draw( batch, correctedPosition, Alpha );

#if DEBUG
            batch.Draw( Primitives.WhiteTexture, new Rectangle( BoundingBox.Left, BoundingBox.Top, BoundingBox.Width, 1 ), Color.White );
            batch.Draw( Primitives.WhiteTexture, new Rectangle( BoundingBox.Left, BoundingBox.Bottom, BoundingBox.Width, 1 ), Color.White );
            batch.Draw( Primitives.WhiteTexture, new Rectangle( BoundingBox.Left, BoundingBox.Top, 1, BoundingBox.Height ), Color.White );
            batch.Draw( Primitives.WhiteTexture, new Rectangle( BoundingBox.Right, BoundingBox.Top, 1, BoundingBox.Height + 1 ), Color.White );
#endif

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

        protected virtual void UpdateEntityState( CollisionCheckResult result )
        {
            Standing = false;
            if( result.YAxis )
            {
                Standing = Velocity.Y > 0;
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
