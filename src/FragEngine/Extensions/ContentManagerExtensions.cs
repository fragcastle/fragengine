using System;
using System.Reflection;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended.Content;

namespace FragEngine.Extensions
{
    public static class ContentManagerExtensions
    {
        public static Texture2D LoadTexture2DFromResource(this ContentManager manager, string path, Assembly caller = null)
        {
            caller = caller ?? Assembly.GetCallingAssembly();

            var stream = caller.GetManifestResourceStream(path);

            if (stream == null)
            {
                throw new Exception("Texture could not be read from resource: " + path);
            }

            return Texture2D.FromStream(manager.GetGraphicsDevice(), stream);
        }

        public static Font LoadFont(this ContentManager manager, string path)
        {
            var txtr = manager.Load<Texture2D>(path);
            if (txtr == null)
            {
                txtr = manager.LoadTexture2DFromResource(path, Assembly.GetCallingAssembly());
            }

            return new Font(txtr);
        }
    }
}
