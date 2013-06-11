using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using FragEngine;
using FragEngine.Data;
using FragEngine.Forms;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
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

        protected override void Draw(SpriteBatch spriteBatch)
        {
            _accumulatedElapsedTime += _gameTimer.Elapsed;
            _gameTimer.Reset();
            _gameTimer.Start();

            // Do not allow any update to take longer than our maximum.
            if( _accumulatedElapsedTime > _maxElapsedTime )
                _accumulatedElapsedTime = _maxElapsedTime;

            var mouse = Mouse.GetState();
            var currentMousePosition = new Vector2( mouse.X, mouse.Y );
            if( currentMousePosition != _lastMousePosition )
            {
                _lastMousePosition = new Vector2( mouse.X, mouse.Y );

                var args = MouseStateHelpers.GetMouseEventArgs();

                OnMouseMove( args );
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
                spriteBatch.Begin();
                DrawEntities( spriteBatch );
                spriteBatch.End();
            }
        }

        private void DrawEntities(SpriteBatch spriteBatch)
        {
            if( Level.Entities.Count > 0 ) {
                foreach( var entity in Level.Entities.Where( entity => entity.IsAlive ) ) {
                    // update the current animation
                    if( entity.Animations.CurrentAnimation != null ) {
                        entity.Animations.CurrentAnimation.Update( _gameTime );
                    }

                    // I honestly have no idea why this has to be called this way
                    // instead of calling entity.draw()...
                    entity.Animations.CurrentAnimation.Draw( spriteBatch, entity.Position );
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
