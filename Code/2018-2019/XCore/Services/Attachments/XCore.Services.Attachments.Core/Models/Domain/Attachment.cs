using XCore.Framework.Infrastructure.Entities.Repositories.Models;
using XCore.Services.Attachments.Core.Models.Enums;

namespace XCore.Services.Attachments.Core.Models
{
    public class Attachment : Entity<string>
    {
        public string Extension { get; set; }
        public string MimeType { get; set; }
        public byte[] Body { get; set; }
        public AttachmentStatus Status { get; set; }
    }
}
