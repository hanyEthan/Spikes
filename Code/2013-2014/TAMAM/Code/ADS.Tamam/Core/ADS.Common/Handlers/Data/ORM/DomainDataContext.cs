using System.Linq;
using System.Configuration;

using Telerik.OpenAccess;
using Telerik.OpenAccess.Metadata;

using ADS.Common.Utilities;
using ADS.Common.Models.Domain;
using ADS.Common.Workflow.Models;
using ADS.Tamam.Common.Data.ORM;
using ADS.Common.Bases.MessageQueuing.Models;
using ADS.Common.Models.Domain.Authorization;
using ADS.Common.Models.Domain.Notification;
using Action = ADS.Common.Models.Domain.Authorization.Action;
using Role = ADS.Common.Models.Domain.Authorization.Role;

namespace ADS.Common.Handlers.Data.ORM
{
    public partial class DomainDataContext : ADSBaseContext , IDomainModelUnitOfWork
    {
        #region IDomainModelUnitOfWork

        public IQueryable<ConfigurationItem> ConfigurationItems { get { return this.GetAll<ConfigurationItem>(); } }

        public IQueryable<AuditTrailAction> AuditTrailActions { get { return this.GetAll<AuditTrailAction>(); } }
        public IQueryable<AuditTrailModule> AuditTrailModules { get { return this.GetAll<AuditTrailModule>(); } }
        public IQueryable<AuditTrailLog> AuditTrailLogs { get { return this.GetAll<AuditTrailLog>(); } }
        public IQueryable<MasterCode> MasterCodes { get { return this.GetAll<MasterCode>(); } }
        public IQueryable<DetailCode> DetailCodes { get { return this.GetAll<DetailCode>(); } }
        public IQueryable<WorkflowInstance> WorkflowInstances { get { return this.GetAll<WorkflowInstance>(); } }
        

        // Authorization
        public IQueryable<Action> Actions { get { return this.GetAll<Action>(); } }
        public IQueryable<Privilege> Privileges { get { return this.GetAll<Privilege>(); } }
        public IQueryable<Role> Roles { get { return this.GetAll<Role>(); } }
        public IQueryable<Actor> Actors { get { return this.GetAll<Actor>(); } }

        // Notification..
        public IQueryable<NotificationMessage> NotificationMessages { get { return this.GetAll<NotificationMessage>(); } }
        public IQueryable<NotificationDetailedMessage> NotificationDetailedMessages { get { return this.GetAll<NotificationDetailedMessage>(); } }
        public IQueryable<EmailMessage> EmailMessages { get { return this.GetAll<EmailMessage>(); } }
        public IQueryable<SMSMessage> SMSMessages { get { return this.GetAll<SMSMessage>(); } }

        public IQueryable<MQMessage> MQMessages { get { return this.GetAll<MQMessage>(); } }

        #endregion
        #region OpenAccessContext

        private static string connectionStringName = XConfig.GetValue(Constants.CommonDatastoreKey);
        private static BackendConfiguration backend = GetBackendConfiguration();

        public DomainDataContext() : base(connectionStringName, backend, GetMetaSource())
        {
            ConfigureTracing();
        }
        public DomainDataContext(string connection) : base(connection, backend, GetMetaSource()) { }
        public DomainDataContext(BackendConfiguration backendConfiguration) : base(connectionStringName, backendConfiguration, GetMetaSource()) { }
        public DomainDataContext(string connection, MetadataSource metadataSource) : base(connection, backend, metadataSource) { }
        public DomainDataContext(string connection, BackendConfiguration backendConfiguration, MetadataSource metadataSource) : base(connection, backendConfiguration, metadataSource) { }

        public static BackendConfiguration GetBackendConfiguration()
        {
            BackendConfiguration backend = new BackendConfiguration();
            backend.Backend = "MsSql";
            backend.ProviderName = "System.Data.SqlClient";

            CustomizeBackendConfiguration(ref backend);

            return backend;
        }

        #endregion
        #region Custom

        private IObjectScope objectScope;
        public BackendConfiguration.BackendInformation BackendInfo
        {
            get
            {
                if ( this.objectScope == null )
                {
                    this.objectScope = this.GetScope();
                }
                return this.objectScope.Database.BackendConfiguration.BackendInfo;
            }
        }
        
        static void CustomizeBackendConfiguration( ref BackendConfiguration config )
        {
            config.SecondLevelCache.Enabled = false;
            config.Backend = ConfigurationManager.AppSettings["BackEnd"];
            config.ProviderName = ConfigurationManager.AppSettings["ProviderName"];
        }
        static MetadataSource GetMetaSource()
        {
            MetadataSource metadataSource = new DomainMetadataSource();
            bool useDelimitedSQL = bool.Parse( ConfigurationManager.AppSettings["UseDelimitedSQL"] );
            metadataSource.GetModel().DefaultMapping.UseDelimitedSQL = useDelimitedSQL;
            return metadataSource;
        }
        static void ConfigureTracing()
        {
            //backend.Logging.LogEvents = LoggingLevel.Normal;
            //backend.Logging.StackTrace = true;
            //backend.Logging.EventStoreCapacity = 10000;
            //backend.Logging.MetricStoreCapacity = 3600;
            //backend.Logging.MetricStoreSnapshotInterval = 1000;
            //backend.Logging.Downloader.EventPollSeconds = 1;
            //backend.Logging.Downloader.MetricPollSeconds = 1;
        }

        #endregion
    }

    public interface IDomainModelUnitOfWork : IUnitOfWork
    {
        IQueryable<ConfigurationItem> ConfigurationItems { get; }

        IQueryable<AuditTrailAction> AuditTrailActions { get; }
        IQueryable<AuditTrailModule> AuditTrailModules { get; }
        IQueryable<AuditTrailLog> AuditTrailLogs { get; }
        IQueryable<MasterCode> MasterCodes { get; }
        IQueryable<DetailCode> DetailCodes { get; }
        IQueryable<WorkflowInstance> WorkflowInstances { get; }

        // Authorization
        IQueryable<Action> Actions { get; }
        IQueryable<Privilege> Privileges { get; }
        IQueryable<Role> Roles { get; }
        IQueryable<Actor> Actors { get; }

        // Notification
        IQueryable<NotificationMessage> NotificationMessages { get; }
        IQueryable<NotificationDetailedMessage> NotificationDetailedMessages { get; }
        IQueryable<EmailMessage> EmailMessages { get; }
        IQueryable<SMSMessage> SMSMessages { get; }
        
        IQueryable<MQMessage> MQMessages { get; }
    }
}
