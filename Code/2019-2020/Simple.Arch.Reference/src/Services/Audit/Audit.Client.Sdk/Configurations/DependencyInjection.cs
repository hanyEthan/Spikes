using Mcs.Invoicing.Core.Framework.Infrastructure.AsyncCommunication.Support;
using Mcs.Invoicing.Services.Audit.Client.Sdk.Configurations.Constants;
using Mcs.Invoicing.Services.Audit.Client.Sdk.Configurations.Models;
using Mcs.Invoicing.Services.Audit.Client.Sdk.Contracts;
using Mcs.Invoicing.Services.Audit.Client.Sdk.Handlers;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Mcs.Invoicing.Services.Audit.Client.Sdk.Configurations
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddAuditSDKInfrastructure(this IServiceCollection services, IConfiguration configuration)
        {
            return services.AddConfig(configuration)
                           .AddAsyncCommunication(configuration)
                           .AddAuditSDKClient(configuration);
        }

        private static IServiceCollection AddConfig(this IServiceCollection services, IConfiguration configuration)
        {
            var auditEndpointConfig = new AuditClientConfigurations();
            configuration.GetSection(AuditSdkConfigurationConstants.AuditAsyncClientSectionKey).Bind(auditEndpointConfig);
            services.AddSingleton(auditEndpointConfig);
            var asyncConfig = new AsyncConfiguration();
            configuration.GetSection(ConfigurationConstants.AsyncSectionKey).Bind(asyncConfig);
            services.AddSingleton(asyncConfig);
            return services;
        }

        private static IServiceCollection AddAuditSDKClient(this IServiceCollection services, IConfiguration configuration)
        {
            return services.AddSingleton<IAuditClient, AuditClient>();
        }
    }
}
