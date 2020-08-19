using XCore.Services.Hiring.SDK.Models;
using XCore.Services.Clients.Common.Framework.Configurations;
using XCore.Services.Configurations.SDK.Contracts;

namespace XCore.Services.Hiring.SDK.Handlers
{
    public class AttachmentClientRemoteConfigProvider : RemoteConfigProvider<HiringClientConfig>
    {
        #region cst.

        public AttachmentClientRemoteConfigProvider(IConfigClient configClient) : base(configClient, Constants.HiringConfigKey)
        {
        }

        #endregion
    }
}
