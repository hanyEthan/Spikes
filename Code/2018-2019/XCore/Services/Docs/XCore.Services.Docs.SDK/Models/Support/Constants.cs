using XCore.Services.Configurations.SDK.Models.Support;

namespace XCore.Services.Docs.SDK.Models
{
    public static class Constants
    {

        public readonly static ConfigKeyDTO DocumentsConfigKey = new ConfigKeyDTO()
        {
            App = 1,
            Module = 4,
            Key = "XCore.SDK.Document.Config",
        };
    }
}

