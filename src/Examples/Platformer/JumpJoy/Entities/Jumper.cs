using FragEngine;
using FragEngine.Animation;
using FragEngine.Collisions;
using FragEngine.Entities;
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

            Name = "Jump";

            Animations = new AnimationSheet(@"Textures\player", 64, 64);

            Animations.Add("idle", 1f, true, 0, 1);

            Animations.Add("run", 0.07f, true, 12, 13, 14, 15, 16, 17, 18, 19, 20);

            Animations.Add("jump", 0.09f, false, 21, 22);

            Index = PlayerIndex.One;

            // our player only has horizontal friction
            Friction = new Vector2(1000, 100);
            Acceleration = Vector2.Zero;
            MaxVelocity = new Vector2(150, 180);

            Settings["_feDrawBox"] = "true";

            BoundingBox = new HitBox { Height = 64, Width = 64 };

            GravityFactor = 10f;
        }

        public override void Update(GameTime gameTime)
        {
            // we made contact with the ground, reset the jump count
            if (IsStanding)
                _jumpCount = 0;

            GravityFactor = 10f;

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

            // time to pick the current animation
            // pick animation based on what the actor is doing
            if (!IsStanding && Velocity.Y != 0)
                CurrentAnimation = "jump";
            else if (Velocity.X != 0f)
                CurrentAnimation = "run";
            else if (IsStanding)
                CurrentAnimation = "idle";
        }

        public override void HandleGamePadInput(GamePadState gamepad)
        {
            // no gamepad bindings yet
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
