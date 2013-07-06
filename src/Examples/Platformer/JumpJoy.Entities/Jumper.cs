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

        private int _airAccel = 1250;
        private int _groundAccel = 2000;

        private int _jumpSpeed = 350;

        private bool _againstWall = false;

        protected override void Initialize()
        {
            Collision = CollisionType.A;

            MaxVelocity = new Vector2 { X = 300, Y = 450 };

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
            // we made contact with the ground, reset the jump count
            if( Standing )
                _jumpCount = 0;

            base.Update( gameTime );
        }

        // custom method to do some extra work after the collision check
        protected override void UpdateEntityState( FragEngine.Mapping.CollisionCheckResult result )
        {

            base.UpdateEntityState( result );

            _againstWall = result.XAxis;
            _againstWallLeft = !Standing && result.XAxis && Acceleration.X < 0;
            _againstWallRight = !Standing && result.XAxis && Acceleration.X > 0;
        }

        public override void HandleKeyboardInput(KeyboardState keyboard)
        {
            var left = keyboard.IsKeyDown( Keys.Left );
            var right = keyboard.IsKeyDown( Keys.Right );
            var jumped = keyboard.IsKeyDown( Keys.Up );

            if( left )
                GoLeft();
            else if( right )
                GoRight();

            if( jumped )
                Jump();


            if( !Standing && !jumped && _againstWall && ( left || right ) )
            {
                _jumpCount = 0;

                if( _againstWallLeft && left )
                    CurrentAnimation = "wallSlideLeft";
                else if( _againstWallRight && right )
                    CurrentAnimation = "wallSlideRight";

                GravityFactor = 1.0f;

                Velocity *= new Vector2( 1, 0.8f );
            }
            else
            {
                // pick animation based on what the actor is doing
                if( !Standing && Velocity.Y != 0 && !_againstWallRight && !_againstWallLeft )
                    CurrentAnimation = "jump";
                else if( Acceleration.X != 0f )
                    CurrentAnimation = "run";
                else if( Standing && CurrentAnimation != "talking" )
                    CurrentAnimation = "idle";
            }
        }

        public override void HandleGamePadInput(GamePadState gamepad)
        {
            // no gamepad bindings yet
        }

        private void GoLeft()
        {
            var accel = Standing ? _groundAccel : _airAccel;
            Acceleration = new Vector2( -accel, Acceleration.Y );
            FlipAnimation = true;
        }

        private void GoRight()
        {
            var accel = Standing ? _groundAccel : _airAccel;
            Acceleration = new Vector2( accel, Acceleration.Y );
            FlipAnimation = false;
        }

        private void Jump()
        {
            if( _jumpCount >= 2 )
                return;

            Velocity = new Vector2( Velocity.X, -_jumpSpeed );

            if( _againstWallLeft || _againstWallRight )
            {
                Velocity = new Vector2( _againstWallRight ? -500 : 500, Velocity.Y );

                _againstWallRight = _againstWallLeft = false;
            }

            if( CurrentAnimation != "jump" )
                Animations.SetCurrentAnimation( "jump" );

            _jumpCount++;
        }
    }
}
