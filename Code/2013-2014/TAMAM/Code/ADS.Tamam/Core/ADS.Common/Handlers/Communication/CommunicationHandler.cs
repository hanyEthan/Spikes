using ADS.Common.Contracts.Communication;
using ADS.Common.Utilities;

namespace ADS.Common.Handlers.Communication
{
    public class CommunicationHandler : ICommunicationHandler
    {
        # region Properties

        public bool Initialized { get; private set; }
        public string Name { get { return "CommunicationHandler"; } }
        private static bool EncryptionEnabled { get; set; }

        # endregion
        # region Cst..

        public CommunicationHandler()
        {
            XLogger.Info("CommunicationHandler ...");
            EncryptionEnabled = GetQuerStringEncryptionMode();
            Initialized = true;
        }

        # endregion

        #region Publics..

        public string EncryptQueryString(string url)
        {
            return EncryptionEnabled ? XWeb.EncryptQueryString(url) : url;
        }

        public string DecryptQueryString(string url)
        {
            return EncryptionEnabled ? XWeb.DecryptQueryString(url) : url;
        }

        #endregion

        private bool GetQuerStringEncryptionMode()
        {
            var encryptionConfiguration = Broker.ConfigurationHandler.GetValue(Constants.TamamWebClientConfig.Section, Constants.TamamWebClientConfig.QueryStringEncryption);

            bool encryptionEnabled;

            if (!bool.TryParse(encryptionConfiguration, out encryptionEnabled)) encryptionEnabled = true;

            return encryptionEnabled;
        }

    }
}