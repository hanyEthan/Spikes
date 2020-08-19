using ADS.Common;
using ADS.Common.Utilities;
using ADS.Tamam.Modules.Integration.Models;
using System.Configuration;
using System.Linq;
using Telerik.OpenAccess;
using Telerik.OpenAccess.Metadata;

namespace ADS.Tamam.Modules.Integration.ORM
{
    public partial class DomainContext : OpenAccessContext
    {
        #region fields

        private static string connectionStringName = XConfig.GetValue(Constants.IntegrationDatastoreKey);
        private static BackendConfiguration backend = GetBackendConfiguration();

        #endregion

        #region Ctor

        public DomainContext() : base(connectionStringName, backend, MetaDataFactory.CreateMetadataSource(backend)) 
        { }

        #endregion
        
        #region Querable

        public IQueryable<Person> Personnel { get { return this.GetAll<Person>(); } }
        public IQueryable<Department> Departments { get { return this.GetAll<Department>(); } }
        public IQueryable<Excuse> Excuses { get { return this.GetAll<Excuse>(); } }
        public IQueryable<Leave> Leaves { get { return this.GetAll<Leave>(); } }
        public IQueryable<Holiday> Holidays { get { return this.GetAll<Holiday>(); } }
        public IQueryable<Delegate> Delegates { get { return this.GetAll<Delegate>(); } }
        public IQueryable<JobTitle> JobTitles { get { return this.GetAll<JobTitle>(); } }
        public IQueryable<LeaveType> LeaveTypes { get { return this.GetAll<LeaveType>(); } }
        public IQueryable<MaritalStatus> MaritalStatuses { get { return this.GetAll<MaritalStatus>(); } }
        public IQueryable<Nationality> Nationalities { get { return this.GetAll<Nationality>(); } }
        public IQueryable<Gender> Genders { get { return this.GetAll<Gender>(); } }
        public IQueryable<Religion> Religions { get { return this.GetAll<Religion>(); } }
        public IQueryable<LeavePolicy> LeavePolicies { get { return this.GetAll<LeavePolicy>(); } }
        #endregion
        
        #region Helpers

        public static BackendConfiguration GetBackendConfiguration()
        {
            BackendConfiguration backend = new BackendConfiguration();
            backend.Backend = "MsSql";
            backend.ProviderName = "System.Data.SqlClient";
            CustomizeBackendConfiguration(ref backend);
            //backend.Logging.LogEvents = LoggingLevel.All;
            //backend.Logging.StackTrace = true;
            //backend.Logging.LogEventsToTrace = true;
            return backend;
        }

        static void CustomizeBackendConfiguration(ref BackendConfiguration config)
        {
            config.SecondLevelCache.Enabled = false;
            config.Backend = ConfigurationManager.AppSettings["BackEnd"];
            config.ProviderName = ConfigurationManager.AppSettings["ProviderName"];
        }

        #endregion

        internal class MetaDataFactory
        {
            private static AggregateMetadataSource meta = null;

            internal static AggregateMetadataSource CreateMetadataSource(BackendConfiguration backend)
            {
                if (meta == null)
                {
                    meta = new AggregateMetadataSource(new DomainMetadataSource(), new ADS.Common.Handlers.Data.ORM.DomainMetadataSource(), AggregationOptions.Late);
                    meta.GetModel().DefaultMapping.UseDelimitedSQL = backend.Backend == "MsSql";
                }
                return meta;
            }
        }

    }
}