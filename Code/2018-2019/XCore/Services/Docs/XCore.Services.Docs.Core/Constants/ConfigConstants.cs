using XCore.Services.Configurations.SDK.Models.Support;

namespace XCore.Services.Docs.Core.Constants
{
    public static class ConfigConstants
    {
        public readonly static ConfigKeyDTO DocumentConfigKey = new ConfigKeyDTO()
        {
            App = 1,
            Module = 4,
            Key = "XCore.Services.Document.Config",
        };
        public readonly static ConfigKeyDTO BusConfig = new ConfigKeyDTO()
        {
            App = 1,
            Module = 4,
            Key = "XCore.Services.Document.Config.Async",
        };
    }
}
