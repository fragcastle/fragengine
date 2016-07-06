using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FragEngine.Entities;
using FragEngine.Services;
using FragEngine.View;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace FragEngine.Layers
{
    // EntityLayer is never serialized. It's only used by the game engine
    public class EntityLayer : Layer
    {
        private IGameObjectService _gameObjectService;

        public EntityLayer( Vector2? parallax = null, IGameObjectService gameObjectService = null)
            : base( parallax )
        {
            DrawMethod = DrawEntities;

            _gameObjectService = gameObjectService ?? ServiceLocator.Get<IGameObjectService>();
        }

        public void DrawEntities( SpriteBatch spriteBatch )
        {
            var queues = _gameObjectService.DrawQueues.Keys.ToArray().OrderByDescending(i => i);
            foreach (var queue in queues)
            {
                var gameObjects = _gameObjectService.DrawQueues[queue];
                gameObjects.ForEach(go => go.Draw(spriteBatch));
            }
        }
    }
}
