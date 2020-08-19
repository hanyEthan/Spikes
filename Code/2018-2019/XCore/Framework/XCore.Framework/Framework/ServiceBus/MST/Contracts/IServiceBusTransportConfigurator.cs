using System;
using MassTransit;
using XCore.Framework.Framework.ServiceBus.MST.Support;

namespace XCore.Framework.Framework.ServiceBus.Contracts
{
    public interface IServiceBusTransportConfigurator
    {
        IBusControl GetServiceBus(ServiceBusConfiguration config, Action<IReceiveEndpointConfigurator> nativeConfig);
        string GetComponentEndPoint(string baseUri, string ComponentRelativeUri);
    }
}
