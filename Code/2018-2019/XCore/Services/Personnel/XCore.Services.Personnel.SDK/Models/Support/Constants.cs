using XCore.Services.Configurations.SDK.Models.Support;

namespace XCore.Services.Personnel.SDK.Models
{
    public static class Constants
    {
        
        public readonly static ConfigKeyDTO PersonnelConfigKey = new ConfigKeyDTO()
        {
            App = 1,
            Module = 8,
            Key = "XCore.SDK.Personnel.Config",
        };
    }
}

