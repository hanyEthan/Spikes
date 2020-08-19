using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NServiceBus;
using NServiceBus.Logging;
using NServiceBus.Persistence.Sql;
using NServiceBus.Transport.SQLServer;
using XCore.Framework.Framework.ServiceBus.Contracts;
using XCore.Framework.Framework.ServiceBus.Models;
using XCore.Utilities.Utilities;

namespace XCore.Framework.Framework.ServiceBus.Handlers
{
    public class ServiceBus : IServiceBus<SqlServerTransport , SqlPersistence>
    {
        #region props.

        public bool Initialized { get; protected set; }

        public string EndpointName { get; set; }
        public int Failures_Immediate_NumberOfRetries { get; set; } = 3;
        public int Failures_Delayed_NumberOfRetries { get; set; } = 3;
        public TimeSpan Failures_Delayed_TimeIncreases { get; set; } = TimeSpan.FromMinutes( 1 );

        public IEndpointInstance EndpointInstance { get; private set; }
        public EndpointConfiguration EndpointConfiguration { get; private set; }
        public TransportExtensions<SqlServerTransport> Transport { get; private set; }
        public RoutingSettings<SqlServerTransport> Routing { get; private set; }
        public RecoverabilitySettings Recoverability { get; private set; }
        public PersistenceExtensions<SqlPersistence> Persistence { get; private set; }

        public string InstanceName { get; private set; }
        public string TransportConnectionString { get; private set; }
        public string PersistanceConnectionString { get; private set; }
        public bool MaintainQueues { get; private set; }
        public int? ConcurrentThreads { get; private set; }

        //public Logger Log { get; private set; }

        #endregion
        #region cst.

        public ServiceBus( string endpointName )
        {
            this.EndpointName = endpointName;
            this.Initialized = Initialize();
        }
        public ServiceBus( string endpointName , int immediateRetriesCount , int delayedRetriesCount , TimeSpan delayedTimeIncreases ) : this( endpointName )
        {
            this.Failures_Immediate_NumberOfRetries = immediateRetriesCount;
            this.Failures_Delayed_NumberOfRetries = delayedRetriesCount;
            this.Failures_Delayed_TimeIncreases = delayedTimeIncreases;
        }

        #endregion
        #region publics.

        public async Task Start()
        {
            #region initialized ?
            if ( !Initialized ) throw new Exception( "ServiceBus is not initialized correctly." );
            #endregion

            this.EndpointInstance = await Endpoint.Start( this.EndpointConfiguration ).ConfigureAwait( false );
        }
        public async Task Stop()
        {
            #region initialized ?
            if ( !Initialized ) throw new Exception( "ServiceBus is not initialized correctly." );
            #endregion

            await this.EndpointInstance.Stop().ConfigureAwait( false );
        }

        public async Task Send( ICommand message )
        {
            #region initialized ?
            if ( !Initialized ) throw new Exception( "ServiceBus is not initialized correctly." );
            #endregion

            await this.EndpointInstance.Send( message ).ConfigureAwait( false );
        }
        public async Task Publish( IEvent message )
        {
            #region initialized ?
            if ( !Initialized ) throw new Exception( "ServiceBus is not initialized correctly." );
            #endregion

            await this.EndpointInstance.Publish( message ).ConfigureAwait( false );
        }

        public async Task<TResponse> Send<TRequest, TResponse>( TRequest request )
        {
            return await this.EndpointInstance.Request<TResponse>( request );
        }

        public void Route<TMessageType>( string endpointName )
        {
            #region initialized ?
            if ( !Initialized ) throw new Exception( "ServiceBus is not initialized correctly." );
            #endregion
            #region DB Transport related.

            this.Transport.UseSchemaForQueue( endpointName , endpointName );

            #endregion

            this.Routing.RouteToEndpoint( typeof( TMessageType ) , endpointName );
        }
        public void Subscribe<TMessageType>( string endpointName )
        {
            #region initialized ?
            if ( !Initialized ) throw new Exception( "ServiceBus is not initialized correctly." );
            #endregion
            #region DB Transport related.

            this.Transport.UseSchemaForQueue( endpointName , endpointName );

            #endregion

            this.Routing.RegisterPublisher( typeof( TMessageType ).Assembly , endpointName );
        }

        #endregion
        #region helpers.

