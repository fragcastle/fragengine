using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;

namespace FragEngine.Animation {
    public class Animation {
        private float _timeForCurrentFrame;

        public Texture2D SpriteSheet { get; set; }

        public int[] Frames { get; set; }
        public bool Repeat { get; set; }
        public float FrameTime { get; set; }
        public int CurrentFrame { get; private set; }

        public Vector2 FrameSize { get; private set; }

        public bool FlipX { get; set; }

        public float Scale { get; set; }

        public string Name {
            get;
            private set;
        }

        public Animation( Vector2 frameSize, string name = null ) {
            FrameSize = frameSize;

            Scale = 1.0f;

            Name = name;
        }

        public void Update( GameTime time ) {
            _timeForCurrentFrame += (float)time.ElapsedGameTime.TotalSeconds;

            if( _timeForCurrentFrame >= FrameTime ) {
                if( ++CurrentFrame >= Frames.Length ) {
                    CurrentFrame = 0;
                }
                _timeForCurrentFrame = 0.0f;
            }
        }

        public void Draw( SpriteBatch batch, Vector2 position, float alpha = 255f ) {
            var tintColor = Color.White * ( alpha / 255f );

            Draw( batch, position, tintColor );
        }

        public void Draw( SpriteBatch batch, Vector2 position, Color tintColor ) {
            int offsetX = (int)FrameSize.X * Frames[ CurrentFrame ],
                offsetY = 0;

            while( offsetX >= SpriteSheet.Width ) {
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

        public override string ToString() {
            return Name;
        }
    }

    public class AnimationSheet {
        private Animation _currentAnimation;
        private string _currentAnimationKey;
        private Dictionary<string, Animation> _animations;

        private string _texturePath;

        public Animation this[ string index ] {
            get {
                return _animations[ index ];
            }
        }

        public Animation this[ int index ] {
            get {
                return _animations[ index.ToString() ];
            }
        }

        protected AnimationSheet( int frameWidth, int frameHeight ) {
            FrameSize = new Vector2( frameWidth, frameHeight );
            _animations = new Dictionary<string, Animation>();
        }

        public AnimationSheet( string texturePath, int frameWidth, int frameHeight )
            : this( frameWidth, frameHeight ) {
            _texturePath = texturePath;
            SpriteSheet = ContentCacheManager.GetTexture( texturePath );
        }

        public AnimationSheet( Texture2D spriteSheet, int frameWidth, int frameHeight )
            : this( frameWidth, frameHeight ) {
            SpriteSheet = spriteSheet;
        }

        public Texture2D SpriteSheet { get; private set; }

        public Vector2 FrameSize { get; private set; }

        public int TotalFrames {
            get {
                var rows = SpriteSheet.Height / FrameSize.Y;
                var cols = SpriteSheet.Width / FrameSize.X;
                return (int)( rows * cols );
            }
        }

        public Animation CurrentAnimation {
            get {
                return _currentAnimation;
            }
        }

        public List<Animation> GetAnimations() {
            return _animations.Select( entry => entry.Value ).ToList();
        }

        public void Previous() {
            var keys = _animations.Keys.ToArray();

            int currentIndex = 0;
            for( int i = 0; i < keys.Length; i++ ) {
                if( keys[ i ] == _currentAnimationKey ) {
                    currentIndex = i;
                    break;
                }
            }

            if( currentIndex == 0 )
                currentIndex = keys.Length;

            var anim = _animations.ElementAt( --currentIndex );
            _currentAnimationKey = anim.Key;
            _currentAnimation = anim.Value;
        }

        public void Next() {
            var keys = _animations.Keys.ToArray();

            int currentIndex = 0;
            for( int i = 0; i < keys.Length; i++ ) {
                if( keys[ i ] == _currentAnimationKey ) {
                    currentIndex = i;
                    break;
                }
            }

            if( currentIndex >= ( keys.Length - 1 ) )
                currentIndex = -1;

            var anim = _animations.ElementAt( ++currentIndex );
            _currentAnimationKey = anim.Key;
            _currentAnimation = anim.Value;
        }

        public void SetCurrentAnimation( string name ) {
            _currentAnimationKey = name;
            _currentAnimation = _animations[ name ];
        }

        public void SetCurrentAnimation( int id ) {
            _currentAnimationKey = id.ToString();
            _currentAnimation = _animations[ id.ToString() ];
        }

        public bool HasAnimation( string name ) {
            return _animations.ContainsKey( name );
        }

        public bool HasAnimation( int id ) {
            return _animations.ContainsKey( id.ToString() );
        }

        public void Add( int id, float frameTime, bool repeat, params int[] frames ) {
            Add( id.ToString(), frameTime, repeat, frames );
        }

        public void Add( string name, float frameTime, bool repeat, params int[] frames ) {
            var animation = new Animation( FrameSize, name ) {
                FrameTime = frameTime,
                Frames = frames,
                Repeat = repeat,
                SpriteSheet = SpriteSheet
            };

            if( _currentAnimation == null || name == "idle"  )
            {
                _currentAnimation = animation;
            }
            
            _animations.Add( name,  animation);
        }
    }
}
