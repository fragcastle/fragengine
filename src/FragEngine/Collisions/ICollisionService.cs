using Microsoft.Xna.Framework;

namespace FragEngine.Collisions
{
    public interface ICollisionService
    {
        CollisionCheckResult Check( Vector2 currentPosition, Vector2 positionDelta, Vector2 objectSize );
    }
}
