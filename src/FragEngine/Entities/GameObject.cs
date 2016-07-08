﻿using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Runtime.Serialization;
using FragEngine.Collisions;
using FragEngine.Services;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using FragEngine.Animation;

namespace FragEngine.Entities
{
    [DataContract]
    public abstract class GameObject
    {

        private const string DefaultAnimationName = "idle";

        private bool _hasBeenConfigured = false;
        private Texture2D _debugOutline;

        protected ICollisionService CollisionService;

        private IDictionary<string, string> _settings = new Dictionary<string, string>();

        public static void SeperateOnYAxis(GameObject top, GameObject bottom, GameObject weak)
        {
            float nudge = (top.Position.Y + top.BoundingBox.Height - bottom.Position.Y);
            var collisionService = ServiceLocator.Get<ICollisionService>();

            // We have a weak entity, so just move this one
            if (weak != null)
            {
                var strong = top == weak ? bottom : top;
                weak.Velocity = weak.Velocity.SetY(-weak.Velocity.Y*weak.Bounciness + strong.Velocity.Y);

                // Riding on a platform?
                float nudgeX = 0;
                if (weak == top && Math.Abs(weak.Velocity.Y - strong.Velocity.Y) < weak.MinBounceVelocity)
                {
                    weak.IsStanding = true;
                    nudgeX = strong.Velocity.X * FragEngineGame.Tick;
                }

                var resWeak = collisionService.Check(
                    weak.Position,
                    new Vector2(nudgeX, weak == top ? -nudge : nudge),
                    weak.BoundingBox 
                    );

                weak.Position = resWeak.Position;
            }

            // Bottom entity is standing - just bounce the top one
            else if (FragEngineGame.Gravity > 0 && (bottom.IsStanding || top.Velocity.Y > 0))
            {
                var resTop = collisionService.Check(
                    top.Position,
                    new Vector2(0, -(top.Position.Y + top.BoundingBox.Height - bottom.Position.Y)), 
                    top.BoundingBox
                    );

                top.Position = top.Position.SetY(resTop.Position.Y);

                if (top.Bounciness > 0 && top.Velocity.Y > top.MinBounceVelocity)
                {
                    top.Velocity *= new Vector2(1, -top.Bounciness);
                }
                else
                {
                    top.IsStanding = true;
                    top.Velocity = top.Velocity.SetY(0);
                }
            }

            // Normal collision - both move
            else
            {
                var v2 = (top.Velocity.Y - bottom.Velocity.Y) / 2;
                top.Velocity = top.Velocity.SetY(-v2);
                bottom.Velocity = bottom.Velocity.SetY(v2);

                var nudgeX = bottom.Velocity.X * FragEngineGame.Tick;
                var resTop = collisionService.Check(
                    top.Position,
                    new Vector2(nudgeX, -nudge/2),
                    top.BoundingBox 
                    );

                top.Position = top.Position.SetY(resTop.Position.Y);

                var resBottom = collisionService.Check(
                    bottom.Position,
                    new Vector2(0, nudge / 2),
                    bottom.BoundingBox 
                    );

                bottom.Position = bottom.Position.SetY(resBottom.Position.Y);
            }
        }

        public static void SeperateOnXAxis(GameObject left, GameObject right, GameObject weak)
        {
            var nudge = (left.Position.X + left.BoundingBox.Width - right.Position.X);
            var collisionService = ServiceLocator.Get<ICollisionService>();

            // We have a weak entity, so just move this one
            if (weak != null)
            {
                var strong = left == weak ? right : left;
                weak.Velocity = weak.Velocity.SetX(weak.Velocity.X * weak.Bounciness + strong.Velocity.X);

                var resWeak = collisionService.Check(
                    weak.Position,
                    new Vector2(weak == left ? -nudge : nudge, 0),
                    weak.BoundingBox
                    );

                weak.Position = new Vector2(resWeak.Position.X, weak.Position.Y);
            }

            // Normal collision - both move
            else
            {
                var v2 = (left.Velocity.X - right.Velocity.X) / 2;
                left.Velocity = left.Velocity.SetX(-v2);
                right.Velocity = left.Velocity.SetX(v2);

                var resLeft = collisionService.Check(
                    left.Position,
                    new Vector2(-nudge / 2, 0),
                    left.BoundingBox 
                    );

                left.Position = left.Position.SetX(Math.Floor(resLeft.Position.X));

                var resRight = collisionService.Check(
                    right.Position,
                    new Vector2(nudge/2, 0),
                    right.BoundingBox 
                    );

                right.Position = right.Position.SetX(Math.Ceiling(resRight.Position.X));
            }
        }

