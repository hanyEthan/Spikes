using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MassTransit;
using MassTransit.ExtensionsDependencyInjectionIntegration;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using XCore.Framework.Framework.ServiceBus.Contracts;
using XCore.Framework.Framework.ServiceBus.Handlers;
using XCore.Framework.Framework.ServiceBus.MST.Host;
using XCore.Framework.Framework.ServiceBus.MST.Support;
using XCore.Framework.Infrastructure.Config.Contracts;
using XCore.Services.Clients.Common.Framework.Configurations;
using XCore.Services.Configurations.SDK.Models.Support;
using XCore.Services.Lookups.API.Constants;
using XCore.Services.Lookups.Core.Extensions.DepencyInjection;

namespace XCore.Services.Lookups.API
{
    public static class StartupHelpers
    {
        #region XCore.

        public static IServiceCollection HandleXCore(this IServiceCollection services, IConfiguration configuration)
        {
            return services.AddXCoreAuditService(configuration)
                           .HandleServiceBus(configuration);
        }

        #endregion
        #region service bus.

        public static IServiceCollection HandleServiceBus(this IServiceCollection services, IConfiguration configuration)
        {
            #region SB : consumers.

            //// ...
            //services.AddScoped<AuditMessageConsumer>();

            //// ...
            //void ConfigureMassTransitSB(IServiceCollectionConfigurator configurator)
            //{
            //    configurator.AddConsumer<AuditMessageConsumer>();
            //}

            //// ...
            //IBusControl CreateBus(IServiceProvider serviceProvider)
            //{
            //    var serviceBus = serviceProvider.GetService<IServiceBus>();
            //    return serviceBus?.GetBusControl(serviceProvider);
            //}

            #endregion
            #region SB : host.

            //services.AddSingleton<ConfigKeyDTO>(ConfigConstants.BusConfig)
            //        .AddSingleton<IConfigProvider<ServiceBusConfiguration>, RemoteConfigProvider<ServiceBusConfiguration>>()
            //        .AddSingleton<IServiceBus, ServiceBus>(x => new ServiceBus(x.GetService<IConfigProvider<ServiceBusConfiguration>>(), e =>
            //        {
            //            e.Consumer<AuditMessageConsumer>(x);
            //        }))
            //        .AddScoped<IAuditEventsPublisher, EventsPublisher>()
            //        .AddHostedService<ServiceBusHostedService>()
            //        .AddMassTransit(CreateBus, ConfigureMassTransitSB);

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
        #region health.


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
            return services.AddCors();
        }

        #endregion
    }
}
