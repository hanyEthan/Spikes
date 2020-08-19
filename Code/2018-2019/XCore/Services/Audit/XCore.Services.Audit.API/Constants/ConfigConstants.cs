using XCore.Services.Configurations.SDK.Models.Support;

namespace XCore.Services.Audit.API.Constants
{
    public static class ConfigConstants
    {
        public readonly static ConfigKeyDTO BusConfig = new ConfigKeyDTO()
        {
            App = 1,
            Module = 2,
            Key = "XCore.Services.Audit.Config.Async",
        };
    }
}
