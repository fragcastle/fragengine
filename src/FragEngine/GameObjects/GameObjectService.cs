using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using FragEngine.Services;
using Microsoft.Xna.Framework;

namespace FragEngine.GameObjects
{
    public class GameObjectService : IGameObjectService
    {
        private readonly Dictionary<string, GameObject> _gameObjectNameIndex = new Dictionary<string, GameObject>();

        public GameObjectService()
        {
            GameObjects = new List<GameObject>();
            DrawQueues = new Dictionary<int, List<GameObject>>();
        }

        public List<GameObject> GameObjects { get; set; }
        public Dictionary<int, List<GameObject>> DrawQueues { get; set; }

        private List<GameObject> GetDrawQueue(int zIndex)
        {
            var idx = (int)Math.Floor(zIndex / 10f);

            if (!DrawQueues.ContainsKey(idx))
            {
                DrawQueues[idx] = new List<GameObject>();
            }

            return DrawQueues[idx];
        }

        private void InsertGameObjects(params GameObject[] gameObjects)
        {
            foreach (var gameObject in gameObjects)
            {
                GameObjects.Add(gameObject);

                if (!String.IsNullOrWhiteSpace(gameObject.Name))
                {
                    _gameObjectNameIndex[gameObject.Name] = gameObject;
                }

                var queue = GetDrawQueue(gameObject.ZIndex);
                queue.Add(gameObject);
            }
        }

        private void ApplySettings(GameObject gameObject, Dictionary<string, object> settings)
        {
            foreach (var pair in settings)
            {
                var propInfo = gameObject.GetType().GetProperty(pair.Key);
                if (propInfo.CanWrite && pair.Value != null)
                {
                    propInfo.SetValue(gameObject, pair.Value);
                }
            }
        }

        public TEntityType SpawnGameObject<TEntityType>(Vector2 position, object settings = null)
            where TEntityType : GameObject, new()
        {
            var gameObject = new TEntityType { Position = position };

            if (settings != null)
            {
                if (settings is Action<TEntityType>)
                {
                    (settings as Action<TEntityType>)?.Invoke(gameObject);
                }
                else
                {
                    var dictionary = new Dictionary<string, object>();
                    if (settings is Dictionary<string, object>)
                    {
                        dictionary = (Dictionary<string, object>)settings;
                    }
                    else
                    {
                        foreach (PropertyDescriptor descriptor in TypeDescriptor.GetProperties(settings))
                        {
                            var obj2 = descriptor.GetValue(settings);
                            dictionary.Add(descriptor.Name, obj2);
                        }
                    }
                    ApplySettings(gameObject, dictionary);
                }
            }

            InsertGameObjects(gameObject);

            return gameObject;
        }

        public void AttachGameObjects(params GameObject[] gameObjects)
        {
            InsertGameObjects(gameObjects);
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

        public void CleanUp()
        {
            var deadObjects = GameObjects.Where(go => !go.IsAlive);
            deadObjects.ToList().ForEach(RemoveGameObject);
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
