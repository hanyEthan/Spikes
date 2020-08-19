using XCore.Services.Attachments.Core.Constants;
using XCore.Services.Attachments.Core.Models.Support;
using XCore.Services.Clients.Common.Framework.Configurations;
using XCore.Services.Configurations.SDK.Contracts;

namespace XCore.Services.Attachments.Core.Support.Config
{
    public class AttachmentServiceRemoteFileConfigProvider : RemoteConfigProvider<FileSystemDirectorySettings>
    {
        #region cst.

        public AttachmentServiceRemoteFileConfigProvider(IConfigClient configClient) : base(configClient, ConfigConstants.AttachmentConfigKey)
        {
        }

        #endregion
    }
}
