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
        TEntitytype SpawnGameObject<TEntitytype>( Vector2 position, Action<TEntitytype> configuration = null ) where TEntitytype : GameObject, new();

        List<GameObject> GameObjects { get; set; }

        Dictionary<int, List<GameObject>> DrawQueues { get; set; }

        GameObject GetGameObjectByName(string name);

        List<GameObject> GetGameObjectsByType(Type gameObjectType);

        void RemoveGameObject(GameObject go);
    }
}
