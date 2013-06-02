using System.Collections.Generic;
using System.Linq;
using FragEngine.Layers;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Audio;
using FragEngine.Entities;
using FragEngine.Entities.Weapons;

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
            _camera = FragEngineGame.ScreenManager.Camera;
        }

        public virtual void Initialize()
        {
            _playerLayer = new EntityLayer( _camera );
            _hudLayer = new StaticLayer( _camera ) { StaticPosition = Vector2.Zero, DrawMethod = _hud.Draw };
            _entityLayer = new EntityLayer( _camera ) { DrawMethod = WeaponManager.Draw };
            _layers = new List<Layer>
            {
                _entityLayer,
                _playerLayer,
                _hudLayer
            };

            // TODO: move this into Hud.LoadContent()????
            ScreenManager.Game.Window.ClientSizeChanged += _hud.ClientSizeChanged;
            ScreenManager.Game.Window.ClientSizeChanged += _camera.ClientSizeChanged;
        }

        public override void LoadContent()
        {
            Initialize();

            // Create a new SpriteBatch, which can be used to draw textures.
            _spriteBatch = new SpriteBatch( ScreenManager.GraphicsDevice );

            _gameFont = ContentCacheManager.GetFont( @"Fonts\GameFont" );

            _camera.Target = _hud.Target = PlayerManager.PlayerOne;

            // TODO: move this into update?!?!?
            _playerLayer.Entities.AddRange( PlayerManager.Players.Values.Cast<EntityBase>() );

            _hud.Font = _gameFont;

            base.LoadContent();
        }

        public override void Draw( Microsoft.Xna.Framework.GameTime gameTime )
        {
            foreach (Layer layer in _layers)
            {
                layer.Alpha = TransitionAlpha;
                layer.Draw( _spriteBatch );
            }
        }

        public override void Update( GameTime gameTime, bool otherScreenHasFocus, bool coveredByOtherScreen )
        {
            _hud.Update( gameTime );

            PlayerManager.Update( gameTime );

            WeaponManager.Update( gameTime );

            foreach (var entity in _entityLayer.Entities)
            {
                if( entity as WeaponBase == null )
                {
                    entity.Update( gameTime );
                }
            }

            base.Update( gameTime, otherScreenHasFocus, coveredByOtherScreen );
        }
    }
}
