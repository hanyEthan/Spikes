using XCore.Services.Audit.Core.Constants;
using XCore.Services.Clients.Common.Framework.Configurations;
using XCore.Services.Configurations.SDK.Contracts;

namespace XCore.Services.Audit.Core.Support.Config
{
    public class AuditServiceRemoteConfigProvider : RemoteConfigProvider<AuditServiceConfig>
    {
        #region cst.

        public AuditServiceRemoteConfigProvider(IConfigClient configClient) : base(configClient, ConfigConstants.AuditConfigKey)
        {
        }

        #endregion
    }
}
