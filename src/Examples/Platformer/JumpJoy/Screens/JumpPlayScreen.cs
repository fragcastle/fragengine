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
using Microsoft.Xna.Framework.Graphics;

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

            _playerLayer.Entities.Add( jump );

            _layers.InsertRange( 0, level.MapLayers );

            // tell XNA not to apply any texture filtering to our sprites
            _layers.ForEach( l => l.SamplerState = SamplerState.PointWrap );
        }
    }
}
