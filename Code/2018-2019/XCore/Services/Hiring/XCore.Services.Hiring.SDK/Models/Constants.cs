using XCore.Services.Configurations.SDK.Models.Support;

namespace XCore.Services.Hiring.SDK.Models
{
    public static class Constants
    {
        
        public readonly static ConfigKeyDTO HiringConfigKey = new ConfigKeyDTO()
        {
            App = 1,
            Module = 10,
            Key = "XCore.SDK.Hiring.Config",
        };
    }
}

