using FragEngine.Services;
using FragEngine.View;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace FragEngine.Layers
{
    public abstract class Layer
    {
        protected bool _initialized;

        public int Order { get; set; }

        public virtual string Name { get; set; }

        public Vector2 Parallax { get; set; }

        public SamplerState SamplerState { get; set; }

        public float Alpha { get; set; }

        public float Opacity
        {
            get
            {
                return Alpha / 255f;
            }
        }

        public Layer( Vector2? parallax = null )
        {
            Parallax = parallax ?? new Vector2( 1, 1 );

            SamplerState = SamplerState.LinearClamp; // XNA Default

            Alpha = 255f; // layer fully opaque by default
        }

        public virtual void Initialize()
        {
            // children can override this
        }

        public virtual void BeginDraw(SpriteBatch spriteBatch)
        {
            var camera = ServiceLocator.Get<Camera>();

            if (camera != null)
                spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, SamplerState, null, null, null, camera.GetViewMatrix(Parallax));
            else
                spriteBatch.Begin(SpriteSortMode.Deferred, null, SamplerState, null, null);
        }

        public void Draw( SpriteBatch spriteBatch )
        {
            if( !_initialized )
            {
                Initialize();
                _initialized = true;
            }

            BeginDraw(spriteBatch);
            CustomDraw(spriteBatch);
            EndDraw(spriteBatch);
        }

        public abstract void CustomDraw(SpriteBatch spriteBatch);

        public virtual void EndDraw( SpriteBatch spriteBatch )
        {
            spriteBatch.End();
        }

        public override string ToString()
        {
            return "Layer: " + Name;
        }
    }
}