        public static void SolveCollision(GameObject a, GameObject b)
        {
            // If one entity is FIXED, or the other entity is LITE, the weak
            // (FIXED/NON-LITE) entity won't move in collision response
            GameObject weak = null;
            if (
                a.CollisionStyle == GameObjectCollisionStyle.Lite ||
                b.CollisionStyle == GameObjectCollisionStyle.Fixed
            )
            {
                weak = a;
            }
            else if (
                b.CollisionStyle == GameObjectCollisionStyle.Lite ||
                a.CollisionStyle == GameObjectCollisionStyle.Fixed
            )
            {
                weak = b;
            }


            // Did they already overlap on the X-axis in the last frame? If so,
            // this must be a vertical collision!
            if (
                a.LastPosition.X + a.BoundingBox.Width > b.LastPosition.X &&
                a.LastPosition.X < b.LastPosition.X + b.BoundingBox.Width
            )
            {
                // Which one is on top?
                if (a.LastPosition.Y < b.LastPosition.Y)
                {
                    SeperateOnYAxis(a, b, weak);
                }
                else
                {
                    SeperateOnYAxis(b, a, weak);
                }
                a.CollideWith(b, "y");
                b.CollideWith(a, "y");
            }

            // Horizontal collision
            else if (
                a.LastPosition.Y + a.BoundingBox.Height > b.LastPosition.Y &&
                a.LastPosition.Y < b.LastPosition.Y + b.BoundingBox.Height
            )
            {
                // Which one is on the left?
                if (a.LastPosition.X < b.LastPosition.X)
                {
                    SeperateOnXAxis(a, b, weak);
                }
                else
                {
                    SeperateOnXAxis(b, a, weak);
                }
                a.CollideWith(b, "x");
                b.CollideWith(a, "x");
            }
        }

        public static void CheckPair(GameObject a, GameObject b)
        {
            // Do these entities want checks?
            if (a.CheckAgainstGroup == b.Group)
            {
                a.Check(b);
            }

            if (b.CheckAgainstGroup == a.Group)
            {
                b.Check(a);
            }

            // If this pair allows collision, solve it! At least one entity must
            // collide ACTIVE or FIXED, while the other one must not collide NEVER.
            if (
                a.CollisionStyle != GameObjectCollisionStyle.Never &&
                b.CollisionStyle != GameObjectCollisionStyle.Never &&
                (int)a.CollisionStyle + (int)b.CollisionStyle > (int)GameObjectCollisionStyle.Active
            )
            {
                SolveCollision(a, b);
            }
        }

        public GameObject()
            : this( Vector2.Zero )
        {
            
        }

        public GameObject( Vector2 initialLocation, Vector2? initialVelocity = null, ICollisionService collisionService = null )
        {
            ZIndex = 1;

            TintColor = Color.White;

            // TODO: add lifetime/fadetime to gameobject

            // FIXES: bug where entities were invisible in fragEd.
            Alpha = 255f; // entities are visible by default

            Position = initialLocation;
            
            IsAlive = true;

            Offset = Vector2.Zero;

            MinBounceVelocity = 40;

            Velocity = initialVelocity ?? Vector2.Zero;

            // by default, gravity will affect entities
            GravityFactor = 1f;

            ExternalAcceleration = ExternalFriction = Acceleration = Friction = Vector2.Zero;

            CollisionService = collisionService ?? ServiceLocator.Get<ICollisionService>();
        }

        [DataMember]
        public string Name { get; set; }

        [IgnoreDataMember]
        public AnimationSheet Animations { get; set; }

        [IgnoreDataMember]
        public Color TintColor { get; set; }

        [IgnoreDataMember]
        public float Alpha { get; set; }

        [IgnoreDataMember]
        public float Health { get; set; }

        [IgnoreDataMember]
        public bool FlipAnimation { get; set; }

        [DataMember]
        public int ZIndex { get; set; }

        [IgnoreDataMember]
        public string CurrentAnimation
        {
            get
            {
                var val = String.Empty;
                if (Animations?.CurrentAnimation != null)
                    val = Animations.CurrentAnimation.Name;
                return val;
            }
            set
            {
                Animations.SetCurrentAnimation(value);
            }
        }

