using System;
using ADS.Common.Contracts.Security.Authentication;
using ADS.Common.Models.Domain;
using ADS.Common.Utilities;

namespace ADS.Common.Handlers.Security.Authentication.Providers
{
    public class LDAPAuthenticationProvider : IAuthenticationProvider
    {
        # region props ...

        public bool Initialized { get; private set; }
        public string Name { get { return "LDAPAuthenticationProvider"; } }
        public string Mode { get { return AuthenticationMode.LDAP.ToString(); } }

        # endregion
        # region cst ...

        public LDAPAuthenticationProvider()
        {
            XLogger.Info( "" );

            try
            {
                Initialized = true;
            }
            catch ( Exception x )
            {
                XLogger.Error( "Exception: " + x );
            }
        }

        # endregion
        # region publics ...

        public bool Authenticate( IIdentity identity )
        {
            var LDAP_Domain = Broker.ConfigurationHandler.GetValue( Constants.SectionBroker , Constants.LDAPDomainKey );
            return XLDAP.Authenticate( LDAP_Domain , identity.Username , identity.Password );
        }

        # endregion
    }
}