using XCore.Services.Configurations.SDK.Models.Support;

namespace XCore.Services.Personnel.Core.Constants
{
    public static class ConfigConstants
    {
        public readonly static ConfigKeyDTO PersonnelConfigKey = new ConfigKeyDTO()
        {
            App = 1,
            Module = 8,
            Key = "XCore.Services.Personnel.Config",
        };
        public readonly static ConfigKeyDTO BusConfig = new ConfigKeyDTO()
        {
            App = 1,
            Module = 8,
            Key = "XCore.Services.Personnel.Config.Async",
        };
    }
}
