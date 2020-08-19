using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Mcs.Invoicing.Services.Config.Api.IOC.Cors
{
    public static class CorsStartupHelpers
    {
        public static IServiceCollection AddCors(this IServiceCollection services, IConfiguration configuration)
        {
            return services.AddCors(c =>
            {
                c.AddPolicy("AllowOrigin", options => options.AllowAnyOrigin());
            });
        }
        public static IApplicationBuilder UseCorsMiddleware(this IApplicationBuilder app)
        {
            app.UseCors(options => options
                                  .AllowAnyOrigin());
                                  //.AllowAnyMethod()
                                  //.AllowAnyHeader());

            return app;
        }
    }
}
