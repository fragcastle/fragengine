using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FragEngine.Animation;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace FragEngine.Entities.UI
{
    public class XboxButtonPrompt : EntityBase
    {
        private bool _isFading;

        public string Text { get; private set; }

        public float Scale { get; set; }

        public Color TextColor { get; set; }

        public SpriteFont Font { get { return FragEngineGame.ScreenManager.Font;  } }

        public int TextPaddingLeft { get; set; }

        public Buttons BoundButton { get; private set; }

        public XboxButtonPrompt( string text, Buttons boundButton )
        {
            Text = text;
            Scale = 1.0f;
            TextColor = Color.White;

            Animations = new AnimationSheet( @"FragEngine.Resources.ui-xbox-buttons.png", 32, 32 );

            Animations.Add( (int)Buttons.A, 1f, false, 0 );
            Animations.Add( (int)Buttons.B, 1f, false, 1 );
            Animations.Add( (int)Buttons.X, 1f, false, 2 );
            Animations.Add( (int)Buttons.Y, 1f, false, 3 );
            Animations.Add( (int)Buttons.DPadUp, 1f, false, 4 );
            Animations.Add( (int)Buttons.DPadRight, 1f, false, 5 );
            Animations.Add( (int)Buttons.DPadDown, 1f, false, 6 );
            Animations.Add( (int)Buttons.DPadLeft, 1f, false, 7 );
            Animations.Add( (int)Buttons.RightShoulder, 1f, false, 8 );
            Animations.Add( (int)Buttons.LeftShoulder, 1f, false, 9 );

            Animations.SetCurrentAnimation( (int)boundButton );

            BoundButton = boundButton;

            TextPaddingLeft = 10;
        }

        public override void Update( GameTime time )
        {
            base.Update( time );

            if (_isFading)
            {
                var delta = 0f;
                if (Alpha > 0)
                {
                    delta = (float)( 1 * time.ElapsedGameTime.TotalMilliseconds );
                    if (Alpha - delta < 0)
                        Alpha = 0f;
                    else
                        Alpha -= delta;
                }
            }
        }

        public override void Draw( Microsoft.Xna.Framework.Graphics.SpriteBatch batch )
        {
            base.Draw( batch );

            if (IsAlive)
            {
                Animations.CurrentAnimation.Scale = Scale;

                // draw the text
                var textDimensions = Font.MeasureString( Text );
                var buttonDimentions = Animations.CurrentAnimation.FrameSize * Scale;
                var delta = ( buttonDimentions.Y - textDimensions.Y ) / 2 * ( textDimensions.Y > buttonDimentions.Y ? -1 : 1 );
                var textPosition = new Vector2( Position.X + TextPaddingLeft + Animations.FrameSize.X, Position.Y + delta );
                batch.DrawString( Font, Text, textPosition, new Color( Alpha, Alpha, Alpha ) );
            }
        }

        public void FadeOut()
        {
            if (!_isFading)
            {
                _isFading = true;
            }
        }
    }
}
