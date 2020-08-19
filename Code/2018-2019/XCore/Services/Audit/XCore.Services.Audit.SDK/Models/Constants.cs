using XCore.Services.Configurations.SDK.Models.Support;

namespace XCore.Services.Audit.SDK.Models
{
    public static class Constants
    {
        public const string AuditAsyncEndppoint = "XCore.Services.Audit";
        
        public readonly static ConfigKeyDTO AuditConfigKey = new ConfigKeyDTO()
        {
            App = 1,
            Module = 2,
            Key = "XCore.SDK.Audit.Config",
        };
    }
}
