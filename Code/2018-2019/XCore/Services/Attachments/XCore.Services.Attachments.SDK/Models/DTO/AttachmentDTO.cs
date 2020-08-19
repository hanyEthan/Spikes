using System;
using XCore.Services.Attachments.SDK.Models.Enums;

namespace XCore.Services.Attachments.SDK.Models
{
    public class AttachmentDTO 
    {
        public virtual string Id { get; set; } = Guid.NewGuid().ToString();
        public virtual string MimeType { get; set; }
        public virtual byte[] Body { get; set; }
        public virtual string Name { get; set; }
        public virtual string Extension { get; set; }
        public virtual string Code { get; set; }
        public virtual string CreatedDate { get; set; }
        public virtual string ModifiedDate { get; set; }
        public virtual string CreatedBy { get; set; }
        public virtual string ModifiedBy { get; set; }
        public virtual string MetaData { get; set; }
        public virtual AttachmentStatus Status { get; set; }


    }
}