        private bool Initialize()
        {
            bool state = true;

            state = state && Config();
            state = state && LogSet();
            state = state && TransportCreate();
            state = state && RoutingSet();
            state = state && PersistenceSet();
            state = state && RecoverabilitySet();

            return state;
        }
        private bool Config()
        {
            this.InstanceName = XConfig.GetString( Constants.NSB.InstanceName );
            this.TransportConnectionString = XConfig.GetString( Constants.NSB.Connection_Transport );
            this.PersistanceConnectionString = XConfig.GetString( Constants.NSB.Connection_Persistance );

            this.MaintainQueues = XConfig.GetBool( Constants.NSB.Queues_Maintain ).GetValueOrDefault();
            this.Failures_Immediate_NumberOfRetries = XConfig.GetInt( Constants.NSB.Failures_Immediate_NumberOfRetries ) ?? this.Failures_Immediate_NumberOfRetries;
            this.Failures_Delayed_NumberOfRetries = XConfig.GetInt( Constants.NSB.Failures_Delayed_NumberOfRetries ) ?? this.Failures_Delayed_NumberOfRetries;
            this.Failures_Delayed_TimeIncreases = XConfig.GetTimeSpan( Constants.NSB.Failures_Delayed_TimeIncreases ) ?? this.Failures_Delayed_TimeIncreases;

            this.ConcurrentThreads = XConfig.GetInt( Constants.NSB.ConcurrentThreads );

            return true;
        }
        private bool LogSet()
        {
            var loggerDefinition = LogManager.Use<NSXLoggerDefinition>();
            loggerDefinition.Level( LogLevel.Info );

            return true;
        }
        private bool TransportCreate()
        {
            this.EndpointConfiguration = new EndpointConfiguration( this.EndpointName );
            this.Transport = this.EndpointConfiguration.UseTransport<SqlServerTransport>();
            this.Routing = this.Transport.Routing();

            #region Callbacks

            if ( !string.IsNullOrWhiteSpace( this.InstanceName ) )
            {
                this.EndpointConfiguration.EnableCallbacks();
                this.EndpointConfiguration.MakeInstanceUniquelyAddressable( this.InstanceName );
            }

            #endregion
            #region Concurrency

            if ( this.ConcurrentThreads.HasValue )
            {
                this.EndpointConfiguration.LimitMessageProcessingConcurrencyTo( this.ConcurrentThreads.Value );
            }

            #endregion

            this.EndpointConfiguration.EnableInstallers();
            this.EndpointConfiguration.UseSerialization<NewtonsoftSerializer>();
            this.EndpointConfiguration.SendFailedMessagesTo( "error" );
            this.EndpointConfiguration.AuditProcessedMessagesTo( "audit" );

            this.Transport.ConnectionString( this.TransportConnectionString );

            this.Transport.DefaultSchema( this.EndpointName );
            this.Transport.UseSchemaForQueue( "error" , "dbo" );
            this.Transport.UseSchemaForQueue( "audit" , "dbo" );

            this.Transport.Transactions( TransportTransactionMode.SendsAtomicWithReceive );

            return MaintainQueues ? MaintainDatastore( this.TransportConnectionString , this.EndpointName ) : true;
        }
        private bool RoutingSet()
        {
            return true;
        }
        private bool PersistenceSet()
        {
            this.Persistence = this.EndpointConfiguration.UsePersistence<SqlPersistence>();
            this.Persistence.ConnectionBuilder( connectionBuilder: () => { return new SqlConnection( this.PersistanceConnectionString ); } );

            var persistenceDialect = this.Persistence.SqlDialect<SqlDialect.MsSqlServer>();
            persistenceDialect.Schema( this.EndpointName );
            this.Persistence.TablePrefix( "Persistence." );

            var subscriptions = this.Persistence.SubscriptionSettings();
            subscriptions.CacheFor( TimeSpan.FromMinutes( 1 ) );

            return MaintainQueues ? MaintainDatastore( this.PersistanceConnectionString , this.EndpointName ) : true;
        }
        private bool RecoverabilitySet()
        {
            var recoverability = this.EndpointConfiguration.Recoverability();

            recoverability.Immediate(
                           immediate =>
                           {
                               immediate.NumberOfRetries( this.Failures_Immediate_NumberOfRetries );
                           } );

            recoverability.Delayed(
                           delayed =>
                           {
                               delayed.NumberOfRetries( this.Failures_Delayed_NumberOfRetries );
                               delayed.TimeIncrease( this.Failures_Delayed_TimeIncreases );
                           } );

            return true;
        }

        private bool MaintainDatastore( string connectionString , string schema )
        {
            try
            {
                XDB.CreateSchema( connectionString , schema );

                return true;
            }
            catch ( Exception )
            {
                return false;
            }
        }

        #endregion
    }
}
