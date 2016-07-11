using System.Collections.Generic;
using System.Linq;
using FragEngine.Services;
using FragEngine.View;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace FragEngine.GameObjects
{
    public abstract class Hud : GameObject
    {
        public GameObject Target { get; set; }
        public bool ShowPauseMessage { get; set; }
        public SpriteFont Font { get; set; }

        private readonly Dictionary<Vector2, string> _text;

        protected Rectangle _windowSize;

        public Hud( Rectangle windowSize ) : base( Vector2.Zero )
        {
            _windowSize = windowSize;
            _text = new Dictionary<Vector2, string>();
        }

        public override void Draw( SpriteBatch spriteBatch )
        {
            if( ShowPauseMessage )
            {
                var center = new Vector2( _windowSize.Width / 2, _windowSize.Height / 2 );

                Vector2 FontOrigin = Font.MeasureString( "Game Paused" ) / 2;
                spriteBatch.DrawString( Font, "Game Paused", center, Color.White, 0, FontOrigin, 1.0f, SpriteEffects.None, 0.5f );
            }

            while ( _text.Any() )
            {
                var entry = _text.First();

                spriteBatch.DrawString( Font, entry.Value, entry.Key, Color.White );

                _text.Remove(entry.Key);
            }
        }

        public override void Update( GameTime gameTime )
        {

        }

        public void ClientSizeChanged( object sender, System.EventArgs e )
        {
            var gameWindow = sender as GameWindow;

            if( gameWindow != null )
            {
                _windowSize = gameWindow.ClientBounds;
            }
        }

        public Matrix GetTransformation()
        {
            var camera = ServiceLocator.Get<Camera>();
            var pos = Target == null ? camera.DefaultPosition : Target.Position;

            return Matrix.CreateTranslation( new Vector3( -pos.X, -pos.Y, 0 ) ) *
                    Matrix.CreateTranslation( new Vector3( _windowSize.Width * 0.5f, _windowSize.Height * 0.5f, 0 ) );
        }

        public void DrawText(string text, Vector2 location, SpriteBatch batch)
        {
            batch.DrawString( Font, text, location, Color.White );
        }

        public void DrawText(string text, Vector2 location)
        {
            if (!_text.ContainsKey(location))
            {
                _text.Add(location, text);
            }
            else
            {
                _text[location] = text;
            }
        }
    }
}
