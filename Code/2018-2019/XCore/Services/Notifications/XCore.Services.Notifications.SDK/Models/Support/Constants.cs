
using XCore.Services.Configurations.SDK.Models.Support;

namespace XCore.Services.Notifications.SDK.Models.Support
{
    public static class Constants
    {
        public const string NotificationsAsyncEndppoint = "XCore.Services.Notifications";

        public readonly static ConfigKeyDTO NotificationsConfigKey = new ConfigKeyDTO()
        {
            App = 1,
            Module = 12,
            Key = "XCore.SDK.Notifications.Config",
        };
    }
}
