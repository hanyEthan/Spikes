using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;
using XCore.Framework.Infrastructure.Context.Execution.Support;
using XCore.Services.Configurations.API.Constants;
using XCore.Services.Configurations.API.Events.Publishers;
using XCore.Services.Configurations.Core.Extensions.DepencyInjection;
using XCore.Services.Configurations.Core.Models.Events.Domain;

namespace XCore.Services.Configurations.API
{
    public static class StartupHelpers
    {
        #region XCore.

        public static IServiceCollection HandleXCore(this IServiceCollection services, IConfiguration configuration)
        {
            return services.AddXCoreConfigurationService(configuration)
                           .HandleConfigurationEvents(configuration);
        }

        private static IServiceCollection HandleConfigurationEvents(this IServiceCollection services, IConfiguration configuration)
        {
            return services.AddMediatR(typeof(Startup))
                           .AddSingleton<MediatR.INotificationHandler<ConfigEditedDomainEvent>, EventsPublisher>()
                           .AddSingleton<MediatR.IRequestHandler<ConfigEditingDomainEvent, ExecutionResponse<bool>>, EventsPublisher>();
        }

        #endregion
        #region asp.net core.

        public static IServiceCollection HandleConfigurations(this IServiceCollection services, IConfiguration configuration)
        {
            return services.AddSingleton<IConfiguration>(configuration);
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
            });
        }
        public static IApplicationBuilder UserSwaggerUI(this IApplicationBuilder app)
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
            return services.AddCors();
        }

        #endregion
    }
}
