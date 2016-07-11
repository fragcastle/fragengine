using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace FragEngine.Animation
{
    public class Animation
    {
        private float _timeForCurrentFrame;

        public Texture2D SpriteSheet { get; set; }

        public int[] Frames { get; set; }
        public bool Repeat { get; set; }
        public float FrameTime { get; set; }
        public int CurrentFrame { get; private set; }

        public Vector2 FrameSize { get; }

        public bool FlipX { get; set; }

        public int LoopCount { get; private set; }

        public float Scale { get; set; }

        public string Name
        {
            get; }

        public Animation( Vector2 frameSize, string name = null )
        {
            FrameSize = frameSize;

            Scale = 1.0f;

            Name = name;

            LoopCount = 0;
        }

        public void Update( GameTime time )
        {
            _timeForCurrentFrame += (float)time.ElapsedGameTime.TotalSeconds;

            if( _timeForCurrentFrame >= FrameTime )
            {
                CurrentFrame++;
                if( ( LoopCount < 1 && Repeat ) || CurrentFrame >= Frames.Length )
                {
                    LoopCount++;
                    CurrentFrame = Repeat ? 0 : Frames.Length - 1;
                }

                _timeForCurrentFrame = 0.0f;
            }
        }

        public void Draw( SpriteBatch batch, Vector2 position, float alpha = 255f )
        {
            var tintColor = Color.White * ( alpha / 255f );

            Draw( batch, position, tintColor );
        }

        public void Draw( SpriteBatch batch, Vector2 position, Color tintColor )
        {
            int offsetX = (int)FrameSize.X * Frames[ CurrentFrame ],
                offsetY = 0;

            while( offsetX >= SpriteSheet.Width )
            {
                // set the x to 0 and increase the height by one whole
                // to push the frame onto the next row
                offsetX = offsetX - SpriteSheet.Width;
                offsetY += (int)FrameSize.Y;
            }

            var animationFrame = new Rectangle( offsetX, offsetY, (int)FrameSize.X, (int)FrameSize.Y );

            var effect = FlipX ? SpriteEffects.FlipHorizontally : SpriteEffects.None;

            batch.Draw( SpriteSheet, position, animationFrame, tintColor, 0, Vector2.Zero, Scale, effect, 0.0f );

            FlipX = false;
        }

        public override string ToString()
        {
            return Name;
        }
    }
}
