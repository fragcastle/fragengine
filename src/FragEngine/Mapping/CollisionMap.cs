using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FragEngine.Data;
using FragEngine.Entities;
using Microsoft.Xna.Framework;

namespace FragEngine.Mapping
{
    public class CollisionMap
    {

        public CompressedMapData MapData;

        public int TileSize;

        public CollisionMap( Level level )
        {
            MapData = level.CollisionLayer().MapData;
            TileSize = level.CollisionLayer().TileSize;
        }

        public CollisionCheckResult Peek( EntityBase entity, Vector2 proposedPosition )
        {
            var collision = new CollisionCheckResult();

            using( var detector = new CollisionDetector(entity, proposedPosition, this ) )
            {
                collision = detector.Result;
            }

            return collision;
        }
    }
}
