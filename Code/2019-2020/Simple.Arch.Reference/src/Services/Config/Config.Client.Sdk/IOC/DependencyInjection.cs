using Mcs.Invoicing.Core.Framework.Infrastructure.AsyncCommunication.Support;
using Mcs.Invoicing.Services.Config.Client.Sdk.Clients.Async;
using Mcs.Invoicing.Services.Config.Client.Sdk.Clients.gRPC;
using Mcs.Invoicing.Services.Config.Client.Sdk.Clients.Rest;
using Mcs.Invoicing.Services.Config.Client.Sdk.Contracts;
using Mcs.Invoicing.Services.Config.Client.Sdk.Handlers;
using Mcs.Invoicing.Services.Config.Client.Sdk.IOC.Constants;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Mcs.Invoicing.Services.Config.Client.Sdk.IOC
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddConfigSDKInfrastructure(this IServiceCollection services, IConfiguration configuration)
        {
            return services.AddConfig(configuration)
                           .AddAsyncCommunication(configuration)
                           .AddConfigSDKClient(configuration);
        }

        #region helpers.

        private static IServiceCollection AddConfig(this IServiceCollection services, IConfiguration configuration)
        {
            #region config : queue transport.

            var endpointConfig = new AsyncClientConfigurations();
            configuration.GetSection(SdkConfigurationConstants.AsyncClientSectionKey).Bind(endpointConfig);
            services.AddSingleton(endpointConfig);

            #endregion
            #region config : service bus

            var asyncConfig = new AsyncConfiguration();
            configuration.GetSection(ConfigurationConstants.AsyncSectionKey).Bind(asyncConfig);
            services.AddSingleton(asyncConfig);

            #endregion
            #region config : gRPC

            var grpcConfig = new GrpcClientConfig();
            configuration.GetSection(SdkConfigurationConstants.GrpcClientSectionKey).Bind(grpcConfig);
            services.AddSingleton(grpcConfig);

            #endregion
            #region config : Rest

            var restConfig = new RestConfig();
            configuration.GetSection(SdkConfigurationConstants.RestClientSectionKey).Bind(restConfig);
            services.AddSingleton(restConfig);

            #endregion

            return services;
        }
        private static IServiceCollection AddConfigSDKClient(this IServiceCollection services, IConfiguration configuration)
        {
            return services.AddScoped<IConfigServiceClient, ConfigServiceClient>();
        }

        #endregion
    }
}
