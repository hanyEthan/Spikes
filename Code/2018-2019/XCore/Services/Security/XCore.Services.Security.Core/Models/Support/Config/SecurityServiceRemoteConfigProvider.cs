using XCore.Services.Clients.Common.Framework.Configurations;
using XCore.Services.Configurations.SDK.Contracts;
using XCore.Services.Security.Core.Utilities;

namespace XCore.Services.Security.Core.Support.Config
{
    public class SecurityServiceRemoteConfigProvider : RemoteConfigProvider<SecurityServiceConfig>
    {
        #region cst.

        public SecurityServiceRemoteConfigProvider(IConfigClient configClient) : base(configClient, ConfigConstants.SecurityConfigKey)
        {
        }

        #endregion
    }
}
