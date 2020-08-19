using System;

namespace XCore.Services.Notifications.API.Model
{
    public class DocumentDTO
    {
        public virtual int Id { get; set; }
        public virtual string Code { get; set; }= Guid.NewGuid().ToString();
        public virtual string Name { get; set; }
        public virtual string NameCultured { get; set; }
        public virtual bool? IsActive { get; set; }
        public virtual string CreatedDate { get; set; }
        public virtual string CreatedBy { get; set; }
        public virtual string ModifiedDate { get; set; }
        public virtual string ModifiedBy { get; set; }
        public virtual string MetaData { get; set; }
        public string attachId { get; set; }
        public string Entity { get; set; }
        public string UserId { get; set; }
        public string UserName { get; set; }
        public string App { get; set; }
        public string Module { get; set; }
        public string Action { get; set; }
        public string DocumentReferenceId { get; set; }
        public string Category { get; set; }
        public MessageTemplateDTO MessageTemplate { get; set; }
    }
}
