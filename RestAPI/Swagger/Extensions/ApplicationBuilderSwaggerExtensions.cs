using Microsoft.AspNetCore.Builder;

namespace RestAPI.Swagger.Extensions
{
    public static class ApplicationBuilderSwaggerExtensions
    {
        public static IApplicationBuilder SetupSwagger(this IApplicationBuilder app)
        {
            app.UseSwagger();
            app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "Baby Educator"));

            return app;
        }
    }
}