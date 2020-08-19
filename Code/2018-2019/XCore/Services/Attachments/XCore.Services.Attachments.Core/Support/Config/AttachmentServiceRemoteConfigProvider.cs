using XCore.Services.Attachments.Core.Constants;
using XCore.Services.Clients.Common.Framework.Configurations;
using XCore.Services.Configurations.SDK.Contracts;

namespace XCore.Services.Attachments.Core.Support.Config
{
    public class AttachmentServiceRemoteConfigProvider : RemoteConfigProvider<AttachmentServiceConfig>
    {
        #region cst.

        public AttachmentServiceRemoteConfigProvider(IConfigClient configClient) : base(configClient, ConfigConstants.AttachmentConfigKey)
        {
        }

        #endregion
    }
}
