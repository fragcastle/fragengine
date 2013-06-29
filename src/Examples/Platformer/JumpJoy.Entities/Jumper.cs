using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FragEngine;
using FragEngine.Animation;
using FragEngine.Entities;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace JumpJoy.Entities
{
    public class Jumper : Player
    {
        public Jumper()
        {
            Collision = CollisionType.A;

            MaxVelocity = new Vector2 { X = 250, Y = 250 };

            Acceleration = 5;

            Animations = new AnimationSheet( @"Textures\player", 64, 64 );

            Animations.Add( "idle", 1f, true, 0, 1 );

            Animations.Add( "run", 0.07f, true, 12, 13, 14, 15, 16, 17, 18, 19, 20 );
        }

        public override void Update( GameTime time )
        {
            float velocity_x = 0, velocity_y = 0;

            Animations.SetCurrentAnimation( "idle" );
            var keyState = Keyboard.GetState();

            if( keyState.IsKeyDown( Keys.Left ) )
            {
                Animations.SetCurrentAnimation( "run" );
                Animations.CurrentAnimation.FlipX = true;
                velocity_x = -Acceleration;
            }

            if( keyState.IsKeyDown( Keys.Right ) )
            {
                Animations.SetCurrentAnimation( "run" );
                velocity_x = Acceleration;
            }

            Velocity = new Vector2( velocity_x, velocity_y );

            base.Update( time );
        }
    }
}
