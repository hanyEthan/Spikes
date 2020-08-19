using XCore.Services.Clients.Common.Framework.Configurations;
using XCore.Services.Configurations.SDK.Contracts;
using XCore.Services.Organizations.Core.Constants;

namespace XCore.Services.Organizations.Core.Support.Config
{
    public class OrganizationServiceRemoteConfigProvider : RemoteConfigProvider<OrganizationServiceConfig>
    {
        #region cst.

        public OrganizationServiceRemoteConfigProvider(IConfigClient configClient) : base(configClient, ConfigConstants.OrganizationConfigKey)
        {
        }

        #endregion
    }
}
