using System.Collections.Generic;
using FragEngine.Animation;
using FragEngine.Collisions;
using FragEngine.GameObjects;
using FragEngine.Services;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace FragEngine.Particles
{
    public enum ParticleSizes
    {
        Tiny = 1,
        Small = 2,
        Medium = 4,
        Large = 6
    }

    public class Particle : GameObject
    {
        private static readonly Dictionary<Color, Texture2D[]> _cache = new Dictionary<Color, Texture2D[]>();
        public Color[] Colors { get; set; }

        public Vector2? AbsoluteVelocity { get; set; }
        public Vector2? SetVelocity { get; set; }

        public bool Splashes { get; set; }

        public Vector2<bool> Drips { get; set; }

        public Particle() : this(Vector2.Zero) { }
        public Particle(Vector2 initialLocation, Vector2? initialVelocity = null) : base(initialLocation, initialVelocity)
        {
            Friction = new Vector2(10f, 0);
            Bounciness = 0.6f;
            MaxVelocity = new Vector2(320, 320);
            BoundingBox = new HitBox(4, 4);
            GravityFactor = 8f;
            CollisionStyle = GameObjectCollisionStyle.Never;
            CheckAgainstGroup = GameObjectGroup.None;
            Group = GameObjectGroup.None;

            Lifetime = 1f;
            Fadetime = 1f;

            Colors = new[] { Color.White };
        }

        private Texture2D CreateTexture(Color color, int size)
        {
            var graphicsDevice = ServiceLocator.Get<GraphicsDevice>();
            var data = new Color[size * size];
            var texture = new Texture2D(graphicsDevice, size, size);
            for (int i = 0; i < data.Length; ++i)
                data[i] = color;
            texture.SetData(data);

            return texture;
        }

        protected override void ConfigureInstance()
        {
            Texture2D rectTexture = null;
            var color = Utility.Random(Colors);

            // get our texture from the cache
            // or create one if one does not exist.
            // this isn't thread safe, but I don't
            // think it needs to be since particles
            // are spawned in loops within a single
            // update()
            if (!_cache.ContainsKey(color))
            {
                _cache[color] = new Texture2D[] { null, null, null, null, null };
            }

            if (_cache.ContainsKey(color))
            {
                var idx = (int) BoundingBox.Width - 1;
                if (_cache[color][idx] != null)
                {
                    rectTexture = _cache[color][idx];
                }
                else
                {
                    rectTexture = _cache[color][idx] = CreateTexture(color, (int)BoundingBox.Width);
                }
            }

            Animations = new AnimationSheet(rectTexture, (int)BoundingBox.Width, (int)BoundingBox.Height);
            Animations.Add("idle", 1f, true, 0);

            // set the particle velocity
            Velocity = Utility.RndPos(Velocity.SetX(-Velocity.X).SetY(-Velocity.Y), Velocity);

            if (AbsoluteVelocity.HasValue)
            {
                Velocity += AbsoluteVelocity.Value;
            }

            if (SetVelocity.HasValue)
            {
                Velocity =
                    Velocity.SetX(Velocity.X*(Utility.CoinFlip() ? 1 : -1))
                        .SetY(Velocity.Y*(Utility.CoinFlip() ? 1 : -1));
            }

            if (Splashes)
            {
                Drips = new Vector2<bool>(Utility.CoinFlip(), Utility.CoinFlip());
            }
        }

        public override void HandleMovementTrace(CollisionCheckResult result)
        {
            if (Splashes)
            {
                var flip = new Vector2<bool>(Velocity.X < 0, Velocity.Y < 0);
                if (result.YAxis && Drips.Y)
                {
                    /*
                    ig.game.spawnEntity(EffectSplatter, res.pos.x, res.pos.y + this.size.y, {
            flipX: flip.x,
            flipY: flip.y,
            color: this.color,
            canDrip: true,
            impactVel: { x: this.vel.x, y: this.vel.y }
          });
                    */
                    Drips.Y = false;
                }
                if (!result.YAxis && result.XAxis && Drips.X)
                {
                    /*
                    ig.game.spawnEntity(EffectSplatter, res.pos.x + this.size.x, res.pos.y, {
            wall: true,
            flipX: flip.x,
            flipY: flip.y,
            canDrip: false,
            color: this.color,
            impactPos: { x: this.pos.x, y: this.pos.y },
            impactVel: { x: this.vel.x, y: this.vel.y }
          });
                    */
                    Drips.X = false;
                }
            }
            base.HandleMovementTrace(result);
        }

        public override void ReceiveDamage(GameObject @from, int amount)
        {
            // invincible
        }

        public override void Draw(SpriteBatch batch)
        {
            // TODO: potentially use a 3D ortho hack here if it gives better performance
            // see: http://stackoverflow.com/questions/23305577/draw-rectangle-in-monogame
            base.Draw(batch);
        }
    }
}
