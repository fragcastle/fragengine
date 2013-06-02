using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace FragEngine.View.Screens
{
    /// <summary>
    /// Base public class for screens that contain a menu of options. The user can
    /// move up and down to select an entry, or cancel to back out of the screen.
    /// </summary>
    /// <remarks>
    /// This public class is similar to one in the GameStateManagement sample.
    /// </remarks>
    public abstract class MenuScreenBase : GameScreenBase
    {
        private List<string> _menuEntries = new List<string>();
        private int _selectedEntry = 0;

        protected Texture2D _titleTexture;

        /// <summary>
        /// Gets the list of menu entry strings, so derived classes can add
        /// or change the menu contents.
        /// </summary>
        protected IList<string> MenuEntries
        {
            get { return _menuEntries; }
        }

        /// <summary>
        /// Constructor.
        /// </summary>
        protected MenuScreenBase()
        {
            TransitionOnTime = TimeSpan.FromSeconds( 1.0 );
            TransitionOffTime = TimeSpan.FromSeconds( 1.0 );
        }

        /// <summary>
        /// Responds to user input, changing the selected entry and accepting
        /// or cancelling the menu.
        /// </summary>
        public override void HandleInput( InputState input )
        {
            // Move to the previous menu entry?
            if ( input.MenuUp )
            {
                _selectedEntry--;

                if ( _selectedEntry < 0 )
                    _selectedEntry = _menuEntries.Count - 1;

                // AudioManager.PlaySound( "menu_scroll" );
            }

            // Move to the next menu entry?
            if ( input.MenuDown )
            {
                _selectedEntry++;

                if ( _selectedEntry >= _menuEntries.Count )
                    _selectedEntry = 0;

                // AudioManager.PlaySound( "menu_scroll" );
            }

            // Accept or cancel the menu?
            if ( input.MenuSelect )
            {
                // AudioManager.PlaySound( "menu_select" );
                OnSelectEntry( _selectedEntry );
            }
            else if ( input.MenuCancel )
            {
                OnCancel();
            }
        }


        /// <summary>
        /// Notifies derived classes that a menu entry has been chosen.
        /// </summary>
        protected abstract void OnSelectEntry( int entryIndex );

        /// <summary>
        /// Notifies derived classes that the menu has been cancelled.
        /// </summary>
        protected abstract void OnCancel();

        /// <summary>
        /// Draws the menu.
        /// </summary>
        public override void Draw( GameTime gameTime )
        {
            Viewport viewport = ScreenManager.GraphicsDevice.Viewport;
            Vector2 viewportSize = new Vector2( viewport.Width, viewport.Height );

            Vector2 position = new Vector2( 0f, viewportSize.Y * 0.65f );

            // Make the menu slide into place during transitions, using a
            // power curve to make things look more interesting (this makes
            // the movement slow down as it nears the end).
            float transitionOffset = (float)Math.Pow( TransitionPosition, 2 );

            if ( ScreenState == ScreenState.TransitionOn )
                position.Y += transitionOffset * 256;
            else
                position.Y += transitionOffset * 512;

            // Draw each menu entry in turn.
            ScreenManager.SpriteBatch.Begin();

            for ( int i = 0; i < _menuEntries.Count; i++ )
            {
                Color color = Color.White;
                float scale = 1.0f;

                if ( IsActive && ( i == _selectedEntry ) )
                {
                    // The selected entry is yellow, and has an animating size.
                    double time = gameTime.TotalGameTime.TotalSeconds;
                    float pulsate = (float)Math.Sin( time * 6f ) + 1f;

                    color = Color.Orange;
                    scale += pulsate * 0.05f;
                }

                // Modify the alpha to fade text out during transitions.
                color = new Color( color.R, color.G, color.B, TransitionAlpha );

                // Draw text, centered on the middle of each line.
                Vector2 origin = new Vector2( 0, ScreenManager.Font.LineSpacing / 2 );
                Vector2 size = ScreenManager.Font.MeasureString( _menuEntries[i] );
                position.X = viewportSize.X / 2f - size.X / 2f * scale;
                ScreenManager.SpriteBatch.DrawString( ScreenManager.Font, _menuEntries[i],
                                                     position, color, 0, origin, scale,
                                                     SpriteEffects.None, 1 );

                position.Y += ScreenManager.Font.LineSpacing;
            }
            ScreenManager.SpriteBatch.End();

            ScreenManager.SpriteBatch.Begin();

            Vector2 titlePosition = new Vector2(
                                        ScreenManager.TitleSafeArea.X +
                                        ( ScreenManager.TitleSafeArea.Width - _titleTexture.Width ) / 2f,
                                        ScreenManager.TitleSafeArea.Y +
                                        ScreenManager.TitleSafeArea.Height * 0.05f );

            ScreenManager.SpriteBatch.Draw( _titleTexture, new Rectangle( 0, 0, viewport.Width, viewport.Height ), new Color( 255, 255, 255, 0 ) );

            ScreenManager.SpriteBatch.End();
        }
    }
}
