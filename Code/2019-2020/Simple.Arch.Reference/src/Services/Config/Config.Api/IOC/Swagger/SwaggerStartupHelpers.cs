using Mcs.Invoicing.Services.Config.Api.Rest.Constants;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;

namespace Mcs.Invoicing.Services.Config.Api.IOC.Swagger
{
    public static class SwaggerStartupHelpers
    {
        public static IServiceCollection AddSwaggerGen(this IServiceCollection services, IConfiguration configuration)
        {
            return services.AddSwaggerGen(options =>
            {
                options.SwaggerDoc(ApiConstants.ApiVersion, new OpenApiInfo { Title = ApiConstants.ApiName, Version = ApiConstants.ApiVersion });
            });
        }
        public static IApplicationBuilder UseSwaggerUI(this IApplicationBuilder app)
        {
            app.UseSwagger();
            app.UseSwaggerUI(options =>
            {
                options.SwaggerEndpoint($"/swagger/{ApiConstants.ApiVersion}/swagger.json", ApiConstants.ApiName);
            });

            return app;
        }
    }
}
