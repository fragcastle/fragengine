using Microsoft.Xna.Framework;

namespace FragEngine.Entities.Weapons
{
    public interface IWeapon
    {
        string Name { get; }

        /// <summary>
        /// Milliseconds between shots
        /// </summary>
        int FireRate { get; }

        WeaponBase GetNewProjectile(Vector2 currentLocation, float rotation, CollisionType collisionType, Vector2 baseVelocity);
    }
}
