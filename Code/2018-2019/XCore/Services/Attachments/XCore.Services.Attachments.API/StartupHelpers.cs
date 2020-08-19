using MassTransit;
using MassTransit.AspNetCoreIntegration;
using MassTransit.ExtensionsDependencyInjectionIntegration;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;
using System;
using XCore.Framework.Framework.ServiceBus.Contracts;
using XCore.Services.Attachments.API.Constants;
using XCore.Services.Attachments.Core.Constants;
using XCore.Services.Attachments.Core.Extensions.DepencyInjection;
using XCore.Services.Configurations.SDK.Models.Support;
using XCore.Services.Docs.API.Events.Publishers;
using XCore.Framework.Framework.ServiceBus.Handlers;
using XCore.Framework.Framework.ServiceBus.MST.Host;
using XCore.Framework.Framework.ServiceBus.MST.Support;
using XCore.Framework.Infrastructure.Config.Contracts;
using XCore.Services.Clients.Common.Framework.Configurations;
using XCore.Services.Attachments.API.Events.Consumers;
using MediatR;

namespace XCore.Services.Attachments.API
{
    public static class StartupHelpers
    {
        #region XCore.

        public static IServiceCollection HandleXCore(this IServiceCollection services, IConfiguration configuration)
        {
            return services.AddXCoreAttachmentsService(configuration)
                           .HandleServiceBus(configuration)
                           .HandleEvents(configuration);
        }

        #endregion
        #region Events.

        private static IServiceCollection HandleEvents(this IServiceCollection services, IConfiguration configuration)
        {
            return services.AddMediatR(typeof(Startup))
                           .AddSingleton<EventsPublisher>();
        }
        #endregion
        #region service bus.

        public static IServiceCollection HandleServiceBus(this IServiceCollection services, IConfiguration configuration)
        {
            #region SB : consumers.

            services.AddScoped<AttachmentMessageConsumers>();

            // ...
            void ConfigureMassTransitSB(IServiceCollectionConfigurator configurator)
            {
                configurator.AddConsumer<AttachmentMessageConsumers>();
            }

            // ...
            IBusControl CreateBus(IServiceProvider serviceProvider)
            {
                var serviceBus = serviceProvider.GetService<IServiceBus>();
                return serviceBus?.GetBusControl(serviceProvider);
            }

            #endregion
            #region SB : host.


            services.AddSingleton<ConfigKeyDTO>(ConfigConstants.BusConfig)
                   .AddSingleton<IConfigProvider<ServiceBusConfiguration>, RemoteConfigProvider<ServiceBusConfiguration>>()
                   .AddSingleton<IServiceBus, ServiceBus>(x => new ServiceBus(x.GetService<IConfigProvider<ServiceBusConfiguration>>(), e =>
                   {
                       e.Consumer<AttachmentMessageConsumers>(x);
                   }))
                   .AddHostedService<ServiceBusHostedService>()
                   .AddMassTransit(CreateBus, ConfigureMassTransitSB);

            #endregion

            return services;
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
         return  services.AddCors(options =>
          {
        options.AddPolicy("CorsPolicy",
            builder => builder.AllowAnyOrigin()
            .AllowAnyMethod()
            .AllowAnyHeader());
          });
        }

        #endregion
    }
}
