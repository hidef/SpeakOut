using Microsoft.Extensions.DependencyInjection;

namespace SpeakOut
{
    public static class IServiceScopeExtensions
    {
        public static T GetService<T>(this IServiceScope self)
        {
            return (T) self.ServiceProvider.GetService(typeof(T));
        }
    }
}
