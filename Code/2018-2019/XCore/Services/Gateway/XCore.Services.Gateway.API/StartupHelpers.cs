using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;
using Ocelot.DependencyInjection;
using XCore.Services.Clients.Extensions.DepencyInjection;
using XCore.Services.Configurations.SDK.Models.Support;
using XCore.Services.Gateway.API.Constants;
using Ocelot.Middleware;


namespace XCore.Services.Gateway.API
{
    public static class StartupHelpers
    {
        #region XCore.

        public static IServiceCollection HandleXCore(this IServiceCollection services, IConfiguration configuration)
        {
            return services.AddXCoreServices(configuration);
        }

        #endregion

        #region asp.net core.

        public static IServiceCollection HandleConfigurations(this IServiceCollection services, IConfiguration configuration)
        {
            return services.AddSingleton<IConfiguration>(configuration)
                           .AddSingleton<ConfigKeyDTO>(ConfigConstants.BusConfig);
        }
        public static IServiceCollection HandleVersioning(this IServiceCollection services, IConfiguration configuration)
        {
            return services.AddApiVersioning(options =>
            {
                options.AssumeDefaultVersionWhenUnspecified = true;
            });
        }

        #endregion
        #region swagger.

        public static IServiceCollection HandleSwaggerGen(this IServiceCollection services, IConfiguration configuration)
        {
            return services.AddSwaggerGen(options =>
            {
                options.SwaggerDoc(ApiConstants.ApiVersion, new OpenApiInfo { Title = ApiConstants.ApiName, Version = ApiConstants.ApiVersion });
                options.CustomSchemaIds(x => x.FullName);
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

        #endregion
        #region Cors.

        public static IServiceCollection HandleCors(this IServiceCollection services, IConfiguration configuration)
        {
            return services.AddCors(options =>
            {
                options.AddPolicy("CorsPolicy",builder => builder
                       .AllowAnyOrigin()
                       .AllowAnyMethod()
                       .AllowAnyHeader()
                       //.AllowCredentials()
                       .WithExposedHeaders("Content-Disposition"));
            });
        }

        #endregion
        #region Ocelot

        public static void HandleOcelot(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddOcelot(configuration);
        }
        public static void UseOcelotMiddleware(this IApplicationBuilder app)
        {
            app.UseOcelot()
               .Wait();
        }

        #endregion
    }
}
