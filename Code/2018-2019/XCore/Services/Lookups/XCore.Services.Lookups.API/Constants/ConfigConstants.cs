using XCore.Services.Configurations.SDK.Models.Support;

namespace XCore.Services.Lookups.API.Constants
{
    public static class ConfigConstants
    {
        public readonly static ConfigKeyDTO BusConfig = new ConfigKeyDTO()
        {
            App = 1,
            Module = 11,
            Key = "XCore.Services.Lookups.Config.Async",
        };
    }
}
