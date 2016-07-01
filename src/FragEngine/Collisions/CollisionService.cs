using Microsoft.Xna.Framework;

namespace FragEngine.Collisions
{
    public class CollisionService : ICollisionService
    {
        private CollisionDetector _detector;

        public CollisionService( CollisionMap map = null )
        {
            _detector = new CollisionDetector( map ?? CollisionMap.Empty );
        }

        public SetCollisionMap( CollisionMap map ) 
        {
            _detector = new CollisionDetector( map );
        }

        public CollisionCheckResult Check( Vector2 currentPosition, Vector2 positionDelta, Vector2 objectSize )
        {
            return _detector.Check( currentPosition, positionDelta, objectSize );
        }
    }
}
