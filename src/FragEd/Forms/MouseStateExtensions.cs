using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Microsoft.Xna.Framework.Input;
using ButtonState = Microsoft.Xna.Framework.Input.ButtonState;

namespace FragEd.Forms
{
    public static class MouseStateHelpers
    {
        public static MouseEventArgs GetMouseEventArgs()
        {
            var mouseState = Mouse.GetState();

            var buttons = MouseButtons.None;

            if( mouseState.LeftButton == ButtonState.Pressed )
            {
                buttons = buttons | MouseButtons.Left;
            }

            if( mouseState.MiddleButton == ButtonState.Pressed )
            {
                buttons = buttons | MouseButtons.Middle;
            }

            if( mouseState.RightButton == ButtonState.Pressed )
            {
                buttons = buttons | MouseButtons.Right;
            }

            return new MouseEventArgs( buttons, 0, mouseState.X, mouseState.Y, 0 );
        }
    }
}
