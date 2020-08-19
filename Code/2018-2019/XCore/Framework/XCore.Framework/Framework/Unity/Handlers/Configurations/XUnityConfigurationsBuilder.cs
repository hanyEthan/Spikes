using System;
using System.Collections.Generic;
using Autofac;
using XCore.Framework.Framework.Unity.Contracts;
using XCore.Framework.Framework.Unity.Models;
using XCore.Framework.Utilities;

namespace XCore.Framework.Framework.Unity.Handlers.Configurations
{
    public class XUnityConfigurationsBuilder : IUnityConfigurationsBuilder
    {
        #region props.

        public virtual bool Initialized { get; protected set; }
        public virtual List<string> InitializaionMessages { get; protected set; }
        public virtual IUnityConfigurationsProvider ConfigurationsProvider { get; set; }

        private IContainer IOCContainer { get; set; }

        #endregion
        #region cst.

        public XUnityConfigurationsBuilder( IUnityConfigurationsProvider configurationsProvider )
        {
            this.ConfigurationsProvider = configurationsProvider;

            this.Initialized = this.ConfigurationsProvider.Initialized;
            this.Initialized = this.Initialized && Initialize();
        }

        #endregion
        #region IUnityConfigurationsBuilder

        public Dictionary<string , IUnityCluster> Build()
        {
            if ( !this.Initialized ) return null;

            var config = this.ConfigurationsProvider.Get();
            var status = BuildServices( config );

            return status ? BuildClusters( config ) : null;
        }

        #endregion
        #region helpers.

        protected virtual bool Initialize()
        {
            try
            {
                return true;
            }
            catch ( Exception x )
            {
                return false;
            }
        }

        protected virtual bool BuildServices( UnityConfig configs )
        {
            try
            {
                #region validate.

                if ( configs == null )
                {
                    MemLog( "Couldn't build services, invalid config." );
                    return false;
                }

                #endregion

                var IOCBuilder = new ContainerBuilder(); // IOC container builder
                foreach ( var configuredService in configs.ConfiguredServices )
                {
                    var referenceInterface = XReflector.GetType( configuredService.ServiceContract );
                    var referenceConcrete = XReflector.GetType( configuredService.ServiceImplementation );

                    if ( !referenceInterface.IsAssignableTo<IUnityService>() )
                    {
                        MemLog( string.Format( "error building services : service with the key {0} is not derived from mandatory interface <IUnityService>" , configuredService.Key ) );
                        return false;
                    }

                    var IOCType = IOCBuilder.RegisterType( referenceConcrete )
                                            .Named( configuredService.Key , referenceInterface )
                                            .SingleInstance();

                    if ( configuredService.Parameters != null )
                    {
                        foreach ( var parameter in configuredService.Parameters )
                        {
                            IOCType = IOCType.WithParameter( parameter.Name , parameter.Value );
                        }
                    }
                }

                this.IOCContainer = IOCBuilder.Build();

                return true;
            }
            catch ( Exception x )
            {
                XLogger.Error( "Exception : " + x );
                MemLog( "Couldn't build services, invalid config : " + x );

                return false;
            }
        }
        protected virtual Dictionary<string , IUnityCluster> BuildClusters( UnityConfig configs )
        {
            try
            {
                #region validate.

                if ( configs == null )
                {
                    MemLog( "Couldn't build clusters, invalid config." );
                    return null;
                }

                #endregion

                var clusters = new Dictionary<string , IUnityCluster>();

                foreach ( var configuredServices in configs.ConfiguredServices )
                {
                    IUnityCluster cluster;
                    if ( !clusters.TryGetValue( configuredServices.ClusterKey , out cluster ) )
                    {
                        cluster = new UnityCluster() { Name = configuredServices.ClusterKey , };
                        clusters.Add( configuredServices.ClusterKey , cluster );
                    }

                    //var handler = this.IOCContainer.Resolve( XReflector.GetType( configuredServices.ServiceContract ) ) as IUnityService;
                    var handler = this.IOCContainer.ResolveNamed( configuredServices.Key , XReflector.GetType( configuredServices.ServiceContract ) ) as IUnityService;

                    #region validate.

                    if ( handler == null )
                    {
                        MemLog( string.Format( "Couldn't build clusters, error loading registered service {0} from IOC Container."  , configuredServices.Key ) );
                        return null;
                    }

                    #endregion

                    cluster.Services.Add( configuredServices.Key , handler );
                }

                return clusters;
            }
            catch ( Exception x )
            {
                XLogger.Error( "Exception : " + x );
                return null;
            }
        }
        private void MemLog( string message )
        {
            // ...
            InitializaionMessages = InitializaionMessages ?? new List<string>();

            // ...
            InitializaionMessages.Add( string.Format( "XUnity : [{0}] : {1}" , DateTime.UtcNow.ToString() , message ) );
        }

        #endregion
    }
}
