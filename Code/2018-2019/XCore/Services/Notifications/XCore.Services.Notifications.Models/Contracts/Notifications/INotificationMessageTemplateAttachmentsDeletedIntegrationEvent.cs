using System.Collections.Generic;
using XCore.Framework.Infrastructure.Entities.Events.Domain;
using XCore.Services.Notifications.Models.Models.Notifications.MessageTemplate;

namespace XCore.Services.Notifications.Models.Contracts.Notifications
{
    public interface INotificationMessageTemplateAttachmentsDeletedIntegrationEvent : IDomainEvent
    {
         List<NotificationMessageTemplateAttachmentMetadata> Attachments { get; set; }
    }
}
