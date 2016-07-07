using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FragEngine.Services;
using FragEngine.View.Screens;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace FragEngine.Debug
{
    public class DebugDecoration : Decoration
    {
        private readonly FpsCounter _frameCounter = new FpsCounter();
        private readonly IGameObjectService _gameObjectService;
        private SpriteBatch _spriteBatch;
        private Font _font;

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
            var deltaTime = (float)gameTime.ElapsedGameTime.TotalSeconds;

            _frameCounter.Update(deltaTime);

            var numberOfObjects = _gameObjectService.GameObjects.Count;

            var fps = string.Format("FPS: {0}\nObjects: {1}", _frameCounter.AverageFramesPerSecond, numberOfObjects);

            _font.Draw(_spriteBatch, fps, Vector2.One);
        }
    }
}
