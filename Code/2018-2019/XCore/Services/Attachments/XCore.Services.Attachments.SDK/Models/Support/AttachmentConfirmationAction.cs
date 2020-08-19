using System.Collections.Generic;
using XCore.Services.Attachments.SDK.Models.Enums;

namespace XCore.Services.Attachments.SDK.Models
{
    public class AttachmentConfirmationAction
    {
        public List<string> AttachmentId { get; set; }
        public AttachmentConfirmationStatus ConfirmationAction { get; set; }
    }
}
