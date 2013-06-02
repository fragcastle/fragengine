using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FragEngine;
using FragEngine.Entities;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace FragEngine.View
{
    public class Camera
    {
        private readonly Viewport _viewport;
        private float _zoom;

        /// <summary>
        /// The camera will center the ViewPort on this Sprite
        /// </summary>
        public EntityBase Target { get; set; }

        /// <summary>
        /// When there is no Target, the camera will center on this position
        /// </summary>
        public Vector2 DefaultPosition { get; set; }

        public Camera( Viewport viewport )
        {
            _viewport = viewport;
            DefaultPosition = Vector2.Zero;
            Origin = Vector2.Zero;
            Zoom = 1.0f;
        }

        public Vector2 Position
        {
            get
            {
                if( Target != null )
                {
                    return Target.Position;
                }

                return DefaultPosition;
            }
        }

        /// <summary>
        /// Center of the Screen
        /// </summary>
        public Vector2 Origin { get; set; }

        public float Zoom
        {
            get { return _zoom; }
            set { _zoom = value; if( _zoom < 0.1f ) _zoom = 0.1f; } // Negative zoom will flip image
        }

        public Vector2 Center
        {
            get
            {
                var centerX = _viewport.Width/2;
                var centerY = _viewport.Height/2;

                return ScreenToGame(new Vector2(centerX, centerY));
            }
        }

        public float Rotation { get; set; }

        public Matrix GetViewMatrix( Vector2 parallax )
        {
            return Matrix.CreateTranslation( new Vector3( -Position * parallax, 0.0f) ) *
                ( FragEngineGame.SpriteScale * Zoom ) *
                   Matrix.CreateTranslation( new Vector3( Origin, 0.0f ) ) *
                   Matrix.CreateRotationZ( Rotation );
        }

        public Matrix GetStaticViewMatrix( Vector2 parallax )
        {
            return ( FragEngineGame.SpriteScale * Zoom ) *
                   Matrix.CreateTranslation( new Vector3( Position * parallax, 0.0f ) ) *
                   Matrix.CreateRotationZ( Rotation ) *
                   Matrix.CreateTranslation( new Vector3( Origin, 0.0f ) ) *
                   Matrix.CreateTranslation( new Vector3( -Position, 0.0f ) );
        }

        public Matrix GetStaticViewMatrixFromOrigin( Vector2 parallax, Vector2 origin )
        {
            return Matrix.CreateTranslation( new Vector3( Position * parallax, 0.0f ) ) *
                   Matrix.CreateScale( 1f ) *
                   Matrix.CreateRotationZ( Rotation ) *
                   Matrix.CreateTranslation( new Vector3( origin, 0.0f ) ) *
                   Matrix.CreateTranslation( new Vector3( -Position, 0.0f ) );
        }

        public void ClientSizeChanged( object sender, System.EventArgs e )
        {
            var gameWindow = sender as GameWindow;

            if( gameWindow != null )
            {
                Origin = new Vector2( gameWindow.ClientBounds.Width / 2.0f, gameWindow.ClientBounds.Height / 2.0f );
            }
        }

        public bool IsShowing( Rectangle box )
        {
            var x1 = (int)( Origin.X - _viewport.Width / 2 );
            var x2 = (int)( Origin.X + _viewport.Width / 2 );
            var y1 = (int)( Origin.Y - _viewport.Height / 2 );
            var y2 = (int)( Origin.Y + _viewport.Height / 2 );

            var completelyOffScreen = ( x1 > box.X + box.Width || x2 < box.X ) && (y1 > box.Y + box.Height || y2 < box.Y);
            return !completelyOffScreen;
        }

        /// <summary>
        /// Translate Screen coordinates to game-world coordinates
        /// </summary>
        /// <param name="screenPosition"></param>
        /// <returns></returns>
        public Vector2 ScreenToGame(Vector2 screenPosition)
        {
            var transform = Matrix.Invert(GetViewMatrix(new Vector2(1, 1)));
            Vector2.Transform(ref screenPosition, ref transform, out screenPosition);
            return screenPosition;
        }

        /// <summary>
        /// Translate game-world coordinates to Screen coordinates
        /// </summary>
        /// <param name="screenPosition"></param>
        /// <returns></returns>
        public Vector2 GameToScreen(Vector2 gamePosition)
        {
            var transform = GetViewMatrix(new Vector2(1, 1));
            Vector2.Transform(ref gamePosition, ref transform, out gamePosition);
            return gamePosition;
        }
    }
}
