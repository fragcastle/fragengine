using System.Linq;
using FragEngine.Layers;
using FragEngine.Services;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace FragEngine.GameObjects
{
    public class GameObjectLayer : Layer
    {
        private readonly IGameObjectService _gameObjectService;

        public GameObjectLayer( Vector2? parallax = null, IGameObjectService gameObjectService = null)
            : base( parallax )
        {
            _gameObjectService = gameObjectService ?? ServiceLocator.Get<IGameObjectService>();
        }

        public override void CustomDraw( SpriteBatch spriteBatch )
        {
            var queues = _gameObjectService.DrawQueues.Keys.ToArray().OrderByDescending(i => i);
            foreach (var queue in queues)
            {
                var gameObjects = _gameObjectService.DrawQueues[queue];
                gameObjects.Where(go => go.IsAlive).ToList().ForEach(go => go.Draw(spriteBatch));
            }
        }
    }
}
