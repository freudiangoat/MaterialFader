using Microsoft.Extensions.DependencyInjection;
using System;
using System.Linq;
using System.Reflection;

namespace MaterialFader
{
    internal static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddAllImplementations<TIFace>(this IServiceCollection svc, Assembly assembly, ServiceLifetime lifetime = ServiceLifetime.Transient)
        {
            foreach (var type in assembly.GetTypes().Where(IsMatch<TIFace>))
            {
                svc.Add(new ServiceDescriptor(typeof(TIFace), type, lifetime));
            }

            return svc;
        }

        public static IServiceCollection AddAllInterfaces<T>(this IServiceCollection svc, ServiceLifetime lifetime = ServiceLifetime.Transient)
        {
            foreach (var iface in typeof(T).GetInterfaces())
            {
                svc.Add(new ServiceDescriptor(iface, typeof(T), lifetime));
            }

            return svc;
        }

        private static bool IsMatch<T>(Type t)
            => typeof(T).IsAssignableFrom(t) &&
                !t.IsAbstract &&
                !t.IsInterface;
    }
}
