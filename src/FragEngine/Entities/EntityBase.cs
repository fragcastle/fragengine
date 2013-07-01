﻿using System;
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

    // TODO: add distanceTo, angleTo, touches calculations
    // TODO: add bounciness
    // TODO: add support for entities with no animations (logical entities)
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

            // entities are moved by changing the acceleration vector...
            Acceleration = Vector2.Zero;
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
        public Vector2 Acceleration { get; set; }

        [IgnoreDataMember]
        public Vector2 Friction { get; set; }

        [IgnoreDataMember]
        public Color TintColor { get; set; }

        [IgnoreDataMember]
        public float Alpha { get; set; }

        [IgnoreDataMember]
        public int Health { get; set; }

        [IgnoreDataMember]
        protected bool Standing { get; set; }

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



        public virtual void Update( GameTime gameTime )
        {
            if (IsAlive)
            {
                if( !_initialized )
                {
                    Initialize();
                    InternalInitialize();
                    _initialized = true;
                }

                CalculateVelocity( gameTime );

                var partialVelocity = Velocity * (float)gameTime.ElapsedGameTime.TotalSeconds;

                // ask the collision system if we're going to have a collision at that co-ord
                var result = CollisionService.Check( Position, partialVelocity, Animations.CurrentAnimation.FrameSize );

                UpdateEntityState( result );

                Position = result.Position;

                Animations.CurrentAnimation.Update( gameTime );

                BoundingBox = new Rectangle( (int)Position.X, (int)Position.Y, BoundingBox.Width, BoundingBox.Height );
            }
        }

        private void CalculateVelocity( GameTime gameTime )
        {
            var tick = (float)gameTime.ElapsedGameTime.TotalMilliseconds;

            // apply gravity
            var gravityVector = new Vector2( 0, FragEngineGame.Gravity * (float)gameTime.ElapsedGameTime.TotalSeconds * GravityFactor );

            Velocity += gravityVector;

            // apply acceleration or friction
            if( Acceleration != Vector2.Zero )
            {
                Velocity += (Acceleration * tick);
            } else if ( Friction != Vector2.Zero )
            {
                var delta = Friction * tick;

                var subtract = Velocity - delta;
                var add = Velocity + delta;

                if( subtract.X > 0 )
                    Velocity -= new Vector2( delta.X, 0 );
                else if( add.X < 0)
                    Velocity += new Vector2( delta.X, 0 );
                else Velocity = new Vector2( 0, Velocity.Y );

                if( subtract.Y > 0 )
                    Velocity -= new Vector2( 0, delta.Y );
                else if( add.Y < 0)
                    Velocity += new Vector2( 0, delta.Y );
                else Velocity = new Vector2( Velocity.X, 0 );
            }

            Velocity = Utility.Limit( Velocity, MaxVelocity );
        }

        protected virtual void UpdateEntityState(CollisionCheckResult result)
        {
            Standing = false;
            if( result.YAxis )
            {
                Standing = Velocity.Y > 0;
            }

            if( result.XAxis )
            {
                Velocity = Vector2.Zero;
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
