using System;
using System.IO;
using System.Linq;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Hosting.Server.Features;
using Microsoft.Extensions.Configuration;
using RestAPI;

namespace AcceptanceTests.Infrastructure
{
    public class TestEnvironment : IDisposable
    {
        private readonly IWebHost _host;
        public string RestApiUrl { get; }

        public TestEnvironment()
        {
            var appSettings = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json")
                .Build();

            _host = new WebHostBuilder()
                .UseKestrel()
                .UseContentRoot(Directory.GetCurrentDirectory())
                .ConfigureAppConfiguration((builderContext, config) =>
                {
                    var ds = Path.DirectorySeparatorChar;
                    var directory = new DirectoryInfo(
                        Path.GetFullPath(Path.Combine(Environment.CurrentDirectory, $"..{ds}..{ds}..{ds}..{ds}..{ds}")));
                    config
                        .AddJsonFile(directory + $"RestAPI{ds}appsettings.json");
                })
                .UseStartup<Startup>()
                .UseUrls(appSettings["RestApiUrl"])
                .Build();
            _host.Start();
            RestApiUrl = _host.ServerFeatures.Get<IServerAddressesFeature>().Addresses.Single();
        }

        public void Dispose()
        {
            _host?.Dispose();
        }
    }
}