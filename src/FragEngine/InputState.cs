using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework;

namespace FragEngine
{
    /// <summary>
    /// Helper for reading input from keyboard and gamepad. This public class tracks
    /// the current and previous state of both input devices, and implements query
    /// properties for high level input actions such as "move up through the menu"
    /// or "pause the game".
    /// </summary>
    /// <remarks>
    /// This public class is similar to one in the GameStateManagement sample.
    /// </remarks>
    public class InputState
    {

        private PlayerIndex _index;

        public InputState( PlayerIndex index )
        {
            _index = index;
        }

        public static Dictionary<Keys, char> KeyCharacterMap = new Dictionary<Keys, char>()
            {
                { Keys.A, 'a' }, { Keys.B, 'b' },
                { Keys.C, 'c' }, { Keys.D, 'd' },
                { Keys.E, 'e' }, { Keys.F, 'f' },
                { Keys.G, 'g' }, { Keys.H, 'h' },
                { Keys.I, 'i' }, { Keys.J, 'j' },
                { Keys.K, 'k' }, { Keys.L, 'l' },
                { Keys.M, 'm' }, { Keys.N, 'n' },
                { Keys.O, 'o' }, { Keys.P, 'p' },
                { Keys.Q, 'q' }, { Keys.R, 'r' },
                { Keys.S, 's' }, { Keys.T, 't' },
                { Keys.U, 'u' }, { Keys.V, 'v' },
                { Keys.W, 'w' }, { Keys.X, 'x' },
                { Keys.Y, 'y' }, { Keys.Z, 'z' }
            };


        public KeyboardState CurrentKeyboardState;
        public GamePadState CurrentGamePadState;

        public KeyboardState LastKeyboardState;
        public GamePadState LastGamePadState;

        /// <summary>
        /// Checks for a "menu up" input action (on either keyboard or gamepad).
        /// </summary>
        public bool MenuUp
        {
            get
            {
                return IsNewKeyPress( Keys.Up ) ||
                       ( CurrentGamePadState.DPad.Up == ButtonState.Pressed &&
                        LastGamePadState.DPad.Up == ButtonState.Released ) ||
                       ( CurrentGamePadState.ThumbSticks.Left.Y > 0 &&
                        LastGamePadState.ThumbSticks.Left.Y <= 0 );
            }
        }


        /// <summary>
        /// Checks for a "menu down" input action (on either keyboard or gamepad).
        /// </summary>
        public bool MenuDown
        {
            get
            {
                return IsNewKeyPress( Keys.Down ) ||
                       ( CurrentGamePadState.DPad.Down == ButtonState.Pressed &&
                        LastGamePadState.DPad.Down == ButtonState.Released ) ||
                       ( CurrentGamePadState.ThumbSticks.Left.Y < 0 &&
                        LastGamePadState.ThumbSticks.Left.Y >= 0 );
            }
        }


        /// <summary>
        /// Checks for a "menu select" input action (on either keyboard or gamepad).
        /// </summary>
        public bool MenuSelect
        {
            get
            {
                return IsNewKeyPress( Keys.Space ) ||
                       IsNewKeyPress( Keys.Enter ) ||
                       ( CurrentGamePadState.Buttons.A == ButtonState.Pressed &&
                        LastGamePadState.Buttons.A == ButtonState.Released ) ||
                       ( CurrentGamePadState.Buttons.Start == ButtonState.Pressed &&
                        LastGamePadState.Buttons.Start == ButtonState.Released );
            }
        }


        /// <summary>
        /// Checks for a "menu cancel" input action (on either keyboard or gamepad).
        /// </summary>
        public bool MenuCancel
        {
            get
            {
                return IsNewKeyPress( Keys.Escape ) ||
                       ( CurrentGamePadState.Buttons.B == ButtonState.Pressed &&
                        LastGamePadState.Buttons.B == ButtonState.Released ) ||
                       ( CurrentGamePadState.Buttons.Back == ButtonState.Pressed &&
                        LastGamePadState.Buttons.Back == ButtonState.Released );
            }
        }


        /// <summary>
        /// Checks for a "pause the game" input action (on either keyboard or gamepad).
        /// </summary>
        public bool PauseGame
        {
            get
            {
                return IsNewKeyPress( Keys.Escape ) ||
                       ( CurrentGamePadState.Buttons.Back == ButtonState.Pressed &&
                        LastGamePadState.Buttons.Back == ButtonState.Released ) ||
                       ( CurrentGamePadState.Buttons.Start == ButtonState.Pressed &&
                        LastGamePadState.Buttons.Start == ButtonState.Released );
            }
        }

        /// <summary>
        /// Checks for a "mark ready" input action (on either keyboard or gamepad).
        /// </summary>
        public bool MarkReady
        {
            get
            {
                return IsNewKeyPress( Keys.X ) ||
                       ( CurrentGamePadState.Buttons.X == ButtonState.Pressed &&
                        LastGamePadState.Buttons.X == ButtonState.Released );
            }
        }

        /// <summary>
        /// Reads the latest state of the keyboard and gamepad.
        /// </summary>
        public void Update()
        {
            LastKeyboardState = CurrentKeyboardState;
            LastGamePadState = CurrentGamePadState;

            CurrentKeyboardState = Keyboard.GetState();
            CurrentGamePadState = GamePad.GetState( _index );
        }

        public bool GamePadStateDirty()
        {
            return LastGamePadState == CurrentGamePadState;
        }

        public bool KeyboardStateDirty()
        {
            return LastKeyboardState == CurrentKeyboardState;
        }

        /// <summary>
        /// Helper for checking if a key was newly pressed during this update.
        /// </summary>
        public bool IsNewKeyPress( Keys key )
        {
            return ( CurrentKeyboardState.IsKeyDown( key ) &&
                    LastKeyboardState.IsKeyUp( key ) );
        }

        public string PressedKeyValue()
        {
            var text = new StringBuilder();
            foreach (var key in CurrentKeyboardState.GetPressedKeys())
            {
                if( IsNewKeyPress(key))
                    text.Append(KeyCharacterMap[key]);
            }
            return text.ToString();
        }
    }
}
