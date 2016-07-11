using FragEngine.Services;
using Microsoft.Xna.Framework.Graphics;

namespace FragEngine.Content
{
    public class TextureImporter : FileImporter<Texture2D>
    {
        public override bool HandlesPath(string path)
        {
            return base.HandlesPath(path) && path.Contains("Textures\\") && path.EndsWith(".png");
        }

        public override Texture2D Import(string path)
        {
            var graphicsDevice = ServiceLocator.Get<GraphicsDevice>();
            using (var stream = OpenStream(path))
            {
                return Texture2D.FromStream(graphicsDevice, stream);
            }
        }
    }
}
