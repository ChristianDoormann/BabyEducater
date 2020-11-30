using System;
using AcceptanceTests.Helpers;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Persistence.Extensions;
using Shared.Extensions;

namespace AcceptanceTests.Infrastructure
{
    public class TestFixtureData : IDisposable
    {
        public ServiceProvider ServiceProvider { get; }
        
        public TestFixtureData()
        {
            var configuration = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json")
                .Build();
            var services = new ServiceCollection();
            services
                .AddPersistence()
                .AddSharedServices()
                .AddSingleton((IConfiguration) configuration)
                .AddSingleton<TestEnvironment>()
                .AddTransient<ApiHelper>()
                .AddHttpClient();

            ServiceProvider = services.BuildServiceProvider();
        }

        public void Dispose()
        {
            ServiceProvider.Dispose();
        }
    }
}