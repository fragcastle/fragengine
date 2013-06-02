using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using FragEngine.View;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using FragEngine.Entities;

namespace FragEngine.Layers
{

    [DataContract]
    public class Layer
    {
        protected Camera _camera;

        protected bool _initialized;

        [DataMember]
        public virtual string Name { get; set; }

        [DataMember]
        public Vector2 Parallax { get; set; }

        [IgnoreDataMember]
        public Action<SpriteBatch> DrawMethod;

        [IgnoreDataMember]
        public SamplerState SamplerState { get; set; }

        public float Alpha { get; set; }

        public float Opacity
        {
            get
            {
                return Alpha / 255f;
            }
        }

        public Layer( Camera camera, Vector2? parallax = null )
        {
            _camera = camera;
            Parallax = parallax ?? new Vector2( 1, 1 );

            SamplerState = SamplerState.LinearClamp; // XNA Default

            Alpha = 255f; // layer fully opaque by default
        }

        public virtual void Initialize()
        {

        }

        public virtual void Draw( SpriteBatch spriteBatch )
        {
            if( !_initialized )
            {
                Initialize();
                _initialized = true;
            }

            if( _camera != null )
            {
                spriteBatch.Begin( SpriteSortMode.Deferred, BlendState.AlphaBlend, SamplerState, null, null, null, _camera.GetViewMatrix( Parallax ) );
            }
            else
            {
                // the editor will not set the camera (static layering)
                // TODO: change this to rely on a RenderMode enum?
                spriteBatch.Begin( SpriteSortMode.Deferred, null, SamplerState, null, null );
            }

            if( DrawMethod != null )
            {
                DrawMethod( spriteBatch );
            }

            spriteBatch.End();
        }

        public override string ToString()
        {
            return "Layer: " + Name;
        }
    }
}
