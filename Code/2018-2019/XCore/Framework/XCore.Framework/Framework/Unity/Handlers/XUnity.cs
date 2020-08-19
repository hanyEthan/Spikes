using System;
using System.Collections.Generic;
using XCore.Framework.Framework.Unity.Contracts;
using XCore.Framework.Framework.Unity.Handlers.Configurations;
using XCore.Framework.Utilities;

namespace XCore.Framework.Framework.Unity.Handlers
{
    public class XUnity
    {
        #region props.

        public virtual bool Initialized { get { return IsInitialized && IsLicensed; } }
        public virtual List<string> InitializaionMessages { get; set; }

        protected virtual bool IsInitialized { get; set; }
        protected virtual bool IsLicensed { get; set; }

        protected virtual Dictionary<string , IUnityCluster> Clusters { get; set; }
        protected virtual IUnityConfigurationsBuilder ConfigBuilder { get; set; }
        protected virtual IUnityConfigurationsProvider ConfigProvider { get; set; }

        #endregion
        #region cst.

        public XUnity()
        {
            this.IsInitialized = Initialize();
            this.IsLicensed = true;
        }

        #endregion
        #region publics.

        public virtual object Resolve( string clusterKey , string serviceKey )
        {
            try
            {
                if ( string.IsNullOrWhiteSpace( clusterKey ) ) return null;
                if ( string.IsNullOrWhiteSpace( serviceKey ) ) return null;
                if ( !this.Clusters.TryGetValue( clusterKey , out IUnityCluster cluster ) ) return null;
                if ( !cluster.Services.TryGetValue( serviceKey , out IUnityService service ) ) return null;

                return service;
            }
            catch ( Exception x )
            {
                XLogger.Error( "Exception : " + x );
                throw;
            }
        }
        public virtual T Resolve<T>( string clusterKey , string serviceKey ) where T : IUnityService
        {
            try
            {
                var service = Resolve( clusterKey , serviceKey );
                return service != null ? ( T ) service : default( T );
            }
            catch ( Exception x )
            {
                XLogger.Error( "Exception : " + x );
                throw;
            }
        }

        #endregion
        #region helpers.

        protected virtual bool Initialize()
        {
            try
            {
                this.ConfigProvider = new XUnityConfigurationsProvider();
                this.ConfigBuilder = new XUnityConfigurationsBuilder( this.ConfigProvider );

                this.Clusters = ConfigBuilder.Build();

                #region validate.

                InitializaionMessages = InitializaionMessages ?? new List<string>();
                if ( ConfigProvider.InitializaionMessages?.Count > 0 ) InitializaionMessages.AddRange( ConfigProvider.InitializaionMessages );
                if ( ConfigBuilder.InitializaionMessages?.Count > 0 ) InitializaionMessages.AddRange( ConfigBuilder.InitializaionMessages );

                if ( InitializaionMessages?.Count > 0 ) return false;

                #endregion

                return true;
            }
            catch ( Exception x )
            {
                return false;
            }
        }

        #endregion
    }
}
