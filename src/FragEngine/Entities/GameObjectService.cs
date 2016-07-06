using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FragEngine.Services;
using Microsoft.Xna.Framework;

namespace FragEngine.Entities
{
    public class GameObjectService : IGameObjectService
    {
        private Dictionary<string, GameObject> _gameObjectNameIndex = new Dictionary<string, GameObject>();

        public GameObjectService()
        {
            GameObjects = new List<GameObject>();
            DrawQueues = new Dictionary<int, List<GameObject>>();
        }

        public List<GameObject> GameObjects { get; set; }
        public Dictionary<int, List<GameObject>> DrawQueues { get; set; }

        private List<GameObject> GetDrawQueue(int zIndex)
        {
            var idx = (int)Math.Floor(zIndex/10f);

            if (!DrawQueues.ContainsKey(idx))
            {
                DrawQueues[idx] = new List<GameObject>();
            }

            return DrawQueues[idx];
        } 

        public TEntitytype SpawnGameObject<TEntitytype>( Vector2 position, Action<TEntitytype> configuration = null ) where TEntitytype : GameObject, new()
        {
            var entity = new TEntitytype {Position = position};

            configuration?.Invoke( entity );

            GameObjects.Add(entity);

            if (!String.IsNullOrWhiteSpace(entity.Name))
            {
                _gameObjectNameIndex[entity.Name] = entity;
            }

            var queue = GetDrawQueue(entity.ZIndex);
            queue.Add(entity);

            return entity;
        }

        public void RemoveGameObject(GameObject go)
        {
            if (!String.IsNullOrWhiteSpace(go.Name))
            {
                _gameObjectNameIndex.Remove(go.Name);
            }

            var queue = GetDrawQueue(go.ZIndex);
            queue.Remove(go);

            GameObjects.Remove(go);

            go = null;
        }

        public GameObject GetGameObjectByName(string name)
        {
            return _gameObjectNameIndex.ContainsKey(name) ? _gameObjectNameIndex[name] : null;
        }

        public List<GameObject> GetGameObjectsByType(Type gameObjectType)
        {
            return GameObjects.FindAll(go => go.GetType() == gameObjectType);
        }
    }
}
