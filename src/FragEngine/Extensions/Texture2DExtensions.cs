using FragEngine.Services;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace FragEngine
{
    public static class Texture2DExtensions
    {
        public static Texture2D SubTexture(this Texture2D texture, Rectangle view)
        {
            var graphicsDevice = ServiceLocator.Get<GraphicsDevice>();
            var imageData = new Color[texture.Width * texture.Height];
            var copy = new Color[view.Width * view.Height];
            texture.GetData<Color>(imageData);
            for (var x = 0; x < view.Width; x++)
            {
                for (var y = 0; y < view.Height; y++)
                {
                    var copyIdx = x + (y*view.Width);
                    var imageIdx = x + view.X + ((y + view.Y)*texture.Width);
                    copy[copyIdx] = imageData[imageIdx];
                }
            }
            var subtexture = new Texture2D(graphicsDevice, view.Width, view.Height);
            return subtexture;
        }
    }
}
