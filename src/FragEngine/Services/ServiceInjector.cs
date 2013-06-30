using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Linq;
using System.Text;

namespace FragEngine.Services
{
    // a really lazy implementation of the service locator pattern
    public static class ServiceInjector
    {
        private static readonly IDictionary<Type, object> _services = new Dictionary<Type, object>();

        public static void Add( Type type, object instance )
        {
            if( Has(type) )
                _services.Remove( type );

            _services.Add(type, instance);
        }

        public static void Add<T>(T instance)
        {
            Add( typeof(T), instance );
        }

        public static IServiceContainer Apply( IServiceContainer container = null )
        {
            container = container ?? new ServiceContainer();

            foreach (var service in _services)
                container.AddService(service.Key, service.Value);

            return container;
        }

        public static bool Has(Type type)
        {
            return _services.ContainsKey( type );
        }

        public static bool Has<T>()
        {
            return Has( typeof( T ) );
        }

        public static T Get<T>()
        {
            var item = default( T );
            if( _services.ContainsKey( typeof(T) ) )
                item = (T)_services[ typeof( T ) ];

            return item;
        }
    }
}
