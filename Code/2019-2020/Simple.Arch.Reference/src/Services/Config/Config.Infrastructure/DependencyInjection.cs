using Mcs.Invoicing.Core.Framework.Infrastructure.AsyncCommunication.Support;
using Mcs.Invoicing.Services.Audit.Client.Sdk.Configurations;
using Mcs.Invoicing.Services.Config.Application.Common.Contracts;
using Mcs.Invoicing.Services.Config.Infrastructure.Messaging.Handlers;
using Mcs.Invoicing.Services.Config.Infrastructure.Persistence.Context;
using Mcs.Invoicing.Services.Config.Infrastructure.Persistence.DataUnity;
using Mcs.Invoicing.Services.Config.Infrastructure.Persistence.Repositories;
using Mcs.Invoicing.Services.Config.Infrastructure.Persistence.Support;
using Mcs.Invoicing.Services.Config.Infrastructure.Services.Audit;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Mcs.Invoicing.Services.Config.Infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
        {
            return services.AddPersistence(configuration)
                           .AddAsyncCommunicationSupport(configuration)
                           .AddSupportingServices(configuration);
        }

        #region Persistence.

        private static IServiceCollection AddPersistence(this IServiceCollection services, IConfiguration configuration)
        {
            var connectionString = configuration.GetConnectionString(PersistenceConstants.ConnectionString_DefaultName);
            var connectionStringRead = connectionString + (connectionString.EndsWith(';') ? "" : ";") + "ApplicationIntent=ReadOnly;";

            return services.AddDbContextPool<ConfigDbContext>(options => options.UseSqlServer(connectionString))
                           .AddDbContextPool<ConfigReadOnlyDbContext>(options => options.UseSqlServer(connectionStringRead))

                           .AddScoped<IConfigItemsRepository, ConfigItemsRepository>()
                           .AddScoped<IConfigItemsReadOnlyRepository, ConfigItemsReadOnlyRepository>()
                           .AddScoped<IModulesRepository, ModulesRepository>()
                           .AddScoped<IModulesReadOnlyRepository, ModulesReadOnlyRepository>()

                           .AddScoped<IConfigurationDataUnity, ConfigurationsDataUnity>();
        }

        #endregion
        #region Async Events.

        private static IServiceCollection AddAsyncCommunicationSupport(this IServiceCollection services, IConfiguration configuration)
        {
            #region config : service bus

            var asyncConfig = new AsyncConfiguration();
            configuration.GetSection(ConfigurationConstants.AsyncSectionKey).Bind(asyncConfig);
            services.AddSingleton(asyncConfig);

            #endregion

            return services.AddAsyncCommunication(configuration)
                           .AddSingleton<IConfigEventsPublisher, ConfigEventsPublisher>();
        }

        #endregion
        #region Supporting Services.

        private static IServiceCollection AddSupportingServices(this IServiceCollection services, IConfiguration configuration)
        {
            return services.AddSingleton<IAuditService, AuditService>()
                           .AddAuditSDKInfrastructure(configuration);
        }

        #endregion
    }
}
