using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using FragEngine.Mapping;
using FragEngine.Services;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using FragEngine.Animation;

namespace FragEngine.Entities
{
    [DataContract]
    public abstract class EntityBase
    {
        private bool _initialized;
        protected ICollisionService CollisionService;
        protected IEntityService EntityService;

        protected EntityBase()
            : this( Vector2.Zero, Vector2.Zero )
        {
        }

        protected EntityBase( Vector2 initialLocation, Vector2 initialVelocity )
        {
            Position = initialLocation;
            Velocity = initialVelocity;

            TintColor = Color.White;

            Settings = new Dictionary<string, string>();

            // FIXES: bug where entities were invisible in fragEd.
            Alpha = 255f; // entities are visible by default
            IsAlive = true;

            // by default, gravity will affect entities
            GravityFactor = 1f;
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

        [IgnoreDataMember]
        protected bool Standing { get; set; }

        public virtual void Update( GameTime time )
        {
            if (IsAlive)
            {
                if( !_initialized )
                {
                    Initialize();
                    InternalInitialize();
                    _initialized = true;
                }

                var gravityVector = new Vector2( 0, FragEngineGame.Gravity * (float)time.ElapsedGameTime.TotalSeconds * GravityFactor );

                // modify our velocity with our gravity vector
                Velocity += gravityVector;

                // ask the collision system if we're going to have a collision at that co-ord
                var result = CollisionService.Check( Position, Velocity, Animations.CurrentAnimation.FrameSize );

                UpdateEntityState( result );

                Position = result.Position;

                Animations.CurrentAnimation.Update( time );

                BoundingBox = new Rectangle( (int)Position.X, (int)Position.Y, BoundingBox.Width, BoundingBox.Height );
            }
        }

        private void UpdateEntityState(CollisionCheckResult result)
        {
            Standing = false;
            if( result.YAxis )
            {
                Standing = Velocity.Y > 0;
            }
        }

        private void InternalInitialize()
        {
            CollisionService = ServiceInjector.Get<ICollisionService>();
            EntityService = ServiceInjector.Get<IEntityService>();

            if( BoundingBox.Width == 0 || BoundingBox.Height == 0 )
            {
                BoundingBox = new Rectangle( 0, 0, (int)Animations.FrameSize.X, (int)Animations.FrameSize.Y );
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
                    InternalInitialize();
                    _initialized = true;
                }
                Animations.CurrentAnimation.Draw( batch, Position, Alpha );

#if DEBUG
                var WhiteTexture = new Texture2D( FragEngineGame.Graphics.GraphicsDevice, 1, 1 );
                WhiteTexture.SetData( new Color[] { Color.White } );

                batch.Draw( WhiteTexture, new Rectangle( BoundingBox.Left,  BoundingBox.Top,    BoundingBox.Width, 1),   Color.White);
                batch.Draw( WhiteTexture, new Rectangle( BoundingBox.Left,  BoundingBox.Bottom, BoundingBox.Width, 1),   Color.White);
                batch.Draw( WhiteTexture, new Rectangle( BoundingBox.Left,  BoundingBox.Top, 1, BoundingBox.Height),     Color.White);
                batch.Draw( WhiteTexture, new Rectangle( BoundingBox.Right, BoundingBox.Top, 1, BoundingBox.Height + 1), Color.White);
#endif
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
