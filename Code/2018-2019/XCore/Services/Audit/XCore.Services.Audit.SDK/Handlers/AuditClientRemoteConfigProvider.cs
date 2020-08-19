using XCore.Services.Audit.SDK.Models;
using XCore.Services.Clients.Common.Framework.Configurations;
using XCore.Services.Configurations.SDK.Contracts;

namespace XCore.Services.Audit.SDK.Handlers
{
    public class AuditClientRemoteConfigProvider : RemoteConfigProvider<AuditClientConfig>
    {
        #region cst.

        public AuditClientRemoteConfigProvider(IConfigClient configClient) : base(configClient, Constants.AuditConfigKey)
        {
        }

        #endregion
    }
}
