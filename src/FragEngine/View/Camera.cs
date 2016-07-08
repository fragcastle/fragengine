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
    public enum CameraShakeLevel
    {
        None,
        AlmostNone,
        ALittle,
        Some,
        More,
        ALot,
        TooMuch,
        Vlambeer
    }

    public class Camera
    {
        private readonly Viewport _viewport;
        private float _zoom;

        /// <summary>
        /// The camera will center the ViewPort on this Sprite
        /// </summary>
        public GameObject Target { get; set; }

        public Vector2 Offset { get; set; }

        /// <summary>
        /// When there is no Target, the camera will center on this position
        /// </summary>
        public Vector2 DefaultPosition { get; set; }

        public Camera( Viewport viewport )
        {
            _viewport = viewport;
            DefaultPosition = Vector2.Zero;
            Origin = new Vector2(viewport.Width / 2, viewport.Height / 2);
            Zoom = 1.0f;
            Offset = Vector2.Zero;
        }

        public Vector2 Position
        {
            get
            {
                return Target != null ? Target.Position : DefaultPosition;
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

        public float Rotation { get; set; }

        public CameraShakeLevel ShakeLevel { get; set; }

        public float ShakeAmount { get; set; }

        private float GetCameraShakeModifier()
        {
            switch(ShakeLevel)
            {
                case CameraShakeLevel.None:
                    return 0;
                case CameraShakeLevel.AlmostNone:
                    return 0.01f;
                case CameraShakeLevel.ALittle:
                    return 0.09f;
                case CameraShakeLevel.Some:
                    return 0.2f;
                case CameraShakeLevel.More:
                    return 0.4f;
                case CameraShakeLevel.ALot:
                    return 0.6f;
                case CameraShakeLevel.TooMuch:
                    return 0.8f;
                case CameraShakeLevel.Vlambeer:
                    return 1.5f;
            }
            return 0.01f;
        }

        public Matrix GetViewMatrix( Vector2 parallax )
        {
            var shake = ShakeAmount/100*GetCameraShakeModifier();
            var shakeVector = new Vector2(Utility.RndRange(-shake, shake), Utility.RndRange(-shake, shake));
            var translationVector = new Vector3(-Position*parallax + shakeVector, 0.0f);
            if (Utility.CoinFlip())
            {
                translationVector = new Vector3(-Position * parallax - shakeVector, 0.0f);
            }
            return Matrix.CreateTranslation( translationVector ) *
                   Matrix.CreateRotationZ( Rotation ) *
                   Matrix.CreateScale( new Vector3(Zoom, Zoom, 1f ) ) *
                   Matrix.CreateTranslation( new Vector3( Origin, 0.0f ) );
        }

        public Matrix GetStaticViewMatrix( Vector2 parallax )
        {
            return Matrix.CreateTranslation( new Vector3( Position * parallax, 0.0f ) ) *
                   Matrix.CreateRotationZ( Rotation ) *
                   Matrix.CreateScale( new Vector3( Zoom, Zoom, 1f ) ) *
                   Matrix.CreateTranslation( new Vector3( Origin, 0.0f ) ) *
                   Matrix.CreateTranslation( new Vector3( -Position, 0.0f ) );
        }

        public Matrix GetStaticViewMatrixFromOrigin( Vector2 parallax, Vector2 origin )
        {
            return Matrix.CreateTranslation( new Vector3( Position * parallax, 0.0f ) ) *
                   Matrix.CreateRotationZ( Rotation ) *
                   Matrix.CreateScale( 1f ) *
                   Matrix.CreateTranslation( new Vector3( origin, 0.0f ) ) *
                   Matrix.CreateTranslation( new Vector3( -Position, 0.0f ) );
        }

        public void ClientSizeChanged( object sender, System.EventArgs e )
        {

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
