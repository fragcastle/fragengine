using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;

namespace FragEngine.Animation
{
    public class AnimationSheet
    {
        private string _currentAnimationKey;
        private Dictionary<string, Animation> _animations;

        private string _texturePath;

        public Animation this[ string index ]
        {
            get
            {
                return _animations[ index ];
            }
        }

        public Animation this[ int index ]
        {
            get
            {
                return _animations[ index.ToString() ];
            }
        }

        protected AnimationSheet( int frameWidth, int frameHeight )
        {
            FrameSize = new Vector2( frameWidth, frameHeight );
            _animations = new Dictionary<string, Animation>();
        }

        public AnimationSheet( string texturePath, int frameWidth, int frameHeight )
            : this( frameWidth, frameHeight )
        {
            _texturePath = texturePath;
            SpriteSheet = ContentCacheManager.GetTexture( texturePath );
        }

        public AnimationSheet( Texture2D spriteSheet, int frameWidth, int frameHeight )
            : this( frameWidth, frameHeight )
        {
            SpriteSheet = spriteSheet;
        }

        public Texture2D SpriteSheet { get; private set; }

        public Vector2 FrameSize { get; private set; }

        public int TotalFrames
        {
            get
            {
                var rows = SpriteSheet.Height / FrameSize.Y;
                var cols = SpriteSheet.Width / FrameSize.X;
                return (int)( rows * cols );
            }
        }

        public Animation CurrentAnimation
        {
            get
            {
                return _animations[ _currentAnimationKey ];
            }
        }

        public List<Animation> GetAnimations()
        {
            return _animations.Select( entry => entry.Value ).ToList();
        }

        public void Previous()
        {
            var keys = _animations.Keys.ToArray();

            int currentIndex = 0;
            for( int i = 0; i < keys.Length; i++ )
            {
                if( keys[ i ] == _currentAnimationKey )
                {
                    currentIndex = i;
                    break;
                }
            }

            if( currentIndex == 0 )
                currentIndex = keys.Length;

            var anim = _animations.ElementAt( --currentIndex );
            _currentAnimationKey = anim.Key;
        }

        public void Next()
        {
            var keys = _animations.Keys.ToArray();

            int currentIndex = 0;
            for( int i = 0; i < keys.Length; i++ )
            {
                if( keys[ i ] == _currentAnimationKey )
                {
                    currentIndex = i;
                    break;
                }
            }

            if( currentIndex >= ( keys.Length - 1 ) )
                currentIndex = -1;

            var anim = _animations.ElementAt( ++currentIndex );
            _currentAnimationKey = anim.Key;
        }

        public void SetCurrentAnimation( string name )
        {
            if( _currentAnimationKey != name )
                _currentAnimationKey = name;
        }

        public void SetCurrentAnimation( int id )
        {
            SetCurrentAnimation( id.ToString() );
        }

        public bool HasAnimation( string name )
        {
            return _animations.ContainsKey( name );
        }

        public bool HasAnimation( int id )
        {
            return _animations.ContainsKey( id.ToString() );
        }

        public void Add( int id, float frameTime, bool repeat, params int[] frames )
        {
            Add( id.ToString(), frameTime, repeat, frames );
        }

        public void Add( string name, float frameTime, bool repeat, params int[] frames )
        {
            var animation = new Animation( FrameSize, name ) {
                FrameTime = frameTime,
                Frames = frames,
                Repeat = repeat,
                SpriteSheet = SpriteSheet
            };

            if( frames.Length == 1 )
                animation.Repeat = false; // no need to repeat if there is one frame

            if( _currentAnimationKey == null || name == "idle" )
                _currentAnimationKey = name;

            _animations.Add( name, animation );
        }
    }
}
