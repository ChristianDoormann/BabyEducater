using Microsoft.Extensions.DependencyInjection;
using Shared.Services;

namespace Shared.Extensions
{
    public static class SharedServiceCollectionExtensions
    {
        public static IServiceCollection AddSharedServices(this IServiceCollection services)
        {
            return services
                .AddTransient<IBabyService, BabyService>();
        }
    }
}