using Microsoft.Xna.Framework;

namespace FragEngine.Collisions
{
    public interface ICollisionService
    {
        void SetCollisionMap(CollisionMap map);
        CollisionCheckResult Check( Vector2 currentPosition, Vector2 positionDelta, Vector2 objectSize );
    }
}
