using XCore.Services.Clients.Common.Framework.Configurations;
using XCore.Services.Configurations.SDK.Contracts;
using XCore.Services.Lookups.Core.Constants;

namespace XCore.Services.Lookups.Core.Support.Config
{
    public class LookupsServiceRemoteConfigProvider : RemoteConfigProvider<LookupsServiceConfig>
    {
        #region cst.

        public LookupsServiceRemoteConfigProvider(IConfigClient configClient) : base(configClient, ConfigConstants.LookupsConfigKey)
        {
        }

        #endregion
    }
}
