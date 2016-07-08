using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace FragEngine.Entities
{
    public abstract class Player: GameObject
    {

        protected InputState inputState;

        public PlayerIndex Index { get; set; }

        public Player() : base( Vector2.Zero, Vector2.Zero )
        {
            inputState = new InputState( Index );
        }

        public override void Update( GameTime gameTime )
        {
            if( inputState != null )
            {
                inputState.Update();

                HandleKeyboardInput( inputState.CurrentKeyboardState );
                HandleGamePadInput( inputState.CurrentGamePadState );
            }

            base.Update( gameTime );

            Acceleration = Vector2.Zero;
        }

        public abstract void HandleKeyboardInput( KeyboardState keyboard );
        public abstract void HandleGamePadInput( GamePadState gamepad );
    }
}
