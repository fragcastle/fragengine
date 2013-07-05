using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace FragEngine.Entities
{
    public abstract class Player: Actor
    {

        protected InputState inputState;

        public PlayerIndex Index { get; set; }

        public Player() : base( Vector2.Zero, Vector2.Zero )
        {

        }

        protected override void Initialize()
        {
            base.Initialize();

            inputState = new InputState( Index );
        }

        public override void Update( GameTime gameTime )
        {
            // zero out the acceleration
            Acceleration = Vector2.Zero;

            if( inputState != null )
            {
                inputState.Update();

                HandleKeyboardInput( inputState.CurrentKeyboardState );
                HandleGamePadInput( inputState.CurrentGamePadState );
            }

            base.Update( gameTime );
        }

        public abstract void HandleKeyboardInput( KeyboardState keyboard );
        public abstract void HandleGamePadInput( GamePadState gamepad );
    }
}
