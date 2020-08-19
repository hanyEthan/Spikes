using XCore.Services.Configurations.SDK.Models.Support;

namespace XCore.Services.Lookups.Core.Constants
{
    public static class ConfigConstants
    {
        public readonly static ConfigKeyDTO LookupsConfigKey = new ConfigKeyDTO()
        {
            App = 1,
            Module = 11,
            Key = "XCore.Services.Lookups.Config",
        };
    }
}
