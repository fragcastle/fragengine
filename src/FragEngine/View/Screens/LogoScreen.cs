using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace FragEngine.View.Screens
{
    // shows annoying logos to people who just want to play the game
    public class LogoScreen : GameScreenBase
    {

        //How long should the screen stay fully visible
        const float timeToStayOnScreen = 1.5f;

        //Keep track of how much time has passed
        float timer = 0f;

        private Texture2D _logoTexture;
        private readonly string _texturePath;

        public LogoScreen(  string logoFilePath )
        {
            TransitionOnTime = TimeSpan.FromSeconds(1.0);
            TransitionOffTime = TimeSpan.FromSeconds(1.0);

            _texturePath = logoFilePath;
        }

        public override void LoadContent()
        {
            _logoTexture = ContentCacheManager.GetTexture( _texturePath );

            base.LoadContent();
        }

        public override void Update(Microsoft.Xna.Framework.GameTime gameTime, bool otherScreenHasFocus, bool coveredByOtherScreen)
        {
            base.Update( gameTime, otherScreenHasFocus, coveredByOtherScreen );

            var keyState = Keyboard.GetState();
            var gamePadState = GamePad.GetState(PlayerIndex.One);

            if( keyState.GetPressedKeys().Length > 0 ||
                gamePadState.IsButtonDown( Buttons.Start ) ||
                gamePadState.IsButtonDown( Buttons.A ) ||
                gamePadState.IsButtonDown( Buttons.B ) ||
                gamePadState.IsButtonDown( Buttons.X ) ||
                gamePadState.IsButtonDown( Buttons.Y ) )
            {
                ExitScreen();
            }

            if ( ScreenState == ScreenState.Active )
            {
                //When this screen is fully active, we want to
                //begin our timer so we know when to fade out
                float elapsed = (float)gameTime.ElapsedGameTime.TotalSeconds;
                timer += elapsed;
                if ( timer >= timeToStayOnScreen )
                {
                    //When we've passed the 'timeToStayOnScreen' time,
                    //we call ExitScreen() which will fade out then
                    //kill the screen afterwards
                    ExitScreen();
                }
            }
            else if ( ScreenState == ScreenState.TransitionOff )
            {
                if ( TransitionPosition == 1 )
                {
                    //When 'TransistionPosition' hits 1 then our screen
                    //is fully faded out. Anything in this block of
                    //code is the last thing to be called before this
                    //screen is killed forever so we add the next screen(s)
                    ScreenManager.AddScreen( NextScreen() );
                }
            }
        }

        public override void Draw(Microsoft.Xna.Framework.GameTime gameTime)
        {
            SpriteBatch spriteBatch = ScreenManager.SpriteBatch;
            Viewport viewport = ScreenManager.GraphicsDevice.Viewport;
            Rectangle fullscreen = new Rectangle(0, 0, viewport.Width, viewport.Height);
            Vector2 center = new Vector2(fullscreen.Center.X, fullscreen.Center.Y);
            spriteBatch.Begin();
            //Draw our logo centered to the screen
            spriteBatch.Draw(_logoTexture,
                center,
                null,
                new Color(TransitionAlpha, TransitionAlpha, TransitionAlpha),
                0f,
                new Vector2(_logoTexture.Width / 2, _logoTexture.Height / 2),
                1f,
                SpriteEffects.None,
                0f);
            spriteBatch.End();
        }
    }
}
