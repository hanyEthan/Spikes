using XCore.Services.Configurations.SDK.Models.Support;

namespace XCore.Services.Hirings.Core.Constants
{
    public static class ConfigConstants
    {
        public readonly static ConfigKeyDTO HiringConfigKey = new ConfigKeyDTO()
        {
            App = 1,
            Module = 10,
            Key = "XCore.Services.Hiring.Config",
        };

        public readonly static ConfigKeyDTO BusConfig = new ConfigKeyDTO()
        {
            App = 1,
            Module = 10,
            Key = "XCore.Services.Hiring.Config.Async",
        };
    }
}
