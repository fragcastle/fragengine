using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FragEngine.Entities;
using FragEngine.Services;
using Microsoft.Xna.Framework;

namespace FragEngine.Mapping
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
