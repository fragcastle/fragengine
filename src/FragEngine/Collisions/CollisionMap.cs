using FragEngine.Data;

namespace FragEngine.Collisions
{
    public class CollisionMap
    {
        public static CollisionMap Empty
        {
            get
            {
                return new CollisionMap();
            }
        }

        public Map MapData;

        public int TileSize;

        public CollisionMap( Level level )
        {
            MapData = level.CollisionLayer.MapData;
            TileSize = level.CollisionLayer.TileSize;
        }

        // private initializer for an "empty" collision map
        private CollisionMap()
        {
            MapData = new Map() { Data = new int[800], Width = 80 };
            TileSize = 16;

            for( int i = 0; i < MapData.Data.Length; i++ )
            {
                MapData.Data[ i ] = -1;
            }
        }
    }
}
