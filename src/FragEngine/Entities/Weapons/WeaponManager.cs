using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace FragEngine.Entities.Weapons
{
    public class WeaponManager
    {
        public static List<WeaponBase> Projectiles { get; set; }

        public static List<IWeapon> PrimaryWeapons { get; set; }
        public static List<IWeapon> SecondaryWeapons { get; set; }

        static WeaponManager()
        {
            Projectiles = new List<WeaponBase>();
            PrimaryWeapons = new List<IWeapon>();
            SecondaryWeapons = new List<IWeapon>();
        }

        public static void AddProjectile( WeaponBase projectile )
        {
            Projectiles.Add( projectile );

            if( projectile.FireSound != null )
            {
                projectile.FireSound.Play();
            }
        }

        public static void RemoveProjectile( WeaponBase projectile )
        {
            Projectiles.Remove( projectile );
        }

        public static void Update( GameTime gameTime )
        {
            foreach( var projectile in Projectiles )
            {
                projectile.Update( gameTime );
            }

            Projectiles.Where( p => !p.IsAlive ).ToList().ForEach( p => p.Kill() );
        }

        public static void Draw( SpriteBatch spriteBatch )
        {
            foreach( var projectile in Projectiles )
            {
                projectile.Draw( spriteBatch );

                if( projectile.TravelingSound != null )
                {
                    projectile.TravelingSound.Play();
                }
            }
        }

        public static WeaponBase CheckIfHit( EntityBase target )
        {
            if( target.Collision == CollisionType.NONE )
                return null;

            var projectile = Projectiles
                                .Where( w => w.Collision != CollisionType.NONE )
                                .Where( w => target.Collision == CollisionType.A ? w.Collision == CollisionType.B : w.Collision == CollisionType.A )
                                .FirstOrDefault( w => w.CollidesWith( target ) );

            if( projectile != null && projectile.CollisionSound != null )
                projectile.CollisionSound.Play();

            return projectile;
        }
    }

}
