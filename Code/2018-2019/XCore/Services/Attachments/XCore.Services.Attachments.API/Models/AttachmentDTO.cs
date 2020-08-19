using System;
using XCore.Services.Attachments.Core.Models.Enums;

namespace XCore.Services.Attachments.API.Models
{
    public class AttachmentDTO
    {
        public string Id { get; set; } = Guid.NewGuid().ToString();
        public string MimeType { get; set; }
        public byte[] Body { get; set; }
        public string Name { get; set; }
        public string Extension { get; set; }
        public string Code { get; set; }
        public string CreatedDate { get; set; }
        public string ModifiedDate { get; set; }
        public string CreatedBy { get; set; }
        public string ModifiedBy { get; set; }
        public string MetaData { get; set; }
        public AttachmentStatus Status { get; set; }
    }
}
