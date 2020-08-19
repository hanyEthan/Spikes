
using XCore.Services.Configurations.SDK.Models.Support;

namespace XCore.Services.Security.SDK.Models.Support
{
    public static class Constants
    {
        public const string SecurityAsyncEndppoint = "XCore.Services.Security";

        public readonly static ConfigKeyDTO SecurityConfigKey = new ConfigKeyDTO()
        {
            App = 1,
            Module = 6,
            Key = "XCore.SDK.Security.Config",
        };
    }
}
