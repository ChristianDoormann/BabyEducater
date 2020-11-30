using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Persistence.Extensions;
using RestAPI.Swagger.Extensions;
using Shared.Extensions;

namespace RestAPI
{
    public class Startup
    {
        private IConfiguration Configuration { get; }

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();
            services
                .AddMvcCore(options =>
                {
                    options.EnableEndpointRouting = false;
                })
                .ConfigureApiBehaviorOptions(options =>
                {
                    options.SuppressMapClientErrors = true;
                })
                .AddDataAnnotations();
            
            services.SetupSwagger();
            services.AddPersistence();
            services.AddSharedServices();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.SetupSwagger();
            app.UseHttpsRedirection();
            app.UseRouting();
            app.UseEndpoints(endpoints => { endpoints.MapControllers(); });
        }
    }
}