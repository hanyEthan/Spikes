using XCore.Services.Configurations.SDK.Models.Support;

namespace XCore.Services.Attachments.Core.Constants
{
    public static class ConfigConstants
    {
        public readonly static ConfigKeyDTO AttachmentConfigKey = new ConfigKeyDTO()
        {
            App = 1,
            Module = 5,
            Key = "XCore.Services.Attachment.Config",
        };

        public readonly static ConfigKeyDTO BusConfig = new ConfigKeyDTO()
        {
            App = 1,
            Module = 5,
            Key = "XCore.Services.Attachments.Config.Async",
        };
    }
}
