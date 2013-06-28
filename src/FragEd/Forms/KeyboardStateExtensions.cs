using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Microsoft.Xna.Framework.Input;
using Keys = Microsoft.Xna.Framework.Input.Keys;

namespace FragEd.Forms {
    public static class KeyboardStateHelpers {

        private static IDictionary<Microsoft.Xna.Framework.Input.Keys, System.Windows.Forms.Keys> map;

        static KeyboardStateHelpers() {
            map = new Dictionary<Keys, System.Windows.Forms.Keys>() {
                { Keys.None,                System.Windows.Forms.Keys.None },
                { Keys.Back,                System.Windows.Forms.Keys.Back },
                { Keys.Tab,                 System.Windows.Forms.Keys.Tab },
                { Keys.Enter,               System.Windows.Forms.Keys.Enter },
                { Keys.Pause,               System.Windows.Forms.Keys.Pause },
                { Keys.CapsLock,            System.Windows.Forms.Keys.CapsLock },
                { Keys.Escape,              System.Windows.Forms.Keys.Escape },
                { Keys.Space,               System.Windows.Forms.Keys.Space },
                { Keys.PageUp,              System.Windows.Forms.Keys.PageUp },
                { Keys.PageDown,            System.Windows.Forms.Keys.PageDown },
                { Keys.End,                 System.Windows.Forms.Keys.End },
                { Keys.Home,                System.Windows.Forms.Keys.Home },
                { Keys.Left,                System.Windows.Forms.Keys.Left },
                { Keys.Up,                  System.Windows.Forms.Keys.Up },
                { Keys.Right,               System.Windows.Forms.Keys.Right },
                { Keys.Down,                System.Windows.Forms.Keys.Down },
                { Keys.Select,              System.Windows.Forms.Keys.Select },
                { Keys.Print,               System.Windows.Forms.Keys.Print },
                { Keys.Execute,             System.Windows.Forms.Keys.Execute },
                { Keys.PrintScreen,         System.Windows.Forms.Keys.PrintScreen },
                { Keys.Insert,              System.Windows.Forms.Keys.Insert },
                { Keys.Delete,              System.Windows.Forms.Keys.Delete },
                { Keys.Help,                System.Windows.Forms.Keys.Help },
                { Keys.D0,                  System.Windows.Forms.Keys.D0 },
                { Keys.D1,                  System.Windows.Forms.Keys.D1 },
                { Keys.D2,                  System.Windows.Forms.Keys.D2 },
                { Keys.D3,                  System.Windows.Forms.Keys.D3 },
                { Keys.D4,                  System.Windows.Forms.Keys.D4 },
                { Keys.D5,                  System.Windows.Forms.Keys.D5 },
                { Keys.D6,                  System.Windows.Forms.Keys.D6 },
                { Keys.D7,                  System.Windows.Forms.Keys.D7 },
                { Keys.D8,                  System.Windows.Forms.Keys.D8 },
                { Keys.D9,                  System.Windows.Forms.Keys.D9 },
                { Keys.A,                   System.Windows.Forms.Keys.A },
                { Keys.B,                   System.Windows.Forms.Keys.B },
                { Keys.C,                   System.Windows.Forms.Keys.C },
                { Keys.D,                   System.Windows.Forms.Keys.D },
                { Keys.E,                   System.Windows.Forms.Keys.E },
                { Keys.F,                   System.Windows.Forms.Keys.F },
                { Keys.G,                   System.Windows.Forms.Keys.G },
                { Keys.H,                   System.Windows.Forms.Keys.H },
                { Keys.I,                   System.Windows.Forms.Keys.I },
                { Keys.J,                   System.Windows.Forms.Keys.J },
                { Keys.K,                   System.Windows.Forms.Keys.K },
                { Keys.L,                   System.Windows.Forms.Keys.L },
                { Keys.M,                   System.Windows.Forms.Keys.M },
                { Keys.N,                   System.Windows.Forms.Keys.N },
                { Keys.O,                   System.Windows.Forms.Keys.O },
                { Keys.P,                   System.Windows.Forms.Keys.P },
                { Keys.Q,                   System.Windows.Forms.Keys.Q },
                { Keys.R,                   System.Windows.Forms.Keys.R },
                { Keys.S,                   System.Windows.Forms.Keys.S },
                { Keys.T,                   System.Windows.Forms.Keys.T },
                { Keys.U,                   System.Windows.Forms.Keys.U },
                { Keys.V,                   System.Windows.Forms.Keys.V },
                { Keys.W,                   System.Windows.Forms.Keys.W },
                { Keys.X,                   System.Windows.Forms.Keys.X },
                { Keys.Y,                   System.Windows.Forms.Keys.Y },
                { Keys.Z,                   System.Windows.Forms.Keys.Z },
                { Keys.Apps,                System.Windows.Forms.Keys.Apps },
                { Keys.Sleep,               System.Windows.Forms.Keys.Sleep },
                { Keys.NumPad0,             System.Windows.Forms.Keys.NumPad0 },
                { Keys.NumPad1,             System.Windows.Forms.Keys.NumPad1 },
                { Keys.NumPad2,             System.Windows.Forms.Keys.NumPad2 },
                { Keys.NumPad3,             System.Windows.Forms.Keys.NumPad3 },
                { Keys.NumPad4,             System.Windows.Forms.Keys.NumPad4 },
                { Keys.NumPad5,             System.Windows.Forms.Keys.NumPad5 },
                { Keys.NumPad6,             System.Windows.Forms.Keys.NumPad6 },
                { Keys.NumPad7,             System.Windows.Forms.Keys.NumPad7 },
                { Keys.NumPad8,             System.Windows.Forms.Keys.NumPad8 },
                { Keys.NumPad9,             System.Windows.Forms.Keys.NumPad9 },
                { Keys.Multiply,            System.Windows.Forms.Keys.Multiply },
                { Keys.Add,                 System.Windows.Forms.Keys.Add },
                { Keys.Separator,           System.Windows.Forms.Keys.Separator },
                { Keys.Subtract,            System.Windows.Forms.Keys.Subtract },
                { Keys.Decimal,             System.Windows.Forms.Keys.Decimal },
                { Keys.Divide,              System.Windows.Forms.Keys.Divide },
                { Keys.F1,                  System.Windows.Forms.Keys.F1 },
                { Keys.F2,                  System.Windows.Forms.Keys.F2 },
                { Keys.F3,                  System.Windows.Forms.Keys.F3 },
                { Keys.F4,                  System.Windows.Forms.Keys.F4 },
                { Keys.F5,                  System.Windows.Forms.Keys.F5 },
                { Keys.F6,                  System.Windows.Forms.Keys.F6 },
                { Keys.F7,                  System.Windows.Forms.Keys.F7 },
                { Keys.F8,                  System.Windows.Forms.Keys.F8 },
                { Keys.F9,                  System.Windows.Forms.Keys.F9 },
                { Keys.F10,                 System.Windows.Forms.Keys.F10 },
                { Keys.F11,                 System.Windows.Forms.Keys.F11 },
                { Keys.F12,                 System.Windows.Forms.Keys.F12 },
                { Keys.F13,                 System.Windows.Forms.Keys.F13 },
                { Keys.F14,                 System.Windows.Forms.Keys.F14 },
                { Keys.F15,                 System.Windows.Forms.Keys.F15 },
                { Keys.F16,                 System.Windows.Forms.Keys.F16 },
                { Keys.F17,                 System.Windows.Forms.Keys.F17 },
                { Keys.F18,                 System.Windows.Forms.Keys.F18 },
                { Keys.F19,                 System.Windows.Forms.Keys.F19 },
                { Keys.F20,                 System.Windows.Forms.Keys.F20 },
                { Keys.F21,                 System.Windows.Forms.Keys.F21 },
                { Keys.F22,                 System.Windows.Forms.Keys.F22 },
                { Keys.F23,                 System.Windows.Forms.Keys.F23 },
                { Keys.F24,                 System.Windows.Forms.Keys.F24 },
                { Keys.NumLock,             System.Windows.Forms.Keys.NumLock },
                { Keys.Scroll,              System.Windows.Forms.Keys.Scroll },
                { Keys.BrowserBack,         System.Windows.Forms.Keys.BrowserBack },
                { Keys.BrowserForward,      System.Windows.Forms.Keys.BrowserForward },
                { Keys.BrowserRefresh,      System.Windows.Forms.Keys.BrowserRefresh },
                { Keys.BrowserStop,         System.Windows.Forms.Keys.BrowserStop },
                { Keys.BrowserSearch,       System.Windows.Forms.Keys.BrowserSearch },
                { Keys.BrowserFavorites,    System.Windows.Forms.Keys.BrowserFavorites },
                { Keys.BrowserHome,         System.Windows.Forms.Keys.BrowserHome },
                { Keys.VolumeMute,          System.Windows.Forms.Keys.VolumeMute },
                { Keys.VolumeDown,          System.Windows.Forms.Keys.VolumeDown },
                { Keys.VolumeUp,            System.Windows.Forms.Keys.VolumeUp },
                { Keys.MediaNextTrack,      System.Windows.Forms.Keys.MediaNextTrack },
                { Keys.MediaPreviousTrack,  System.Windows.Forms.Keys.MediaPreviousTrack },
                { Keys.MediaStop,           System.Windows.Forms.Keys.MediaStop },
                { Keys.MediaPlayPause,      System.Windows.Forms.Keys.MediaPlayPause },
                { Keys.LaunchMail,          System.Windows.Forms.Keys.LaunchMail },
                { Keys.SelectMedia,         System.Windows.Forms.Keys.SelectMedia },
                { Keys.LaunchApplication1,  System.Windows.Forms.Keys.LaunchApplication1 },
                { Keys.LaunchApplication2,  System.Windows.Forms.Keys.LaunchApplication2 },
                { Keys.OemSemicolon,        System.Windows.Forms.Keys.OemSemicolon },
                { Keys.OemMinus,            System.Windows.Forms.Keys.OemMinus },
                { Keys.OemPeriod,           System.Windows.Forms.Keys.OemPeriod },
                { Keys.OemQuestion,         System.Windows.Forms.Keys.OemQuestion },
                { Keys.OemOpenBrackets,     System.Windows.Forms.Keys.OemOpenBrackets },
                { Keys.OemPipe,             System.Windows.Forms.Keys.OemPipe },
                { Keys.OemCloseBrackets,    System.Windows.Forms.Keys.OemCloseBrackets },
                { Keys.OemQuotes,           System.Windows.Forms.Keys.OemQuotes },
                { Keys.Oem8,                System.Windows.Forms.Keys.Oem8 },
                { Keys.OemBackslash,        System.Windows.Forms.Keys.OemBackslash },
                { Keys.ProcessKey,          System.Windows.Forms.Keys.ProcessKey },
                { Keys.Attn,                System.Windows.Forms.Keys.Attn },
                { Keys.Crsel,               System.Windows.Forms.Keys.Crsel },
                { Keys.Exsel,               System.Windows.Forms.Keys.Exsel },
                { Keys.EraseEof,            System.Windows.Forms.Keys.EraseEof },
                { Keys.Play,                System.Windows.Forms.Keys.Play },
                { Keys.Zoom,                System.Windows.Forms.Keys.Zoom },
                { Keys.Pa1,                 System.Windows.Forms.Keys.Pa1 },
                { Keys.OemClear,            System.Windows.Forms.Keys.OemClear },
            };
        }

        public static KeyEventArgs GetKeyEventArgs() {
            var count = 0;
            var keys = Keyboard.GetState().GetPressedKeys();

            var downKeys = System.Windows.Forms.Keys.None;

            foreach(var key in keys)
            {
                if( ++count == 0 )
                    downKeys = map[ key ];
                else
                    downKeys |= map[ key ];
            }

            return new KeyEventArgs( downKeys );
        }
    }
}
