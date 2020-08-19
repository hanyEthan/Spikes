using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NServiceBus;
using NServiceBus.Persistence;
using NServiceBus.Transport;

namespace XCore.Framework.Framework.ServiceBus.Contracts
{
    public interface IServiceBus<TTransport, TPersistence> where TTransport : TransportDefinition, new()
                                                           where TPersistence : PersistenceDefinition, new()
    {
        #region props.

        bool Initialized { get; }

        string EndpointName { get; set; }
        int Failures_Immediate_NumberOfRetries { get; set; }
        int Failures_Delayed_NumberOfRetries { get; set; }
        TimeSpan Failures_Delayed_TimeIncreases { get; set; }

        IEndpointInstance EndpointInstance { get; }
        EndpointConfiguration EndpointConfiguration { get; }
        TransportExtensions<TTransport> Transport { get; }
        RoutingSettings<TTransport> Routing { get; }
        RecoverabilitySettings Recoverability { get; }
        PersistenceExtensions<TPersistence> Persistence { get; }

        string TransportConnectionString { get; }
        string PersistanceConnectionString { get; }

        //Logger Log { get; }

        #endregion
        #region publics.

        Task Start();
        Task Stop();

        Task Send( ICommand message );
        Task Publish( IEvent message );

        void Route<TMessageType>( string endpointName );
        void Subscribe<TMessageType>( string endpointName );

        #endregion
    }
}
