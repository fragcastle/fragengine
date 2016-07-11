using FragEngine.Maps;

namespace FragEngine.Collisions
{
    public class CollisionMap
    {
        private readonly bool _isEmpty;

        public static CollisionMap Empty
        {
            get
            {
                return new CollisionMap();
            }
        }

        public MapDataDictionary MapData { get; set; }

        public int TileSize { get; set; }

        public bool IsEmpty 
        { 
            get 
            { 
                return _isEmpty;
            }
        }

        public CollisionMap(MapDataDictionary collisionMap, int tileSize)
        {
            MapData = collisionMap;
            TileSize = tileSize;
        }

        // private initializer for an "empty" collision map
        private CollisionMap()
        {
            _isEmpty = true;
            MapData = new MapDataDictionary() { Data = new int[800], Width = 80 };
            TileSize = 16;

            for( int i = 0; i < MapData.Data.Length; i++ )
            {
                MapData.Data[ i ] = -1;
            }
        }
    }
}
