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

        [IgnoreDataMember]
        public Rectangle CollisionBox
        {
            get
            {
                Rectangle rect = BoundingBox - Offset;

                rect.X = (int)Position.X;
                rect.Y = (int)Position.Y;

                return rect;
            }
        }
        
        public virtual void Kill()
        {
            IsAlive = false;
        }

        public float DistanceTo( GameObject gameObject )
        {
            var xd = ( CollisionBox.X / 2 ) - ( gameObject.CollisionBox.X / 2 );
            var yd = ( CollisionBox.Y / 2 ) - ( gameObject.CollisionBox.Y / 2 );
            return (float)Math.Sqrt( xd * xd + yd * yd );
        }

        public override string ToString()
        {
            return string.Format("{0} ( {1} )", this.GetType().Name, this.GetType().Namespace);
        }

        public virtual void Update( GameTime gameTime )
        {
            
        }

        public virtual void Draw( SpriteBatch batch )
        {
            string drawBox = null;
            if( Settings.TryGetValue("_feDrawBox", out drawBox) && drawBox == "true" )
            {
                var whiteTexture = new Texture2D( batch.GraphicsDevice, 1, 1 );
                whiteTexture.SetData( new Color[] { Color.White } );

                batch.Draw( whiteTexture, new Rectangle( CollisionBox.Left,  CollisionBox.Top,    CollisionBox.Width, 1 ), Color.White );
                batch.Draw( whiteTexture, new Rectangle( CollisionBox.Left,  CollisionBox.Bottom, CollisionBox.Width, 1 ), Color.White );
                batch.Draw( whiteTexture, new Rectangle( CollisionBox.Left,  CollisionBox.Top, 1, CollisionBox.Height ), Color.White );
                batch.Draw( whiteTexture, new Rectangle( CollisionBox.Right, CollisionBox.Top, 1, CollisionBox.Height + 1 ), Color.White );
            }
        }
    }
}
