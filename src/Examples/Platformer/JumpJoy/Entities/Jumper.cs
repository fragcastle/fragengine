using FragEngine;
using FragEngine.Animation;
using FragEngine.Collisions;
using FragEngine.GameObjects;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace JumpJoy.Entities
{
    public class Jumper : Player
    {
        private int _jumpCount;

        private Vector2 _speed = new Vector2(1000, 300);

        public Jumper()
        {
            Group = GameObjectGroup.A;
            CheckAgainstGroup = GameObjectGroup.B;
            CollisionStyle = GameObjectCollisionStyle.Passive;

            Name = "Jump";

            Animations = new AnimationSheet(ContentCacheManager.GetTextureFromResource(@"JumpJoy.Resources.rock.png", typeof(Jumper).Assembly), 32, 32);

            Animations.Add("idle", 0.08f, true, 1, 2, 3, 4, 5, 6, 7);
            Animations.Add("run", 0.07f, true, 45, 46, 47, 48, 49, 50);
            Animations.Add("jump", 1f, false, 73);

            Index = PlayerIndex.One;

            // our player only has horizontal friction
            Friction = new Vector2(1000, 100);
            Acceleration = Vector2.Zero;
            MaxVelocity = new Vector2(150, 180);

            Settings["_feDrawBox"] = "true";

            BoundingBox = new HitBox { Height = 22, Width = 8 };
            Offset = new Vector2(12, 10);

            GravityFactor = 10f;
        }

        public override void Update(GameTime gameTime)
        {
            // we made contact with the ground, reset the jump count
            if (IsStanding)
                _jumpCount = 0;

            GravityFactor = 10f;

            // time to pick the current animation
            // pick animation based on what the actor is doing
            if (!IsStanding && Velocity.Y != 0)
                CurrentAnimation = "jump";
            else if (Velocity.X != 0f)
                CurrentAnimation = "run";
            else if (IsStanding)
                CurrentAnimation = "idle";

            base.Update(gameTime);
        }

        public override void HandleKeyboardInput(KeyboardState keyboard)
        {
            var left = keyboard.IsKeyDown(Keys.Left);
            var right = keyboard.IsKeyDown(Keys.Right);
            var jumped = keyboard.IsKeyDown(Keys.Up);

            if (left)
                GoLeft();
            else if (right)
                GoRight();

            if (jumped)
                Jump();
        }

        public override void HandleGamePadInput(GamePadState gamepad)
        {
            if (gamepad.DPad.Left == ButtonState.Pressed)
            {
                GoLeft();
            }
            else if (gamepad.DPad.Right == ButtonState.Pressed)
            {
                GoRight();
            }

            if (gamepad.Buttons.A == ButtonState.Pressed)
            {
                Jump();
            }
        }

        private void GoLeft()
        {
            Acceleration = Acceleration.SetX(-_speed.X);
            FlipAnimation = true;
        }

        private void GoRight()
        {
            Acceleration = Acceleration.SetX(_speed.X);
            FlipAnimation = false;
        }

        private void Jump()
        {
            if (_jumpCount >= 2)
                return;

            Velocity = Velocity.SetY(-_speed.Y);

            if (CurrentAnimation != "jump")
                Animations.SetCurrentAnimation("jump");

            _jumpCount++;
        }
    }
}
