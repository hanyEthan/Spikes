using XCore.Services.Clients.Common.Framework.Configurations;
using XCore.Services.Configurations.SDK.Contracts;
using XCore.Services.Hirings.Core.Constants;
using XCore.Services.Hiring.Core.Support.Config;

namespace XCore.Services.Hiring.Core.Support.Config
{
    public class HiringServiceRemoteConfigProvider : RemoteConfigProvider<HiringServiceConfig>
    {
        #region cst.

        public HiringServiceRemoteConfigProvider(IConfigClient configClient) : base(configClient, ConfigConstants.HiringConfigKey)
        {
        }

        #endregion
    }
}
