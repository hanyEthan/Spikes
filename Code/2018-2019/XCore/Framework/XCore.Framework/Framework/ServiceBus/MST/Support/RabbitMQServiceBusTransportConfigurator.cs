using MassTransit;
using System;
using XCore.Framework.Framework.ServiceBus.Contracts;
using XCore.Framework.Framework.ServiceBus.MST.Models;

namespace XCore.Framework.Framework.ServiceBus.MST.Support
{
    public class RabbitMQServiceBusTransportConfigurator : IServiceBusTransportConfigurator
    {
        #region IServiceBusTransportConfigurator

        public IBusControl GetServiceBus(ServiceBusConfiguration config, Action<IReceiveEndpointConfigurator> nativeConfig)
        {
            #region validate.

            if (!ValidateConfig(config))
            {
                throw new Exception("Invalid configuration");
            }

            #endregion

            var serviceBus = Bus.Factory.CreateUsingRabbitMq(bus =>
            {
                #region Host.

                var host = bus.Host(new Uri(config.Transport.Uri), settings =>
                {
                    settings.Username(config.Transport.CredentialsUsername);
                    settings.Password(config.Transport.CredentialsPassword);
                });

                #endregion
                #region Concurrent Messages.

                if (config.MaxConcurrentMessages.HasValue)
                {
                    bus.PrefetchCount = config.MaxConcurrentMessages.Value;
                }

                #endregion
                #region endpoints.

                if (nativeConfig != null)
                {
                    bus.ReceiveEndpoint(config.EndPointName, nativeConfig);
                }

                #endregion
            });

            return serviceBus;
        }
        public string GetComponentEndPoint(string baseUri, string ComponentRelativeUri)
        {
            if (string.IsNullOrWhiteSpace(baseUri)) return null;
            if (string.IsNullOrWhiteSpace(ComponentRelativeUri)) return baseUri;

            baseUri = baseUri.EndsWith("/") ? baseUri : baseUri + "/";
            return string.Concat(baseUri, ComponentRelativeUri);
        }

        #endregion
        #region helpers.

        private bool ValidateConfig(ServiceBusConfiguration config)
        {
            bool isValid = true;

            isValid = isValid && config?.Transport != null;
            isValid = isValid && Enum.IsDefined(typeof(TransportType), config.TransportType);
            isValid = isValid && !string.IsNullOrWhiteSpace(config.Transport.Uri);
            isValid = isValid && !string.IsNullOrWhiteSpace(config.Transport.CredentialsUsername);
            isValid = isValid && !string.IsNullOrWhiteSpace(config.Transport.CredentialsPassword);
            isValid = isValid && !string.IsNullOrWhiteSpace(config.EndPointName);

            return isValid;
        }

        #endregion
    }
}
