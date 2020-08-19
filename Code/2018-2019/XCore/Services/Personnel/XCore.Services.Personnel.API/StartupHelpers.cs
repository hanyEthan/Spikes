using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;
using XCore.Services.Personnel.API.Constants;
using XCore.Services.Personnel.Core.Constants;
using XCore.Services.Personnel.Core.Extensions.DepencyInjection;
using XCore.Services.Configurations.SDK.Models.Support;
using MassTransit;
using System;
using XCore.Framework.Framework.ServiceBus.Contracts;
using XCore.Framework.Infrastructure.Config.Contracts;
using XCore.Framework.Framework.ServiceBus.MST.Support;
using XCore.Services.Clients.Common.Framework.Configurations;
using XCore.Framework.Framework.ServiceBus.Handlers;
using XCore.Services.Personnel.Core.Contracts.Events;
using XCore.Services.Personnel.API.Events.Publishers;
using XCore.Framework.Framework.ServiceBus.MST.Host;
using MassTransit.AspNetCoreIntegration;
using MassTransit.ExtensionsDependencyInjectionIntegration;
using MediatR;
using XCore.Services.Personnel.Models.Events.Domain;

namespace XCore.Services.Personnel.API
{
    public static class StartupHelpers
    {
        #region XCore.

        public static IServiceCollection HandleXCore(this IServiceCollection services, IConfiguration configuration)
        {
            return services.AddXCorePersonnelService(configuration)
                           .HandleServiceBus(configuration)
                           .HandleConfigurationEvents(configuration);

        }

        #endregion
        #region MediatR Events.

        private static IServiceCollection HandleConfigurationEvents(this IServiceCollection services, IConfiguration configuration)
        {
            return services.AddMediatR(typeof(Startup))
                           .AddSingleton</*MediatR.INotificationHandler<PersonCreatedDomainEvent>, */EventsPublisher>();
                           //.AddSingleton<MediatR.INotificationHandler<PersonActivatedDomainEvent>, EventsPublisher>()
                           //.AddSingleton<MediatR.INotificationHandler<PersonDeActivatedDomainEvent>, EventsPublisher>()
                           //.AddSingleton<MediatR.INotificationHandler<PersonUpdatedDomainEvent>, EventsPublisher>()
                           //.AddSingleton<MediatR.INotificationHandler<PersonDeletedDomainEvent>, EventsPublisher>();
        }
        #endregion
        #region service bus.

        public static IServiceCollection HandleServiceBus(this IServiceCollection services, IConfiguration configuration)
        {
            #region SB : consumers.

            // ...
            //services.AddScoped<SecurityMessageConsumer>();

            // ...
            void ConfigureMassTransitSB(IServiceCollectionConfigurator configurator)
            {
                //configurator.AddConsumer<SecurityMessageConsumer>();
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
                        //e.Consumer<SecurityMessageConsumer>(x);
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
    }

}
