using FragEngine.Data;

namespace FragEngine.Collisions
{
    public class CollisionMap
    {
        private bool _isEmpty;

        public static CollisionMap Empty
        {
            get
            {
                return new CollisionMap();
            }
        }

        public Map MapData;

        public int TileSize;

        public bool IsEmpty 
        { 
            get 
            { 
                return _isEmpty;
            }
        }

        public CollisionMap( Level level )
        {
            MapData = level.CollisionLayer.MapData;
            TileSize = level.CollisionLayer.TileSize;
        }

        // private initializer for an "empty" collision map
        private CollisionMap()
        {
            _isEmpty = true;
            MapData = new Map() { Data = new int[800], Width = 80 };
            TileSize = 16;

            for( int i = 0; i < MapData.Data.Length; i++ )
            {
                MapData.Data[ i ] = -1;
            }
        }
    }
}
