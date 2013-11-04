using Microsoft.Xna.Framework;

namespace FragEngine.Collisions
{
    public class CollisionService : ICollisionService
    {
        private CollisionMap _map;

        private CollisionDetector _detector;

        public CollisionService( CollisionMap map = null )
        {
            _map = map ?? CollisionMap.Empty;

            _detector = new CollisionDetector( _map );
        }

        public CollisionCheckResult Check( Vector2 currentPosition, Vector2 positionDelta, Vector2 objectSize )
        {
            return _detector.Check( currentPosition, positionDelta, objectSize );
        }
    }
}
