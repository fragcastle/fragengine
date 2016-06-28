using FragEngine.Services;
using FragEngine.View;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using FragEngine.Entities;

namespace FragEngine.Layers
{
    public class StaticLayer : Layer
    {

        public StaticLayer(Vector2D? parallax = null)
            : base(parallax)
        {
            if (parallax == null)
                Parallax = new Vector2D();
        }

        public Vector2 StaticPosition { get; set; }
        public bool Centered { get; set; }

        public override void Draw( SpriteBatch spriteBatch )
        {
            var _camera = ServiceLocator.Get<Camera>();

            Matrix matrix = Centered ?
                                _camera.GetStaticViewMatrix( Parallax ) :
                                _camera.GetStaticViewMatrixFromOrigin( Parallax, StaticPosition );

            spriteBatch.Begin( SpriteSortMode.Deferred, null, null, null, null, null, matrix );

            if( DrawMethod != null )
                DrawMethod( spriteBatch );

            spriteBatch.End();
        }
    }
}
