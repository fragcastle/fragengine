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
        public JumpPlayScreen(HudBase hud) : base(hud)
        {
        }

        public override void Initialize()
        {
            base.Initialize();

            // load the level

            var path = Path.Combine( AppDomain.CurrentDomain.BaseDirectory, "Data\\jumpjoy_1.json" );

            var level = Level.Load( new FileInfo( path ) );

            var jump = level.Entities.First( e => e as Jumper != null );

            var camera = ServiceInjector.Get<Camera>();

            camera.Target = jump;

            // this kind of works... but seems odd...
            camera.Offset = new Vector2( 48, 256 );

            _playerLayer.Entities.Add( jump );

            _layers.InsertRange( 0, level.MapLayers );

            // tell XNA not to apply any texture filtering to our sprites
            _layers.ForEach( l => l.SamplerState = SamplerState.PointWrap );
        }

        // just a quick and dirty example to show off the camera zoom :D
        // also, Keys.PageUp and Keys.PageDown do not work. At all.
        public override void HandleInput( FragEngine.InputState input )
        {
            base.HandleInput( input );

            var camera = ServiceInjector.Get<Camera>();

            if( input.CurrentKeyboardState.IsKeyDown( Keys.Q ) )
            {
                camera.Zoom += 0.1f;
            }

            if( input.CurrentKeyboardState.IsKeyDown( Keys.E ) )
            {
                camera.Zoom -= 0.1f;
            }
        }
    }
}
