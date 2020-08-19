using XCore.Services.Docs.Core.Constants;
using XCore.Services.Clients.Common.Framework.Configurations;
using XCore.Services.Configurations.SDK.Contracts;

namespace XCore.Services.Docs.Core.Support.Config
{
    public class DocumentServiceRemoteConfigProvider : RemoteConfigProvider<DocumentServiceConfig>
    {
        #region cst.

        public DocumentServiceRemoteConfigProvider(IConfigClient configClient) : base(configClient, ConfigConstants.DocumentConfigKey)
        {
        }

        #endregion
    }
}
