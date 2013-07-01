using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using FragEngine.Data;
using FragEngine.Layers;
using FragEngine.Mapping;
using FragEngine.Services;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using FragEngine.Entities;

namespace FragEngine.View.Screens
{
    public class PlayScreen : GameScreenBase
    {
        protected SpriteBatch _spriteBatch;

        protected SpriteFont _gameFont;

        protected Camera _camera;
        protected List<Layer> _layers;
        protected HudBase _hud;

        protected EntityLayer _playerLayer;
        protected Layer _hudLayer;
        private EntityLayer _entityLayer;

        public PlayScreen( HudBase hud )
        {
            _hud = hud;
            _camera = ServiceInjector.Get<Camera>();
        }

        public virtual void Initialize()
        {
            _playerLayer = new EntityLayer( _camera );
            _entityLayer = new EntityLayer( _camera );
            _layers = new List<Layer>
            {
                _entityLayer,
                _playerLayer
            };

            if( _hud != null )
            {
                _hudLayer = new StaticLayer( _camera ) { StaticPosition = Vector2.Zero, DrawMethod = _hud.Draw };
                _layers.Add(_hudLayer);
            }
        }

        public void LoadLevel( string levelName )
        {
            var path = Path.Combine( AppDomain.CurrentDomain.BaseDirectory, "Data\\" + levelName + ".json" );

            CurrentLevel = Level.Load( new FileInfo( path ) );

            var players = CurrentLevel.Entities.Where( e => e is Player );

            _playerLayer.Entities.AddRange( players );

            _layers.InsertRange( 0, CurrentLevel.MapLayers );

#if DEBUG
            _layers.Add( CurrentLevel.CollisionLayer );
#endif

            // replace the collision service with one setup for this level
            var collisionMap = new CollisionMap( CurrentLevel );
            var collisionService = new CollisionService( collisionMap );

            ServiceInjector.Add<ICollisionService>( collisionService );
        }

        protected Level CurrentLevel { get; private set; }

        public override void LoadContent()
        {
            Initialize();

            // Create a new SpriteBatch, which can be used to draw textures.
            _spriteBatch = new SpriteBatch( ScreenManager.GraphicsDevice );


            if( _hud != null )
            {
                _gameFont = ContentCacheManager.GetFont( @"Fonts\GameFont" );
                _hud.Font = _gameFont;
            }


            base.LoadContent();
        }

        public override void Draw( GameTime gameTime )
        {
            foreach (Layer layer in _layers)
            {
                layer.Alpha = TransitionAlpha;
                layer.Draw( _spriteBatch );
            }
        }

        public override void Update( GameTime gameTime, bool otherScreenHasFocus, bool coveredByOtherScreen )
        {
            if( _hud != null )
                _hud.Update( gameTime );

            foreach (var entity in _entityLayer.Entities)
            {
                entity.Update( gameTime );
            }

            foreach( var entity in _playerLayer.Entities )
            {
                entity.Update( gameTime );
            }

            base.Update( gameTime, otherScreenHasFocus, coveredByOtherScreen );
        }
    }
}
