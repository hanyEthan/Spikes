using Config.Messaging.Processor.Consumers;
using Mcs.Invoicing.Core.Framework.Infrastructure.AsyncCommunication.Support;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Config.Messaging.Processor.IOC.ServiceBus
{
    public static class ServiceBusConfig
    {
        public static IServiceCollection AddAsyncCommunication(this IServiceCollection services, IConfiguration configuration)
        {
            #region config.

            var serviceBusConfig = new AsyncConfiguration();
            configuration.GetSection(Constants.ConfigurationConstants.AsyncSectionKey).Bind(serviceBusConfig);

            services.AddSingleton(serviceBusConfig);

            #endregion

            return services.AddAsyncCommunication(configuration, consumers =>
            {
                consumers.AddAsyncConsumer<ConfigMessageConsumer>($"{serviceBusConfig?.EndPointName}.Create.Queue");
            }, configureHost: true);
        }
    }
}
