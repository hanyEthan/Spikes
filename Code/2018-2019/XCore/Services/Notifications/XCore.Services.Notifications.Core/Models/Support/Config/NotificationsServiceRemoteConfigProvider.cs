using System;
using System.Collections.Generic;
using System.Text;
using XCore.Services.Clients.Common.Framework.Configurations;
using XCore.Services.Configurations.SDK.Contracts;
using XCore.Services.Notifications.Core.Utilities;

namespace XCore.Services.Notifications.Core.Models.Support.Config
{
   public class NotificationsServiceRemoteConfigProvider : RemoteConfigProvider<NotificationsServiceConfig>
    {
        #region cst.

        public NotificationsServiceRemoteConfigProvider(IConfigClient configClient) : base(configClient, ConfigConstants.NotificationsConfigKey)
        {
        }

        #endregion
    }
}
