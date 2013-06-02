using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;

namespace FragEngine.Entities.Weapons
{
    public abstract class WeaponBase : EntityBase
    {
        public virtual double MaxLifeSpan { get; private set; }
        public virtual int Damage { get; private set; }
        public virtual bool IsCritical { get; private set; }

        // sound effect overrides
        /// <summary>
        /// The sound that will play when the weapon is first spawned.
        /// </summary>
        public virtual SoundEffect FireSound { get; protected set; }

        /// <summary>
        /// The sound that will play when the weapon is traveling across the game space
        /// </summary>
        public virtual SoundEffect TravelingSound { get; protected set; }

        /// <summary>
        /// The sound that will play when the weapon collides with an object
        /// </summary>
        public virtual SoundEffect CollisionSound { get; protected set; }

        public double LifeSpan { get; private set; }

        public WeaponBase( Vector2 initialLocation, Vector2 initialVelocity )
            : base( initialLocation, initialVelocity )
        {
            var critCheck = Utility.NextFloat();
            IsCritical = critCheck > 0f && critCheck < 0.2f;
        }

        public override void Update(GameTime gameTime)
        {
            LifeSpan += gameTime.ElapsedGameTime.TotalMilliseconds;

            if (LifeSpan > MaxLifeSpan)
            {
                IsAlive = false;
                return;
            }

            base.Update(gameTime);
        }

        public override void Kill()
        {
            base.Kill();

            // remove the laser from the WeaponManager
            WeaponManager.RemoveProjectile( this );
        }
    }
}
