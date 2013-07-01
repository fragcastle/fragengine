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

        public override void Update( GameTime gameTime )
        {
            base.Update( gameTime );

            if( inputState != null )
            {
                inputState.Update();

                HandleKeyboardInput( inputState.CurrentKeyboardState );
                HandleGamePadInput( inputState.CurrentGamePadState );
            }
        }

        public abstract void HandleKeyboardInput( KeyboardState keyboard );
        public abstract void HandleGamePadInput( GamePadState gamepad );
    }
}
