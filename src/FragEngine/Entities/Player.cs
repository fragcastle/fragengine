using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace FragEngine.Entities
{
    public abstract class Player: EntityBase
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

        public override void Update( GameTime time )
        {
            // allow player objects to respond to game input before their update is done
            if( inputState != null )
            {
                inputState.Update();

                if( inputState.KeyboardStateDirty() )
                    HandleKeyboardInput( inputState.CurrentKeyboardState );

                if( inputState.GamePadStateDirty() )
                    HandleGamePadInput( inputState.CurrentGamePadState );
            }

            base.Update( time );
        }

        public abstract void HandleKeyboardInput( KeyboardState keyboard );
        public abstract void HandleGamePadInput( GamePadState gamepad );
    }
}
