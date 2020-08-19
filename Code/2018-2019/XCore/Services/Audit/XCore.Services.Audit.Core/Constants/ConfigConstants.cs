using XCore.Services.Configurations.SDK.Models.Support;

namespace XCore.Services.Audit.Core.Constants
{
    public static class ConfigConstants
    {
        public readonly static ConfigKeyDTO AuditConfigKey = new ConfigKeyDTO()
        {
            App = 1,
            Module = 2,
            Key = "XCore.Services.Audit.Config",
        };
    }
}
