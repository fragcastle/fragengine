using System;
using FragEngine.Layers;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace FragEngine.Maps
{
    public class MapLayer : Layer
    {

        public MapLayer(MapDataDictionary mapData, MapConfiguration config)
        {
            MapData = mapData;
            Config = config;
            SamplerState = SamplerState.PointClamp;
        }

        public MapDataDictionary MapData { get; set; }

        public MapConfiguration Config { get; set; }

        public int? GetTile(int x, int y)
        {
            var tx = (int)Math.Floor((double)x / Config.TileSize);
            var ty = (int)Math.Floor((double)y / Config.TileSize);

            return MapData.GetTile(new Vector2(tx, ty));
        }

        public void SetTile(Vector2 gameWorldPosition, int tileValue)
        {
            // truncate both the values, keeping as floats will produce unexpected tile placements
            var correctedPosition = new Vector2(
                (int)(gameWorldPosition.X / Config.TileSize),
                (int)(gameWorldPosition.Y / Config.TileSize));

            var index = MapData.GetCellIndex(correctedPosition);

            MapData[index] = tileValue;
        }

        public void SetTile(int x, int y, int tile)
        {
            var tx = (int)Math.Floor((double)x / Config.TileSize);
            var ty = (int)Math.Floor((double)y / Config.TileSize);

            var position = new Vector2(tx, ty);

            MapData[position] = tile;
        }

        public override void CustomDraw(SpriteBatch spriteBatch)
        {
            spriteBatch.DrawMapLayer(this);
        }
    }
}
