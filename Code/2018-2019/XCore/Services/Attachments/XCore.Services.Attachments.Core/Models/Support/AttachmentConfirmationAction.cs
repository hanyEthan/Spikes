using System.Collections.Generic;
using XCore.Services.Attachments.Core.Models.Enums;

namespace XCore.Services.Attachments.Core.Models.Support
{
    public class AttachmentConfirmationAction
    {
        public List<string> AttachmentIds { get; set; }
        public AttachmentConfirmationStatus ConfirmationAction { get; set; }
    }
}
