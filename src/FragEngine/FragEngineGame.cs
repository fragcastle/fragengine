using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FragEngine.Collisions;
using FragEngine.Debug;
using FragEngine.Entities;
using FragEngine.Services;
using FragEngine.View;
using FragEngine.View.Screens;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Graphics;
using Newtonsoft.Json;

namespace FragEngine
{
    public abstract class FragEngineGame : Game
    {

        /// <summary>
        /// Use in your games to get a close approximation of the Gravitational Constant
        /// </summary>
        public const float GRAVITY_CONSTANT = 613f;

        public static GraphicsDeviceManager Graphics;
        public static Matrix SpriteScale { get; protected set; }

        public static ScreenManager ScreenManager { get; private set; }

        public static DirectoryInfo DataDirectory { get; private set; }

        /// <summary>
        /// Set this to true to enable debug in the engine
        /// </summary>
        /// <remarks>
        /// This will cause the CollisionLayer for a level to be drawn
        /// </remarks>
        public static bool IsDebug { get; set; }

        /// <summary>
        /// Affects the size of sprites when they are rendered to the <see cref="GraphicsDevice"/>
        /// </summary>
        public static float Scale { get; set; }

        /// <summary>
        /// The gravitational constant of your game.
        /// </summary>
        public static float Gravity { get; set; }

        public static float Tick { get; private set; }

        /// <summary>
        /// Affects the passage of time in your game.
        /// </summary>
        public static float TimeScale { get; set; }

        private bool _isPauseKeyDown;

        private IServiceContainer _services;

        private readonly FpsCounter _frameCounter = new FpsCounter();

        static FragEngineGame()
        {
            Gravity = 0f; // default to no gravity

            Scale = 1.2f;

            TimeScale = 1f;
        }

        public FragEngineGame()
        {
            // TODO: figure out a way to change this to use the IGraphicsDeviceManager interface
            // FIXME: this is fucked. In DIRECTX versions of this code, we _must_ instantiate GraphicsDeviceManager
            // in the initialize, but in OPENGL versions we have to do it here (check the code in Game.cs)
            // it throws an exception if GraphicsDevice is null???? WHAT THE FUCK!?!?!?
            Graphics = new GraphicsDeviceManager( this );

            Graphics.CreateDevice();

            Content.RootDirectory = "Content";

            DataDirectory = new DirectoryInfo( Path.Combine( Directory.GetCurrentDirectory(), "Data" ) );

            ClearColor = Color.White;
        }

        public bool IsPaused { get; set; }

        public bool IsPausedForGuide { get; set; }

        protected Color ClearColor { get; set; }

        protected override void Initialize()
        {
            Graphics.PreferredBackBufferWidth = 1280;
            Graphics.PreferredBackBufferHeight = 720;

            ServiceLocator.Apply(Services);

            if (!ServiceLocator.Has<GraphicsDevice>())
                ServiceLocator.Add(Graphics.GraphicsDevice);

            if (!ServiceLocator.Has<Camera>())
                ServiceLocator.Add(new Camera(Graphics.GraphicsDevice.Viewport));

            if (!ServiceLocator.Has<IGameObjectService>())
                ServiceLocator.Add<IGameObjectService>(new GameObjectService());

            if (!ServiceLocator.Has<ICollisionService>())
                ServiceLocator.Add<ICollisionService>(new CollisionService());

#if !DEBUG
            Graphics.IsFullScreen = true;
            Graphics.PreferredBackBufferWidth = GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Width;
            Graphics.PreferredBackBufferHeight = GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Height;
            Graphics.PreferMultiSampling = false;
#else
            IsDebug = true;
#endif

            // TODO: GGGGGGGGAAAAAAAAAAAAAAAAAHHHHHHHHH WE'RE IO BOUND IN A CTOR!!!!!!!!!!!!!!!!!! FFFFFFFFFFFFFUUUUUUUUUUUUUUUUUUUUUUU
            // ContentCacheManager must be loaded first, this will scan
            // every directory in the content project and load all of the
            // content into a cache
            ContentCacheManager.LoadContent(Content);

            // TODO: in DIRECTX versions we'll have to do this in initialize...
            // maybe move this code there now?
            ScreenManager = new ScreenManager(this);
            Components.Add(ScreenManager);

            // set our resolution into the viewport, otherwise camera calculations will be fucked.
            GraphicsDevice.Viewport = new Viewport( 0, 0, Graphics.PreferredBackBufferWidth, Graphics.PreferredBackBufferHeight );

            SetSpriteScale();

            base.Initialize();
        }

        protected override void Update( GameTime gameTime )
        {
            Timer.Update(gameTime);

            // Allows the game to exit
            if ( GamePad.GetState( PlayerIndex.One ).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().GetPressedKeys().Contains( Keys.X ) )
                Exit();

            // Check to see if the user has paused or unpaused
            checkPauseKey( Keyboard.GetState(), GamePad.GetState( PlayerIndex.One ) );

            Tick = gameTime.GetGameTick();

            base.Update( AdjustGameTime( gameTime ) );
        }

        protected override void Draw( GameTime gameTime )
        {
            GraphicsDevice.Clear( ClearColor );

            if( IsPaused || IsPausedForGuide )
            {
                // shutdown all vibrations for the controllers
                // TODO: move this into GameInputManager
                GamePad.SetVibration( PlayerIndex.One, 0f, 0f );
                GamePad.SetVibration( PlayerIndex.Two, 0f, 0f );
                GamePad.SetVibration( PlayerIndex.Three, 0f, 0f );
                GamePad.SetVibration( PlayerIndex.Four, 0f, 0f );
            }

            base.Draw( AdjustGameTime( gameTime ) );
        }

        private GameTime AdjustGameTime( GameTime gameTime )
        {
            var msDelta = (float)gameTime.ElapsedGameTime.TotalMilliseconds;

            var adjustedMsDelta = msDelta * TimeScale;

            var totalTime = gameTime.TotalGameTime.TotalMilliseconds - msDelta + adjustedMsDelta;

            return new GameTime( new TimeSpan( 0, 0, 0, 0, (int)totalTime ), new TimeSpan( 0, 0, 0, 0, (int)adjustedMsDelta ), gameTime.IsRunningSlowly );
        }

        private void SetSpriteScale()
        {
            // Create the scale transform for Draw.
            // Do not scale the sprite depth (Z=1).
            SpriteScale = Matrix.CreateScale( Scale, Scale, 1 );
        }

        private void BeginPause( bool UserInitiated )
        {
            IsPaused = true;
            IsPausedForGuide = !UserInitiated;
            //TODO: Pause audio playback
            //TODO: Pause controller vibration
        }

        private void EndPause()
        {
            //TODO: Resume audio
            //TODO: Resume controller vibration
            IsPausedForGuide = false;
            IsPaused = false;
        }

        private void checkPauseKey( KeyboardState keyboardState, GamePadState gamePadState )
        {
            bool pauseKeyDownThisFrame = ( keyboardState.IsKeyDown( Keys.P ) || ( gamePadState.Buttons.Start == ButtonState.Pressed ) );
            // If key was not down before, but is down now, we toggle the
            // pause setting
            if( !_isPauseKeyDown && pauseKeyDownThisFrame )
            {
                if( !IsPaused )
                    BeginPause( true );
                else
                    EndPause();
            }
            _isPauseKeyDown = pauseKeyDownThisFrame;
        }
    }
}
