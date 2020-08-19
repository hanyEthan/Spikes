using XCore.Services.Configurations.SDK.Models.Support;

namespace XCore.Services.Attachments.SDK.Models
{
    public static class Constants
    {
        
        public readonly static ConfigKeyDTO AttachmentsConfigKey = new ConfigKeyDTO()
        {
            App = 1,
            Module = 5,
            Key = "XCore.SDK.Attachment.Config",
        };
    }
}

