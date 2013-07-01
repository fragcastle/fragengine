using System;
using FragEngine;
using FragEngine.Animation;
using FragEngine.Entities;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace JumpJoy.Entities
{
    public class Jumper : Player
    {

        private bool _againstWallLeft;
        private bool _againstWallRight;

        private int _jumpCount;

        protected override void Initialize()
        {
            Collision = CollisionType.A;

            MaxVelocity = new Vector2 { X = 250, Y = 250 };

            Animations = new AnimationSheet( @"Textures\player", 64, 64 );

            Animations.Add( "idle", 1f, true, 0, 1 );

            Animations.Add( "run", 0.07f, true, 12, 13, 14, 15, 16, 17, 18, 19, 20 );

            Animations.Add( "jump", 0.09f, false, 21, 22 );

            Animations.Add( "wallSlideRight", 1f, true, 2 );
            Animations.Add( "wallSlideLeft", 1f, true, 3 );

            Index = PlayerIndex.One;

            // our player only has horizontal friction
            Friction = new Vector2( 2000, 0 );

            base.Initialize();
        }

        public override void Update( GameTime gameTime )
        {
            base.Update( gameTime );

            // we made contact with the ground, reset the jump count
            if( Standing )
                _jumpCount = 0;
        }

        // custom method to do some extra work after the collision check
        protected override void UpdateEntityState( FragEngine.Mapping.CollisionCheckResult result )
        {
            base.UpdateEntityState( result );

            _againstWallLeft = result.XAxis && Velocity.X < 0;
            _againstWallRight = result.XAxis && Velocity.X >= 0;
        }

        public override void HandleKeyboardInput(KeyboardState keyboard)
        {
            Acceleration = Vector2.Zero;

            float velocity_x = 0, velocity_y = 0;

            Animations.SetCurrentAnimation( "idle" );

            if( keyboard.IsKeyDown( Keys.Left ) || keyboard.IsKeyDown( Keys.Right ) )
            {
                Animations.SetCurrentAnimation( "run" );

                var mod = keyboard.IsKeyDown( Keys.Left ) ? -1 : 1;
                velocity_x += 300 * mod;

                if( Math.Abs(velocity_x) > MaxVelocity.X )
                    velocity_x = MaxVelocity.X * mod;

                if( !Standing && ( _againstWallLeft && keyboard.IsKeyDown( Keys.Left ) || _againstWallRight && keyboard.IsKeyDown( Keys.Right ) ) )
                {
                    _jumpCount = 0;

                    if( _againstWallLeft && keyboard.IsKeyDown( Keys.Left ) )
                        Animations.SetCurrentAnimation( "wallSlideLeft" );

                    if( _againstWallRight && keyboard.IsKeyDown( Keys.Right ) )
                        Animations.SetCurrentAnimation( "wallSlideRight" );

                    GravityFactor = 1.0f;

                    Velocity *= new Vector2(1, 0.8f);
                }
            }

            if( keyboard.IsKeyDown( Keys.Up ) && ( Standing || _jumpCount <= 1 ) )
            {
                velocity_y -= 100;

                if( _againstWallLeft || _againstWallRight )
                {
                    Velocity = new Vector2( _againstWallRight ? -500 : 500, Velocity.Y );

                    _againstWallRight = _againstWallLeft = false;
                }

                _jumpCount++;
            }

            if( Velocity.Y != 0 && !Standing )
            {
                Animations.SetCurrentAnimation( "jump" );
            }

            Animations.CurrentAnimation.FlipX = Velocity.X < 0;

            Acceleration = new Vector2( velocity_x, velocity_y );
        }

        public override void HandleGamePadInput(GamePadState gamepad)
        {
            // no gamepad bindings yet
        }
    }
}
