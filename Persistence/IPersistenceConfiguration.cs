using Microsoft.Extensions.Configuration;

namespace Persistence
{
    public interface IPersistenceConfiguration
    {
        string ConnectionString { get; }
    }

    public class PersistenceConfiguration : IPersistenceConfiguration
    {
        public string ConnectionString { get; }

        public PersistenceConfiguration(IConfiguration configuration)
        {
            var appSettings = configuration.GetSection("SQLConfiguration");
            ConnectionString = appSettings["ConnectionString"];
        }
    }
}