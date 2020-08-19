using XCore.Services.Configurations.SDK.Models.Support;

namespace XCore.Services.Organizations.Core.Constants
{
    public static class ConfigConstants
    {
        public readonly static ConfigKeyDTO OrganizationConfigKey = new ConfigKeyDTO()
        {
            App = 1,
            Module = 7,
            Key = "XCore.Services.Organization.Config",
        };

        public readonly static ConfigKeyDTO BusConfig = new ConfigKeyDTO()
        {
            App = 1,
            Module = 7,
            Key = "XCore.Services.Organizations.Config.Async",
        };
    }
}
