using XCore.Services.Clients.Common.Framework.Configurations;
using XCore.Services.Configurations.SDK.Contracts;
using XCore.Services.Notifications.SDK.Models.Support;

namespace XCore.Services.Notifications.SDK.Handlers
{
    public class NotificationsClientRemoteConfigProvider : RemoteConfigProvider<NotificationsClientConfig>
    {
        #region cst.

        public NotificationsClientRemoteConfigProvider(IConfigClient configClient) : base(configClient, Constants.NotificationsConfigKey)
        {

        }

        #endregion
    }
}
