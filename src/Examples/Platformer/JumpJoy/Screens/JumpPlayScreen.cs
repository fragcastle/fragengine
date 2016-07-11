using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using FragEngine.Data;
using FragEngine.GameObjects;
using FragEngine.Services;
using FragEngine.View;
using FragEngine.View.Screens;
using FragEngine.View.Screens.Play;
using JumpJoy.Entities;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace JumpJoy.Screens
{
    public class JumpPlayScreen : PlayScreen
    {
        public override void Initialize()
        {
            base.Initialize();

            // TODO: load the level

            var jump = ServiceLocator.Get<IGameObjectService>().GetGameObjectByName("Jump");

            var camera = ServiceLocator.Get<Camera>();

            camera.Target = jump;
            camera.Zoom = 5f;

            // this kind of works... but seems odd...
            camera.Offset = new Vector2(48, 256);

            // tell XNA not to apply any texture filtering to our sprites
            Layers.ForEach(l => l.SamplerState = SamplerState.PointWrap);
        }

        // just a quick and dirty example to show off the camera zoom :D
        // also, Keys.PageUp and Keys.PageDown do not work. At all.
        public override void HandleInput(FragEngine.InputState input)
        {
            base.HandleInput(input);

            var camera = ServiceLocator.Get<Camera>();


            // Zooming
            if (input.CurrentKeyboardState.IsKeyDown(Keys.Q)) camera.Zoom += 0.1f;
            if (input.CurrentKeyboardState.IsKeyDown(Keys.W)) camera.Zoom = 1f;
            if (input.CurrentKeyboardState.IsKeyDown(Keys.E)) camera.Zoom -= 0.1f;

            // Rotation
            if (input.CurrentKeyboardState.IsKeyDown(Keys.A)) camera.Rotation -= 0.1f;
            if (input.CurrentKeyboardState.IsKeyDown(Keys.S)) camera.Rotation = 0f;
            if (input.CurrentKeyboardState.IsKeyDown(Keys.D)) camera.Rotation += 0.1f;

            // Zoom Presets
            if (input.CurrentKeyboardState.IsKeyDown(Keys.NumPad1)) camera.Zoom = 1f;
            if (input.CurrentKeyboardState.IsKeyDown(Keys.NumPad2)) camera.Zoom = 2f;
            if (input.CurrentKeyboardState.IsKeyDown(Keys.NumPad3)) camera.Zoom = 3f;
            if (input.CurrentKeyboardState.IsKeyDown(Keys.NumPad4)) camera.Zoom = 4f;
            if (input.CurrentKeyboardState.IsKeyDown(Keys.NumPad5)) camera.Zoom = 5f;
            if (input.CurrentKeyboardState.IsKeyDown(Keys.NumPad6)) camera.Zoom = 6f;
            if (input.CurrentKeyboardState.IsKeyDown(Keys.NumPad7)) camera.Zoom = 7f;
            if (input.CurrentKeyboardState.IsKeyDown(Keys.NumPad8)) camera.Zoom = 8f;
            if (input.CurrentKeyboardState.IsKeyDown(Keys.NumPad9)) camera.Zoom = 9f;
            if (input.CurrentKeyboardState.IsKeyDown(Keys.NumPad0)) camera.Zoom = 10f;

            if (input.CurrentKeyboardState.IsKeyDown(Keys.OemPlus)) camera.Zoom += 1f;
            if (input.CurrentKeyboardState.IsKeyDown(Keys.OemMinus)) camera.Zoom -= 1f;

            if (input.CurrentKeyboardState.IsKeyDown(Keys.T)) camera.ShakeAmount += 0.1f;
            if (input.CurrentKeyboardState.IsKeyDown(Keys.Y)) camera.ShakeAmount -= 0.1f;
            if (input.CurrentKeyboardState.IsKeyUp(Keys.U)) shakeLevelToggle = false;
            if (input.CurrentKeyboardState.IsKeyDown(Keys.U) && !shakeLevelToggle)
            {
                SetShakeLevel();
                shakeLevelToggle = true;
            }

            if (input.CurrentKeyboardState.IsKeyDown(Keys.LeftShift) || input.CurrentKeyboardState.IsKeyDown(Keys.RightShift))
            {
                if (input.CurrentKeyboardState.IsKeyDown(Keys.H)) camera.Target.BoundingBox += new Vector2(1, 0);
                if (input.CurrentKeyboardState.IsKeyDown(Keys.L)) camera.Target.BoundingBox -= new Vector2(1, 0);
                if (input.CurrentKeyboardState.IsKeyDown(Keys.K)) camera.Target.BoundingBox += new Vector2(0, 1);
                if (input.CurrentKeyboardState.IsKeyDown(Keys.J)) camera.Target.BoundingBox -= new Vector2(0, 1);
            }
            else
            {
                if (input.CurrentKeyboardState.IsKeyDown(Keys.H)) camera.Target.Offset += new Vector2(1, 0);
                if (input.CurrentKeyboardState.IsKeyDown(Keys.L)) camera.Target.Offset -= new Vector2(1, 0);
                if (input.CurrentKeyboardState.IsKeyDown(Keys.K)) camera.Target.Offset += new Vector2(0, 1);
                if (input.CurrentKeyboardState.IsKeyDown(Keys.J)) camera.Target.Offset -= new Vector2(0, 1);
            }

        }

        private bool shakeLevelToggle = false;
        private void SetShakeLevel()
        {
            var camera = ServiceLocator.Get<Camera>();
            if (camera.ShakeLevel == CameraShakeLevel.AlmostNone) camera.ShakeLevel = CameraShakeLevel.ALittle;
            else if (camera.ShakeLevel == CameraShakeLevel.ALittle) camera.ShakeLevel = CameraShakeLevel.Some;
            else if (camera.ShakeLevel == CameraShakeLevel.Some) camera.ShakeLevel = CameraShakeLevel.More;
            else if (camera.ShakeLevel == CameraShakeLevel.More) camera.ShakeLevel = CameraShakeLevel.ALot;
            else if (camera.ShakeLevel == CameraShakeLevel.ALot) camera.ShakeLevel = CameraShakeLevel.TooMuch;
            else if (camera.ShakeLevel == CameraShakeLevel.TooMuch) camera.ShakeLevel = CameraShakeLevel.Vlambeer;
            else if (camera.ShakeLevel == CameraShakeLevel.Vlambeer) camera.ShakeLevel = CameraShakeLevel.None;
            else if (camera.ShakeLevel == CameraShakeLevel.None) camera.ShakeLevel = CameraShakeLevel.AlmostNone;
        }
    }
}
