using System;
using System.Diagnostics;
using System.Linq;
using FragEd.Forms;
using FragEngine;
using FragEngine.Data;
using FragEngine.Services;
using FragEngine.View;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace FragEd.Controls
{
    public class LevelEditorControl : GraphicsDeviceControl
    {
        private Vector2 _lastMousePosition = Vector2.One;

        public Level Level { get; set; }

        private readonly GameTime _gameTime = new GameTime();
        private Stopwatch _gameTimer = Stopwatch.StartNew();
        private TimeSpan _accumulatedElapsedTime;

        private readonly TimeSpan _maxElapsedTime = TimeSpan.FromMilliseconds( 500 );

        public float LatestTick { get; private set; }

        public void SetGridState( bool drawGrid )
        {
            Level.MapLayers.ForEach( m => m.DrawGrid = drawGrid );
        }

        protected override void Draw(SpriteBatch spriteBatch)
        {
            _accumulatedElapsedTime += _gameTimer.Elapsed;
            _gameTimer.Reset();
            _gameTimer.Start();

            // Do not allow any update to take longer than our maximum.
            if( _accumulatedElapsedTime > _maxElapsedTime )
                _accumulatedElapsedTime = _maxElapsedTime;

            var mouse = Mouse.GetState();
            var mouse_args = MouseStateHelpers.GetMouseEventArgs();
            var currentMousePosition = new Vector2( mouse.X, mouse.Y );
            if( currentMousePosition != _lastMousePosition )
            {
                _lastMousePosition = new Vector2( mouse.X, mouse.Y );

                OnMouseMove( mouse_args );
            }

            if( mouse.ScrollWheelValue != 0 )
                OnMouseWheel( mouse_args );

            // check if a key is being pressed
            var keys = Keyboard.GetState().GetPressedKeys();
            if( keys.Length > 0 )
            {
                var args = KeyboardStateHelpers.GetKeyEventArgs();
                OnKeyDown(args);
            }

            if( Level != null )
            {
                _gameTime.ElapsedGameTime = _accumulatedElapsedTime;
                _gameTime.TotalGameTime += _accumulatedElapsedTime;
                _accumulatedElapsedTime = TimeSpan.Zero;

                DrawLevel( spriteBatch );

                // batch has to be started before animations/entities can be drawn
                // if you get the "big red x" error, ctrl+alt+e and check "thrown" for
                // Common Language Runtime: http://thewayofcoding.com/2011/08/xna-4-0-red-x-exceptions/
                var _camera = ServiceInjector.Get<Camera>();
                spriteBatch.Begin( SpriteSortMode.Deferred, BlendState.AlphaBlend, SamplerState.PointClamp, null, null, null, _camera.GetViewMatrix( new Vector2( 1, 1 ) ) );
                DrawEntities( spriteBatch );
                spriteBatch.End();
            }

            LatestTick = _gameTime.GetGameTick();
        }

        private void DrawEntities(SpriteBatch spriteBatch)
        {
            if( Level.Entities.Count > 0 ) {
                foreach( var entity in Level.Entities.Where( entity => entity.IsAlive ) ) {
                    // update the current animation and draw the entity
                    entity.Update( _gameTime );
                    entity.Draw( spriteBatch );
                }
            }
        }

        private void DrawLevel(SpriteBatch spriteBatch)
        {
            if( Visible )
            {
                foreach( var map in Level.MapLayers )
                {
                    map.Draw( spriteBatch );
                }

                // draw the collision layer
                // in the game we don't draw this layer
                Level.CollisionLayer.Draw( spriteBatch );
            }
        }
    }
}
