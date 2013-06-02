using System;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace FragEngine.View.Screens
{
    // shows annoying logos to people who just want to play the game
    public class FragEngineLogoScreen : LogoScreen
    {
        public FragEngineLogoScreen() : base (@"FragEngine.Resources.frag_castle_logo.png")
        {
            TransitionOnTime = TimeSpan.FromSeconds( 1.0 );
            TransitionOffTime = TimeSpan.FromSeconds( 1.0 );
        }
    }
}
