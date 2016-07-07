using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using FragEngine.Data;
using FragEngine.Entities;
using FragEngine.Services;
using FragEngine.View;
using FragEngine.View.Screens;
using JumpJoy.Entities;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace JumpJoy.Screens
{
    public class JumpPlayScreen : PlayScreen
    {
        public JumpPlayScreen( Hud hud )
            : base( hud )
        {

        }

        public override void Initialize()
        {
            base.Initialize();

            // load the level

            LoadLevel( "jumpjoy_1" );

            var jump = ServiceLocator.Get<IGameObjectService>().GetGameObjectByName("Jump");

            var camera = ServiceLocator.Get<Camera>();

            camera.Target = jump;

            // this kind of works... but seems odd...
            camera.Offset = new Vector2( 48, 256 );

            // tell XNA not to apply any texture filtering to our sprites
            _layers.ForEach( l => l.SamplerState = SamplerState.PointWrap );
        }

        // just a quick and dirty example to show off the camera zoom :D
        // also, Keys.PageUp and Keys.PageDown do not work. At all.
        public override void HandleInput( FragEngine.InputState input )
        {
            base.HandleInput( input );

            var camera = ServiceLocator.Get<Camera>();


            // Zooming
            if( input.CurrentKeyboardState.IsKeyDown( Keys.Q ) ) camera.Zoom += 0.1f;
            if( input.CurrentKeyboardState.IsKeyDown( Keys.W ) ) camera.Zoom = 1f;
            if( input.CurrentKeyboardState.IsKeyDown( Keys.E ) ) camera.Zoom -= 0.1f;

            // Rotation
            if( input.CurrentKeyboardState.IsKeyDown( Keys.A ) ) camera.Rotation -= 0.1f;
            if( input.CurrentKeyboardState.IsKeyDown( Keys.S ) ) camera.Rotation = 0f;
            if( input.CurrentKeyboardState.IsKeyDown( Keys.D ) ) camera.Rotation += 0.1f;

            // Zoom Presets
            if( input.CurrentKeyboardState.IsKeyDown( Keys.NumPad1 ) ) camera.Zoom = 1f;
            if( input.CurrentKeyboardState.IsKeyDown( Keys.NumPad2 ) ) camera.Zoom = 2f;
            if( input.CurrentKeyboardState.IsKeyDown( Keys.NumPad3 ) ) camera.Zoom = 3f;
            if( input.CurrentKeyboardState.IsKeyDown( Keys.NumPad4 ) ) camera.Zoom = 4f;
            if( input.CurrentKeyboardState.IsKeyDown( Keys.NumPad5 ) ) camera.Zoom = 5f;
            if( input.CurrentKeyboardState.IsKeyDown( Keys.NumPad6 ) ) camera.Zoom = 6f;
            if( input.CurrentKeyboardState.IsKeyDown( Keys.NumPad7 ) ) camera.Zoom = 7f;
            if( input.CurrentKeyboardState.IsKeyDown( Keys.NumPad8 ) ) camera.Zoom = 8f;
            if( input.CurrentKeyboardState.IsKeyDown( Keys.NumPad9 ) ) camera.Zoom = 9f;
            if( input.CurrentKeyboardState.IsKeyDown( Keys.NumPad0 ) ) camera.Zoom = 10f;

            if( input.CurrentKeyboardState.IsKeyDown( Keys.OemPlus ) ) camera.Zoom += 1f;
            if( input.CurrentKeyboardState.IsKeyDown( Keys.OemMinus ) ) camera.Zoom -= 1f;

            if( input.CurrentKeyboardState.IsKeyDown( Keys.LeftShift ) || input.CurrentKeyboardState.IsKeyDown( Keys.RightShift ) )
            {
                if( input.CurrentKeyboardState.IsKeyDown( Keys.H ) ) camera.Target.BoundingBox += new Vector2( 1, 0 );
                if( input.CurrentKeyboardState.IsKeyDown( Keys.L ) ) camera.Target.BoundingBox -= new Vector2( 1, 0 );
                if( input.CurrentKeyboardState.IsKeyDown( Keys.K ) ) camera.Target.BoundingBox += new Vector2( 0, 1 );
                if( input.CurrentKeyboardState.IsKeyDown( Keys.J ) ) camera.Target.BoundingBox -= new Vector2( 0, 1 );
            }
            else
            {
                if( input.CurrentKeyboardState.IsKeyDown( Keys.H ) ) camera.Target.Offset += new Vector2( 1, 0 );
                if( input.CurrentKeyboardState.IsKeyDown( Keys.L ) ) camera.Target.Offset -= new Vector2( 1, 0 );
                if( input.CurrentKeyboardState.IsKeyDown( Keys.K ) ) camera.Target.Offset += new Vector2( 0, 1 );
                if( input.CurrentKeyboardState.IsKeyDown( Keys.J ) ) camera.Target.Offset -= new Vector2( 0, 1 );
            }

        }
    }
}