        [IgnoreDataMember]
        public Vector2 Size
        {
            get
            {
                // Returns the FrameSize of the CurrentAnimation
                return Animations == null ? new Vector2(16, 16) : Animations.CurrentAnimation.FrameSize;
            }
        }

        [DataMember]
        public Vector2 Position { get; set; }

        [IgnoreDataMember]
        public Vector2 LastPosition { get; private set; }

        [DataMember]
        public IDictionary<string, string> Settings
        {
            get { return _settings; }
            set { _settings = value; }
        }

        [DataMember]
        public float Bounciness { get; set; }

        [DataMember]
        public float MinBounceVelocity { get; set; }

        [IgnoreDataMember]
        public bool IsAlive { get; set; }

        [IgnoreDataMember]
        public bool IsStanding { get; set; } 

        // Physics Properties       

        [DataMember]
        public float GravityFactor { get; set; }

        [DataMember]
        public Vector2 Velocity { get; set; }

        [DataMember]
        public Vector2 MaxVelocity { get; set; }

        [IgnoreDataMember]
        public Vector2 Acceleration { get; set; }

        [IgnoreDataMember]
        public Vector2 ExternalAcceleration { get; set; }

        [DataMember]
        public Vector2 Friction { get; set; }

        [IgnoreDataMember]
        public Vector2 ExternalFriction { get; set; }

        // Collision Properties

        [IgnoreDataMember]
        public GameObjectGroup Group { get; set; }

        [IgnoreDataMember]
        public GameObjectGroup CheckAgainstGroup { get; set; }

        [IgnoreDataMember]
        public GameObjectCollisionStyle CollisionStyle { get; set; }

        [IgnoreDataMember]
        public bool IgnoreCollisions { get; set; }

        [DataMember]
        public HitBox BoundingBox { get; set; }

        [IgnoreDataMember]
        public Vector2 Offset { get; set; }

        public float? Lifetime { get; set; }
        public float? Fadetime { get; set; }

        public Timer IdleTimer { get; set; }

        protected virtual void ConfigureInstance()
        {
            // use this method in your objects to set default values before the
            // object is used
        }

        public virtual void Check( GameObject other) { }

        public virtual void CollideWith( GameObject other, string axis ) { }

        public virtual void Ready() { }

        public virtual void Erase() { }

        public override string ToString()
        {
            return string.Format("{0} ( {1} )", this.GetType().Name, this.GetType().Namespace);
        }

        public virtual void Update( GameTime gameTime )
        {
            if (!_hasBeenConfigured)
            {
                ConfigureInstance();
                _hasBeenConfigured = true;
            }

            if (Animations != null && Animations.CurrentAnimation == null)
            {
                if (Animations.HasAnimation(DefaultAnimationName))
                {
                    Animations.SetCurrentAnimation(DefaultAnimationName);
                }
                else
                {
                    Animations.SetCurrentAnimation(Animations.GetAnimations().First().Name);
                }
                
            }

            if (Lifetime.HasValue)
            {
                if (IdleTimer == null)
                {
                    IdleTimer = new Timer(Lifetime.Value);
                }
                if (IdleTimer.Delta() > Lifetime.Value)
                {
                    Kill(); // TODO: make sure calling Kill() multiple times has no side effects
                    return;
                }
            }

            LastPosition = Position;

            var tick = gameTime.GetGameTick();
            var gravityVector = new Vector2(0, FragEngineGame.Gravity * tick * GravityFactor);
            Velocity += gravityVector;
            Velocity = Velocity.SetX(
                GetNewVelocity(Velocity.X, Acceleration.X + ExternalAcceleration.X, Friction.X + ExternalFriction.X, MaxVelocity.X, tick)
                );
            Velocity = Velocity.SetY(
                GetNewVelocity(Velocity.Y, Acceleration.Y + ExternalAcceleration.Y, Friction.Y + ExternalFriction.Y, MaxVelocity.Y, tick)
                );

            var partialVelocity = Velocity * gameTime.GetGameTick();

            if (!IgnoreCollisions)
            {
                // ask the collision system if we're going to have a collision at that co-ord
                var result = CollisionService.Check(Position, partialVelocity, BoundingBox);

                HandleMovementTrace(result);
            }
            else
            {
                Position += partialVelocity;
            }

            Animations?.CurrentAnimation?.Update(gameTime);

            ExternalAcceleration = Vector2.Zero;
        }

