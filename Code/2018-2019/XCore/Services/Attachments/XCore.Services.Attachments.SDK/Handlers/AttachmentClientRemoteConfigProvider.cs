using XCore.Services.Attachments.SDK.Models;
using XCore.Services.Clients.Common.Framework.Configurations;
using XCore.Services.Configurations.SDK.Contracts;

namespace XCore.Services.Attachments.SDK.Handlers
{
    public class AttachmentClientRemoteConfigProvider : RemoteConfigProvider<AttachmentClientConfig>
    {
        #region cst.

        public AttachmentClientRemoteConfigProvider(IConfigClient configClient) : base(configClient, Constants.AttachmentsConfigKey)
        {
        }

        #endregion
    }
}
