using System.Collections.Generic;
using XCore.Framework.Infrastructure.Entities.Events.Domain;
using XCore.Services.Notifications.Models.Contracts.Notifications;
using XCore.Services.Notifications.Models.Models.Notifications.MessageTemplate;

namespace XCore.Services.Notifications.Core.Models.Events.Integration
{
    public  class MessageTemplateAttachmentsDeletedIntegrationEvent : DomainEventBase, INotificationMessageTemplateAttachmentsDeletedIntegrationEvent
    {
        public List<NotificationMessageTemplateAttachmentMetadata> Attachments { get; set; }
    }
}
