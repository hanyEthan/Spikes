using System;
using System.Collections.Generic;

namespace XCore.Services.Notifications.API.Model
{
    public class MessageTemplateDTO
    {
        public virtual int Id { get; set; }
        public virtual string Code { get; set; } = Guid.NewGuid().ToString();
        public virtual string Name { get; set; }
        public virtual string NameCultured { get; set; }
        public virtual bool? IsActive { get; set; }
        public virtual string CreatedDate { get; set; }
        public virtual string CreatedBy { get; set; }
        public virtual string ModifiedDate { get; set; }
        public virtual string ModifiedBy { get; set; }
        public virtual string MetaData { get; set; }

        public string Title { get; set; }
        public virtual string AppId { get; set; }
        public virtual string ModuleId { get; set; }
        public virtual string Body { get; set; }
        public virtual List<MessageTemplateKeyDTO> Keys { get; set; }
        public virtual List<DocumentDTO> Attachments { get; set; }

    }
}