        public virtual void HandleMovementTrace(CollisionCheckResult result)
        {
            IsStanding = false;

            if (result.YAxis)
            {
                if (Bounciness > 0 && Math.Abs(Velocity.Y) > MinBounceVelocity)
                {
                    Velocity *= new Vector2(1, -Bounciness);
                }
                else
                {
                    if (Velocity.Y > 0)
                    {
                        IsStanding = true;
                    }
                    Velocity *= new Vector2(1, 0);
                }
            }
            if (result.XAxis)
            {
                if (Bounciness > 0 && Math.Abs(Velocity.X) > MinBounceVelocity)
                {
                    Velocity *= new Vector2(-Bounciness, 1);
                }
                else
                {
                    Velocity *= new Vector2(0, 1);
                }
            }
            Position = result.Position;
        }

        public virtual void Draw( SpriteBatch batch )
        {
            string drawBox = null;
            if (Settings.TryGetValue("_feDrawBox", out drawBox) && drawBox == "true" && FragEngineGame.IsDebug)
            {
                if (_debugOutline == null)
                {
                    _debugOutline = new Texture2D(batch.GraphicsDevice, 1, 1);
                    _debugOutline.SetData(new Color[] { Color.White });
                }

                Rectangle rect = BoundingBox;

                rect.X = (int)Position.X;
                rect.Y = (int)Position.Y;

                batch.Draw(_debugOutline, new Rectangle(rect.Left, rect.Top, rect.Width, 1), Color.White);
                batch.Draw(_debugOutline, new Rectangle(rect.Left, rect.Bottom, rect.Width, 1), Color.White);
                batch.Draw(_debugOutline, new Rectangle(rect.Left, rect.Top, 1, rect.Height), Color.White);
                batch.Draw(_debugOutline, new Rectangle(rect.Right, rect.Top, 1, rect.Height + 1), Color.White);
            }

            if (Animations?.CurrentAnimation != null)
            {
                Animations.CurrentAnimation.FlipX = FlipAnimation;

                var correctedPosition = Position - Offset;

                Animations.CurrentAnimation.Draw(batch, correctedPosition, Alpha);
            }
        }

        public virtual void Kill()
        {
            IsAlive = false;
            Group = GameObjectGroup.None;
            CheckAgainstGroup = GameObjectGroup.None;
            CollisionStyle = GameObjectCollisionStyle.Never;
        }

        public virtual void ReceiveDamage(GameObject from, int amount)
        {
            if (Health < amount)
            {
                Health = 0;
                Kill();
            }
            else
            {
                Health -= amount;
            }
        }

        public bool Touches(GameObject other)
        {
            return !(
                Position.X >= other.Position.X + other.BoundingBox.Width ||
                Position.X + BoundingBox.Width <= Position.X ||
                Position.Y >= other.Position.Y + other.BoundingBox.Height ||
                Position.Y + BoundingBox.Height <= other.Position.Y
            );
        }

        public float DistanceTo(GameObject gameObject)
        {
            var xd = (Position.X + BoundingBox.Width / 2) - (gameObject.Position.X + gameObject.BoundingBox.Width / 2);
            var yd = (Position.Y + BoundingBox.Height / 2) - (gameObject.Position.Y + gameObject.BoundingBox.Height / 2);
            return (float)Math.Sqrt((xd * xd) + (yd * yd));
        }

        public float AngleTo(GameObject other)
        {
            return (float)Math.Atan2(
                (other.Position.Y + other.BoundingBox.Height / 2) - (Position.Y + BoundingBox.Height / 2),
                (other.Position.X + other.BoundingBox.Width / 2) - (Position.X + BoundingBox.Width / 2)
            );
        }

        private float GetNewVelocity(float vel, float accel, float friction, float max, float tick)
        {
            if (accel != 0)
            {
                return Utility.Limit(vel + accel*tick, -max, max);
            }
            else if (friction != 0)
            {
                var delta = friction*tick;
                if (vel - delta > 0)
                {
                    return vel - delta;
                }
                else if (vel + delta < 0)
                {
                    return vel + delta;
                }
                else
                {
                    return 0;
                }
            }
            return Utility.Limit(vel, -max, max);
        }
    }
}
