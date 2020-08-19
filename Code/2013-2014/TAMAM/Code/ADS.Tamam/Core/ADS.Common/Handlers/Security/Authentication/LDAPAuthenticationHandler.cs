using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ADS.Common.Contracts.Security.Authentication;
using ADS.Common.Models.Domain;
using ADS.Common.Utilities;

namespace ADS.Common.Handlers.Security.Authentication
{
    public class LDAPAuthenticationHandler : IAuthenticationHandler
    {
        # region Properties

        public bool Initialized { get; private set; }
        public string Name
        {
            get { return "LDAPAuthenticationHandler"; }
        }

        # endregion

        # region Constructor

        public LDAPAuthenticationHandler(object obj)
        {
            XLogger.Info("");

            try
            {
                Initialized = true;
            }
            catch (Exception x)
            {
                XLogger.Error("Exception: " + x);
            }
        }

        # endregion

        # region Publics

        public bool Authenticate(BaseIdentity identity)
        {
            var LDAP_Domain = Broker.ConfigurationHandler.GetValue(Constants.SectionBroker, Constants.LDAPDomainKey);
            return XLDAP.Authenticate(LDAP_Domain, identity.Username, identity.Password);
        }

        public bool CreateIdentity(BaseIdentity identity)
        {
            throw new NotSupportedException("LDAP does not support the capability of creating new identities.");
        }

        public List<BaseIdentity> GetIdentities(string username, string providerId)
        {
            throw new NotSupportedException("LDAP does not support the capability of get identities by username");
        }

        public bool UpdateIdentity(BaseIdentity identity)
        {
            throw new NotSupportedException("LDAP does not support the capability of updating existing identities.");
        }

        public bool ChangePassword(BaseIdentity identity, string newPassword)
        {
            throw new NotSupportedException("LDAP does not support the capability of changing passwords.");
        }

        # endregion
    }
}