using XCore.Services.Configurations.SDK.Models.Support;

namespace XCore.Services.Gateway.API.Constants
{
    public static class ConfigConstants
    {
        public readonly static ConfigKeyDTO BusConfig = new ConfigKeyDTO()
        {
            App = 1,
            Module = 3,
            Key = "XCore.Services.Gateway.Config.Async",
        };
    }
}
