using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
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
            }
        }

        private void DrawLevel(SpriteBatch spriteBatch)
        {
            if( Visible )
            {
                foreach( var map in Level.MapLayers() )
                {
                    map.Draw( spriteBatch );
                }

                if( Level.Entities.Count > 0 )
                {
                    foreach( var entity in Level.Entities )
                    {
                        // update the current animation
                        if( entity.Animations.CurrentAnimation != null )
                        {
                            entity.Animations.CurrentAnimation.Update( _gameTime );
                        }
                    }
                }

                // draw the collision layer
                // in the game we don't draw this layer
                Level.CollisionLayer().Draw( spriteBatch );
            }
        }
    }
}
