using System;
using System.Linq;
using System.Collections.Generic;

using ADS.Common.Utilities;
using ADS.Common.Models.Domain;
using ADS.Common.Contracts.Security.Authentication;

namespace ADS.Common.Handlers.Security.Authentication
{
    public class AuthenticationService : IAuthenticationService
    {
        # region props ...

        public bool Initialized { get; private set; }
        public string Name { get { return "AuthenticationService"; } }

        private Dictionary<string , IAuthenticationProvider> _AuthenticationProviders;
        public Dictionary<string , IAuthenticationProvider> AuthenticationProviders
        {
            get { return _AuthenticationProviders; }
            set
            {
                _AuthenticationProviders = value;
                Initialized = IsDependenciesInitialized();
            }
        }

        private IIdentityProvider _IdentityProvider;
        public IIdentityProvider IdentityProvider
        {
            get { return _IdentityProvider; }
            set
            {
                _IdentityProvider = value;
                Initialized = IsDependenciesInitialized();
            }
        }

        # endregion
        # region cst ...

        public AuthenticationService()
        {
            XLogger.Info( "" );
        }

        # endregion

        # region publics ...

        public bool Authenticate( IIdentity identity )
        {
            XLogger.Info( "" );

            try
            {
                var provider = GetAuthenticationProvider( identity );
                if ( provider == null ) return false;

                return provider.Authenticate( identity );
            }
            catch ( Exception x )
            {
                XLogger.Error( "Exception : " + x );
                return false;
            }
        }

        public IIdentity GetIdentity( string username )
        {
            XLogger.Info( "" );

            try
            {
                return IdentityProvider.GetIdentity( username );
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
                return IdentityProvider.ChangePassword( identity , newPassword );
            }
            catch ( Exception x )
            {
                return XLogger.Error( "Exception : " + x );
            }
        }

        # endregion
        #region helpers ...

        private IAuthenticationProvider GetAuthenticationProvider( IIdentity identity )
        {
            if ( identity == null ) return null;
            if ( string.IsNullOrWhiteSpace( identity.ProviderName ) ) return null;
            if ( !AuthenticationProviders.ContainsKey( identity.ProviderName ) ) return null;

            return AuthenticationProviders[identity.ProviderName];
        }
        private bool IsDependenciesInitialized()
        {
            try
            {
                var status_identity = ( _IdentityProvider != null && _IdentityProvider.Initialized );
                var status_providers = AuthenticationProviders != null && !AuthenticationProviders.Any( x => x.Value.Initialized == false );

                return status_identity && status_providers;
            }
            catch
            {
                return false;
            }
        }

        #endregion
    }
}