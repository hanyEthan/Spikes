namespace XCore.Services.Notifications.SDK.Model
{
    public class MessageTemplateKeyDTO
    {
        public virtual int Id { get; set; }
        public virtual bool? IsActive { get; set; }
        public virtual string CreatedDate { get; set; }
        public virtual string CreatedBy { get; set; }
        public virtual string ModifiedDate { get; set; }
        public virtual string ModifiedBy { get; set; }
        public virtual string MetaData { get; set; }

        public string Key { get; set; }
        public string Description { get; set; }
        public int MessageTemplateId { get; set; }
        public virtual MessageTemplateDTO MessageTemplate { get; set; }
    }
}
