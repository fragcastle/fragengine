using FragEngine.Services;
using FragEngine.View;
using FragEngine.View.Screens;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace FragEngine.Debug
{
    public class DebugDecoration : Decoration
    {
        private readonly FpsCounter _frameCounter = new FpsCounter();
        private readonly IGameObjectService _gameObjectService;
        private readonly SpriteBatch _spriteBatch;
        private readonly Font _font;

        public DebugDecoration(IGameObjectService gameObjectService = null)
        {
            _gameObjectService = gameObjectService ?? ServiceLocator.Get<IGameObjectService>();
            var graphicsDevice = ServiceLocator.Get<GraphicsDevice>();
            // Create a new SpriteBatch, which can be used to draw textures.
            _spriteBatch = new SpriteBatch(graphicsDevice);
            _font = ContentCacheManager.GetFont(@"FragEngine.Resources.font_04b03_white_16.png");
            Name = "FramePerSecond";
            ZIndex = 100;
        }

        public override void Draw(GameTime gameTime)
        {
            var camera = ServiceLocator.Get<Camera>();
            var deltaTime = (float)gameTime.ElapsedGameTime.TotalSeconds;

            _frameCounter.Update(deltaTime);

            var numberOfObjects = _gameObjectService.GameObjects.Count;

            var fps = string.Format("FPS: {0}\nObjects: {1}\nShake Level:{2}\nShake Amount:{3}", 
                _frameCounter.AverageFramesPerSecond, 
                numberOfObjects,
                camera.ShakeLevel,
                camera.ShakeAmount
                );

            _font.Draw(_spriteBatch, fps, Vector2.One);
        }
    }
}
