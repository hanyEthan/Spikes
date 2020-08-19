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
    public class TamamAuthenticationHandler : IAuthenticationHandler
    {
        # region Properties

        public bool Initialized { get; private set; }
        public string Name
        {
            get { return "TamamAuthenticationHandler"; }
        }

        private IAuthenticationDataHandler _DataHandler;

        # endregion

        # region Constructor

        public TamamAuthenticationHandler(IAuthenticationDataHandler dataHandler)
        {
            XLogger.Info("");

            try
            {
                _DataHandler = dataHandler;
                Initialized = _DataHandler != null && _DataHandler.Initialized;
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
            XLogger.Info("");

            return _DataHandler.Authenticate(identity);
        }

        public bool CreateIdentity(BaseIdentity identity)
        {
            XLogger.Info("");

            return _DataHandler.CreateIdentity(identity);
        }

        public List<BaseIdentity> GetIdentities(string username, string providerId)
        {
            XLogger.Info("");

            return _DataHandler.GetIdentities(username, providerId);
        }

        public bool UpdateIdentity(BaseIdentity identity)
        {
            XLogger.Info("");

            return _DataHandler.UpdateIdentity(identity);
        }

        public bool ChangePassword(BaseIdentity identity, string newPassword)
        {
            XLogger.Info("");

            return _DataHandler.ChangePassword(identity, newPassword);
        }

        # endregion



    }
}