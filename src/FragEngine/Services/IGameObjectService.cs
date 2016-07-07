using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FragEngine.Entities;
using Microsoft.Xna.Framework;

namespace FragEngine.Services
{
    public interface IGameObjectService
    {
        TEntityType SpawnGameObject<TEntityType>( Vector2 position, Action<TEntityType> configuration = null ) where TEntityType : GameObject, new();
        TEntityType SpawnGameObject<TEntityType>(Vector2 position, Dictionary<string, object> settings) where TEntityType : GameObject, new();
        TEntityType SpawnGameObject<TEntityType>(Vector2 position, object settings) where TEntityType : GameObject, new();

        List<GameObject> GameObjects { get; set; }

        Dictionary<int, List<GameObject>> DrawQueues { get; set; }

        GameObject GetGameObjectByName(string name);

        List<GameObject> GetGameObjectsByType(Type gameObjectType);

        void AttachGameObjects(params GameObject[] gameObjects);

        void RemoveGameObject(GameObject go);
    }
}
