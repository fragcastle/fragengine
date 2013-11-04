using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using Microsoft.Xna.Framework;

namespace FragEngine.Entities
{
    public class GameObjectSpawner : IGameObjectFactory
    {
        public GameObject Spawn<TGameobject>( Vector2 position, IDictionary<string, string> settings, GameObjectBuilder<TGameobject> builder ) 
            where TGameobject : GameObject, new()
        {
            return builder.Set( go => go.Position, position ).Set( go => go.Settings, settings ).Target;
        }
    }

    public interface IGameObjectFactory
    {
        GameObject Spawn<TGameobject>( Vector2 position, IDictionary<string, string> settings, GameObjectBuilder<TGameobject> builder ) 
            where TGameobject : GameObject, new();
    }

    public class GameObjectBuilder<TGameobject> 
        where TGameobject : GameObject, new()
    {

        private TGameobject _object;

        public GameObjectBuilder()
        {
            _object = new TGameobject();
        }

        public GameObjectBuilder<TGameobject> Set( Expression<Func<TGameobject, object>> memberLamda, object value )
        {
            var memberSelectorExpression = memberLamda.Body as MemberExpression;
            if( memberSelectorExpression != null )
            {
                var property = memberSelectorExpression.Member as PropertyInfo;
                if( property != null )
                {
                    property.SetValue( _object, value, null );
                }
            }
            return this;
        }

        public GameObject Target { get { return _object; } }
    }
}
