using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.ServiceProcess;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

using ADS.Common.Services.Cache.WCF;
using ADS.Common.Utilities;

namespace ADS.CommonServices.CentralizedCache
{
    public partial class CentralizedCacheWindowsService : ServiceBase
    {
        #region props

        public ServiceHost serviceHost = null;

        #endregion
        #region cst

        public CentralizedCacheWindowsService()
        {
            InitializeComponent();
        }

        #endregion
        #region events

        protected override void OnStart( string[] args )
        {
            //#if DEBUG
            //Debugger.Launch();
            //#endif

            try
            {
                XLogger.Trace( "" );
                if ( serviceHost != null ) serviceHost.Close();

                serviceHost = new ServiceHost( typeof( CacheService ) );
                serviceHost.Open();
            }
            catch ( Exception x )
            {
                XLogger.Error( "Exception : " + x );
                throw;
            }
        }
        protected override void OnStop()
        {
            try
            {
                XLogger.Trace( "" );
                if ( serviceHost != null )
                {
                    serviceHost.Close();
                    serviceHost = null;
                }
            }
            catch ( Exception x )
            {
                XLogger.Error( "Exception : " + x );
                throw;
            }
        }

        #endregion
    }
}
