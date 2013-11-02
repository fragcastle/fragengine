using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FragEngine.Services;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace FragEngine
{
    public class Primitives
    {
        static Primitives()
        {
            var device = ServiceLocator.Get<GraphicsDevice>();

            WhiteTexture = new Texture2D( device, 1, 1 );
            WhiteTexture.SetData( new Color[] { Color.White } );
        }

        public static Texture2D WhiteTexture { get; private set; }
    }
}
