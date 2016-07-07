using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using FragEngine.Collisions;
using FragEngine.Data;
using FragEngine.Debug;
using FragEngine.Layers;
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
        protected Hud _hud;

        protected Layer _hudLayer;
        private EntityLayer _entityLayer;

        private readonly IGameObjectService _gameObjectService = null;

        public PlayScreen( Hud hud, IGameObjectService gameObjectService = null, Camera camera = null )
        {
            _hud = hud;
            _camera = camera ?? ServiceLocator.Get<Camera>();
            _gameObjectService = gameObjectService ?? ServiceLocator.Get<IGameObjectService>();
            Decorations = new List<Decoration>();
        }

        public virtual void Initialize()
        {
            _entityLayer = new EntityLayer();
            _layers = new List<Layer>
            {
                _entityLayer
            };

            if( _hud != null )
            {
                _hudLayer = new StaticLayer() { StaticPosition = Vector2.Zero, DrawMethod = _hud.Draw };
                _layers.Add(_hudLayer);
            }

#if DEBUG
            Decorations.Add(new DebugDecoration());
#endif
        }

        public void LoadLevel( string levelName )
        {
            var path = Path.Combine( AppDomain.CurrentDomain.BaseDirectory, "Data\\" + levelName + ".json" );

            CurrentLevel = Level.Load( new FileInfo( path ) );

            _layers.InsertRange( 0, CurrentLevel.MapLayers );

#if DEBUG
            _layers.Add( CurrentLevel.CollisionLayer );
#endif

            _gameObjectService.AttachGameObjects(CurrentLevel.GameObjects.ToArray());

            // replace the collision service with one setup for this level
            var collisionMap = new CollisionMap( CurrentLevel );
            var collisionService = new CollisionService( collisionMap );

            ServiceLocator.Add<ICollisionService>( collisionService );
        }

        public List<Decoration> Decorations { get; set; }

        protected Level CurrentLevel { get; private set; }

        public override void LoadContent()
        {
            Initialize();

            // Create a new SpriteBatch, which can be used to draw textures.
            _spriteBatch = new SpriteBatch( ScreenManager.GraphicsDevice );


            if( _hud != null )
            {
                _gameFont = ContentCacheManager.GetSpriteFont( @"Fonts\GameFont" );
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

            if (Decorations.Count > 0)
            {
                Decorations.Sort((a, b) => a.ZIndex < b.ZIndex ? -1 : 1);
                Decorations.ForEach(d => d.Draw(gameTime));
            }   
        }

        public override void Update( GameTime gameTime, bool otherScreenHasFocus, bool coveredByOtherScreen )
        {
            if( _hud != null )
                _hud.Update( gameTime );

            // copy the GameObjects collection in case it is modified while we
            // are updating the objects
            // TODO: maybe don't update these objects inside the screen? This could be expensive if we have 10k objects
            foreach (var gameObject in _gameObjectService.GameObjects.ToList())
            {
                if (gameObject.IsAlive)
                {
                    gameObject.Update(gameTime);
                }
            }

            base.Update( gameTime, otherScreenHasFocus, coveredByOtherScreen );
        }
    }
}
