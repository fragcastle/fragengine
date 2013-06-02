using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace FragEngine.Entities.Hud
{
    public class HudButton : EntityBase
    {

        public enum HudButtonState
        {
            Up = 0,
            Down = 1,
            Hover = 2
        }

        public HudButtonState ButtonState { get; private set; }

        public Action WhenClicked { get; set; }

        public string Text { get; set; }

        private Vector2 _staticPosition;

        private bool _alreadyClicked;

        public HudButton()
            : this( Vector2.Zero, Vector2.Zero )
        {

        }

        public HudButton( Vector2 initialLocation, Vector2 initialVelocity )
            : base( initialLocation, initialVelocity )
        {

        }

        protected override void Initialize()
        {
            // setup default animations
            Animations.Add( "up", 1, false, 0 );
            Animations.Add( "down", 1, false, 1 );
            Animations.Add( "hover", 1, false, 2 );

            _staticPosition = Position;
        }

        public override void Update( GameTime time )
        {
            UpdateButtonPosition();
            UpdateButtonState();

            if (ButtonState == HudButtonState.Hover && Animations.HasAnimation("hover"))
            {
                Animations.SetCurrentAnimation("hover");
            }

            if( ButtonState == HudButtonState.Down && Animations.HasAnimation( "down" ) )
            {
                Animations.SetCurrentAnimation( "down" );
            }

            if( ButtonState == HudButtonState.Up && Animations.HasAnimation( "up" ) )
            {
                Animations.SetCurrentAnimation( "up" );
            }

            base.Update( time );
        }

        private void UpdateButtonPosition()
        {
            // buttons are fixed in the ui at all times
            Position = FragEngineGame.ScreenManager.Camera.ScreenToGame( _staticPosition );
        }

        public override void Draw( Microsoft.Xna.Framework.Graphics.SpriteBatch batch )
        {
            base.Draw( batch );

            if( ButtonState == HudButtonState.Down && !_alreadyClicked )
            {
                if( WhenClicked != null )
                    WhenClicked();
                _alreadyClicked = true;
            }

            if( ButtonState == HudButtonState.Up && _alreadyClicked )
            {
                _alreadyClicked = false;
            }
        }

        private void UpdateButtonState()
        {
            var mouseState = Mouse.GetState();

            var mousePosition = FragEngineGame.ScreenManager.Camera.ScreenToGame( new Vector2( mouseState.X, mouseState.Y ) );

            var mouseCollide = new Rectangle( (int)mousePosition.X, (int)mousePosition.Y, 1, 1 );

            if (CollidesWith(mouseCollide))
            {
                // mouse is over us
                ButtonState = HudButtonState.Hover;
                if (mouseState.LeftButton == Microsoft.Xna.Framework.Input.ButtonState.Pressed)
                {
                    ButtonState = HudButtonState.Down;
                }
            }
            else
            {
                ButtonState = HudButtonState.Up;
            }
        }
    }
}
