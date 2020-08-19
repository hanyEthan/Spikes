using System;
using ADS.Common.Context;
using ADS.Common.Utilities;
using ADS.Tamam.Common.Data.Handlers;
using ADS.Common.Models.Domain;
using ADS.Common.Contracts.Security.Authentication;
using ADS.Common.Handlers;
using ADS.Tamam.Common.Data.Model.Enums;

namespace ADS.Tamam.Modules.Personnel.Handlers
{
    public class TamamIdentitiyProvider : IIdentityProvider , IAuthenticationProvider
    {
        #region Properties

        public bool Initialized { get; private set; }
        public string Name { get { return "TamamIdentitiyProvider"; } }
        public string Mode { get { return AuthenticationMode.Tamam.ToString(); } }

        private PersonnelDataHandler DataHandler;

        #endregion
        #region cst.

        public TamamIdentitiyProvider()
        {
            XLogger.Info( "Initializing ..." );

            DataHandler = new PersonnelDataHandler();
            if ( DataHandler != null && DataHandler.Initialized )
            {
                XLogger.Info( "Initialized" );
                Initialized = true;
            }
            else
            {
                XLogger.Error( "Initialization Failed, underlying handlers are not registered or initilaized successfully." );
                Initialized = false;
            }
        }

        #endregion

        #region IAuthenticationProvider

        public bool Authenticate( IIdentity identity )
        {
            XLogger.Info( "" );

            try
            {
                var response = DataHandler.Authenticate( identity );
                return response.Type == ResponseState.Success && response.Result;
            }
            catch ( Exception x )
            {
                return XLogger.Error( "Exception : " + x );
            }
        }

        #endregion
        #region IIdentityProvider

        public IIdentity GetIdentity( string username )
        {
            XLogger.Info( "" );

            try
            {
                #region Cache

                var cacheKey = "TamamIdentityProvider_GetIdentity" + username;
                var cached = Broker.Cache.Get<IIdentity>( TamamCacheClusters.Person , cacheKey );
                if ( cached != null ) return cached;

                #endregion

                var response = DataHandler.GetPersonByIdentifier( username );
                var identity = response.Type == ResponseState.Success ? response.Result : null;

                #region Cache

                if ( identity != null ) Broker.Cache.Add<IIdentity>( TamamCacheClusters.Person , cacheKey , identity );

                #endregion

                return identity;
            }
            catch ( Exception x )
            {
                XLogger.Error( "Exception : " + x );
                return null;
            }
        }
        public bool ChangePassword( IIdentity identity , string newPassword )
        {
            XLogger.Info( "" );

            try
            {
                if ( identity == null || identity.ProviderName != AuthenticationMode.Tamam.ToString() ) return false;

                var response = DataHandler.ChangePassword( identity , newPassword );

                #region Cache

                if ( response.Type == ResponseState.Success ) Broker.Cache.Invalidate( TamamCacheClusters.Person );

                #endregion

                return response.Type == ResponseState.Success && response.Result;
            }
            catch ( Exception x )
            {
                return XLogger.Error( "Exception : " + x );
            }
        }
        
        #endregion
    }
}