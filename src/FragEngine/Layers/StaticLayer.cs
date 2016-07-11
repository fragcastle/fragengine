using FragEngine.Services;
using FragEngine.View;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace FragEngine.Layers
{
    public class StaticLayer : Layer
    {

        public StaticLayer(Vector2? parallax = null)
            : base(parallax)
        {
            if (parallax == null)
                Parallax = new Vector2();
        }

        public Vector2 StaticPosition { get; set; }
        public bool Centered { get; set; }

        public override void BeginDraw(SpriteBatch spriteBatch)
        {
            var _camera = ServiceLocator.Get<Camera>();

            Matrix matrix = Centered ?
                                _camera.GetStaticViewMatrix(Parallax) :
                                _camera.GetStaticViewMatrixFromOrigin(Parallax, StaticPosition);

            spriteBatch.Begin(SpriteSortMode.Deferred, null, null, null, null, null, matrix);
        }

        public override void CustomDraw(SpriteBatch spriteBatch)
        {
            
        }
    }
}
