using Microsoft.Extensions.DependencyInjection;
using Persistence.Repositories;

namespace Persistence.Extensions
{
    public static class PersistenceServiceCollectionExtensions
    {
        public static IServiceCollection AddPersistence(this IServiceCollection services)
        {
            return services
                .AddTransient<IPersistenceConfiguration, PersistenceConfiguration>()
                .AddSingleton<IBabyRepository, BabyRepository>();
        }
    }
}