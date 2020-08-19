using XCore.Services.Configurations.SDK.Models.Support;

namespace XCore.Services.Notifications.Core.Utilities
{
    public static class ConfigConstants
    {

        public readonly static ConfigKeyDTO NotificationsConfigKey = new ConfigKeyDTO()
        {
            App = 1,
            Module = 12,
            Key = "XCore.Services.Notifications.Config",
        };

        public readonly static ConfigKeyDTO BusConfig = new ConfigKeyDTO()
        {
            App = 1,
            Module = 12,
            Key = "XCore.Services.Notifications.Config.Async",
        };
    }
}
