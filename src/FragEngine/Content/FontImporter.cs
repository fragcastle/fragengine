using FragEngine.Services;
using Microsoft.Xna.Framework.Graphics;

namespace FragEngine.Content
{
    public class FontImporter : FileImporter<Font>
    {
        public override bool HandlesPath(string path)
        {
            return base.HandlesPath(path) && path.Contains("Fonts\\") && path.EndsWith(".png");
        }

        public override Font Import(string path)
        {
            var graphicsDevice = ServiceLocator.Get<GraphicsDevice>();
            using (var stream = OpenStream(path))
            {
                var txtr = Texture2D.FromStream(graphicsDevice, stream);
                return new Font(txtr);
            }
        }
    }
}
