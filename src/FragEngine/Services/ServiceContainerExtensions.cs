using System.ComponentModel.Design;

// yeah, yeah but I want these extensions everwhere
namespace FragEngine
{
    public static class ServiceContainerExtensions
    {
        public static void AddService<T>( this IServiceContainer container, T instance )
        {
            container.AddService(typeof(T), instance);
        }

        public static T GetService<T>( this IServiceContainer container )
        {
            return (T)container.GetService( typeof( T ) );
        }
    }
}
