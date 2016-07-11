using FragEngine.Maps;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace FragEngine
{
    public static class SpriteBatchExtensions
    {
        public static void DrawMapLayer(this SpriteBatch spriteBatch, MapLayer layer)
        {
            if (layer.Config.TileSet != null)
            {
                layer.MapData.EachCell((cell, tile) =>
                {
                    // adjust the vector position according to the tilesize
                    cell *= layer.Config.TileSize;

                    // only draw tiles that have data
                    if (tile > -1)
                    {
                        var offsetX = layer.Config.TileSize * tile;
                        var offsetY = 0;

                        while (offsetX >= layer.Config.TileSet.Width)
                        {
                            // set the x to 0 and increase the height by one whole
                            // to push the frame onto the next row
                            offsetX = offsetX - layer.Config.TileSet.Width;
                            offsetY += layer.Config.TileSize;
                        }

                        var animationFrame = new Rectangle(offsetX, offsetY, layer.Config.TileSize, layer.Config.TileSize);
                        var effect = SpriteEffects.None;

                        spriteBatch.Draw(layer.Config.TileSet, cell, animationFrame, Color.White * layer.Config.Opacity, 0, Vector2.Zero, 1f, effect, 0.0f);
                    }
                        
                    //if (DrawGrid)
                    //{
                    //    var frame = new Rectangle((int)cell.X, (int)cell.Y, layer.Config.TileSize, layer.Config.TileSize);

                    //    var whiteTexture = new Texture2D(spriteBatch.GraphicsDevice, 1, 1);
                    //    whiteTexture.SetData(new Color[] { Color.White });

                    //    spriteBatch.Draw(whiteTexture, new Rectangle(frame.Left, frame.Top, frame.Width, 1), Color.White);
                    //    spriteBatch.Draw(whiteTexture, new Rectangle(frame.Left, frame.Bottom, frame.Width, 1), Color.White);
                    //    spriteBatch.Draw(whiteTexture, new Rectangle(frame.Left, frame.Top, 1, frame.Height), Color.White);
                    //    spriteBatch.Draw(whiteTexture, new Rectangle(frame.Right, frame.Top, 1, frame.Height + 1), Color.White);
                    //}
                });
            }
        }
    }
}
