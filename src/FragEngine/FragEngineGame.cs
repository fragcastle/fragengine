using System;
using System.ComponentModel.Design;
using System.Linq;
using FragEngine.Collisions;
using FragEngine.Content;
using FragEngine.Debug;
using FragEngine.GameObjects;
using FragEngine.Maps.Tiled;
using FragEngine.Services;
using FragEngine.View;
using FragEngine.View.Screens;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace FragEngine
{
    public abstract class FragEngineGame : Game
    {

        /// <summary>
        /// Use in your games to get a close approximation of the Gravitational Constant
        /// </summary>
        public const float GRAVITY_CONSTANT = 613f;

        public static GraphicsDeviceManager Graphics;

        public static ScreenManager ScreenManager { get; private set; }

        /// <summary>
        /// Set this to true to enable debug in the engine
        /// </summary>
        /// <remarks>
        /// This will cause the CollisionLayer for a level to be drawn
        /// </remarks>
        public static bool IsDebug { get; set; }

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
            TimeScale = 1f;
        }

        public FragEngineGame()
        {
            Graphics = new GraphicsDeviceManager(this);
            Graphics.CreateDevice();
            Content.RootDirectory = "Content";
            ClearColor = Color.White;
        }

        public bool IsPaused { get; set; }

        public bool IsPausedForGuide { get; set; }

        protected Color ClearColor { get; set; }

        protected override void Initialize()
        {
#if !DEBUG
            Graphics.IsFullScreen = true;
            Graphics.PreferredBackBufferWidth = GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Width;
            Graphics.PreferredBackBufferHeight = GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Height;
            Graphics.PreferMultiSampling = false;
#else
            Graphics.PreferredBackBufferWidth = 1280;
            Graphics.PreferredBackBufferHeight = 720;
            IsDebug = true;
#endif

            Graphics.ApplyChanges();

            SetupServices();

            // set our resolution into the viewport, otherwise camera calculations will be fucked.
            GraphicsDevice.Viewport = new Viewport( 0, 0, Graphics.PreferredBackBufferWidth, Graphics.PreferredBackBufferHeight );

            base.Initialize();
            
            // TODO: in DIRECTX versions we'll have to do this in initialize...
            // maybe move this code there now?
            ScreenManager = new ScreenManager(this);
            Components.Add(ScreenManager);
        }

        protected virtual void SetupServices()
        {
            ServiceLocator.Apply(Services);

            var contentCache = new ContentCache();
            var contentCacheLoader = new ContentCacheLoader(contentCache, Content);

            if (!ServiceLocator.Has<IReadableContentCache>())
                ServiceLocator.Add<IReadableContentCache>(contentCache);
            if (!ServiceLocator.Has<IWriteableContentCache>())
                ServiceLocator.Add<IWriteableContentCache>(contentCache);
            if (!ServiceLocator.Has<ContentCacheLoader>())
                ServiceLocator.Add(contentCacheLoader);

            if (!ServiceLocator.Has<GraphicsDevice>())
                ServiceLocator.Add(Graphics.GraphicsDevice);

            if (!ServiceLocator.Has<Camera>())
                ServiceLocator.Add(new Camera(Graphics.GraphicsDevice.Viewport));

            if (!ServiceLocator.Has<IGameObjectService>())
                ServiceLocator.Add<IGameObjectService>(new GameObjectService());

            if (!ServiceLocator.Has<ICollisionService>())
                ServiceLocator.Add<ICollisionService>(new CollisionService());
        }

        protected override void LoadContent()
        {
            var loader = ServiceLocator.Get<ContentCacheLoader>();
            loader.RegisterImporter(new FontImporter());
            loader.RegisterImporter(new TextureImporter());
            loader.RegisterImporter(new TiledMapImporter());
            loader.LoadContent();
            base.LoadContent();
        }

        protected override void Update( GameTime gameTime )
        {
            // update the game timers
            Timer.Update(gameTime);

            // Allows the game to exit
            if ( GamePad.GetState( PlayerIndex.One ).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().GetPressedKeys().Contains( Keys.X ) )
                Exit();

            // Check to see if the user has paused or unpaused
            CheckPauseKey( Keyboard.GetState(), GamePad.GetState( PlayerIndex.One ) );

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

        private void CheckPauseKey( KeyboardState keyboardState, GamePadState gamePadState )
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
