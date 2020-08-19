using XCore.Framework.Infrastructure.Entities.Repositories.Models;

namespace XCore.Services.Notifications.Core.Models.Domain
{
    public class MessageTemplateAttachment : Entity<int>
    {
        public string DocumentReferenceId { get; set; }
        public string AttachmentReferenceId { get; set; }
        public string UserId { get; set; }
        public string UserName { get; set; }
        public string App { get; set; }
        public string Module { get; set; }
        public string Action { get; set; }
        public string Category { get; set; }

        public int MessageTemplateId { get; set; }
        public MessageTemplate MessageTemplate { get; set; }
    }
}
