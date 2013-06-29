using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Linq;
using System.Text;

namespace FragEngine.Services
{
    public static class ServiceInjector
    {
        private static readonly IDictionary<Type, object> _services;

        static ServiceInjector()
        {
            _services = new Dictionary<Type, object>();
        }

        public static void Add( Type type, object instance )
        {
            _services.Add(type, instance);
        }

        public static void Add<T>(T instance)
        {
            _services.Add( typeof(T), instance );
        }

        public static IServiceContainer Apply( IServiceContainer container = null )
        {
            container = container ?? new ServiceContainer();

            foreach (var service in _services)
            {
                container.AddService(service.Key, service.Value);
            }

            return container;
        }

        public static bool Has(Type type)
        {
            return _services.ContainsKey( type );
        }

        public static T Get<T>()
        {
            if( _services.ContainsKey( typeof(T) ) )
            {
                return (T)_services[ typeof( T ) ];
            }
            else
            {
                return default( T );
            }
        }
    }
}
