using XCore.Services.Configurations.SDK.Models.Support;

namespace XCore.Services.Security.Core.Utilities
{
    public static class ConfigConstants
    {
        public readonly static ConfigKeyDTO SecurityConfigKey = new ConfigKeyDTO()
        {
            App = 1,
            Module = 6,
            Key = "XCore.Services.Security.Config",
        };

        public readonly static ConfigKeyDTO BusConfig = new ConfigKeyDTO()
        {
            App = 1,
            Module = 6,
            Key = "XCore.Services.Security.Config.Async",
        };
    }
}
