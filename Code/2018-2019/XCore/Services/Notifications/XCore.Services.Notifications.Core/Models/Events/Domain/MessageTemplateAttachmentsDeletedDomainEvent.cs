using System.Collections.Generic;
using XCore.Framework.Infrastructure.Entities.Events.Domain;
using XCore.Services.Notifications.Models.Models.Notifications.MessageTemplate;

namespace XCore.Services.Notifications.Core.Models.Events.Domain
{
    public class MessageTemplateAttachmentsDeletedDomainEvent : DomainEventBase, MediatR.INotification
    {
        #region props.

        public List<NotificationMessageTemplateAttachmentMetadata> Attachments { get; set; }

        #endregion
        #region cst.

        public MessageTemplateAttachmentsDeletedDomainEvent()
        {
            base.App = null;
            base.Module = null;
            base.Model = "NotificationMessageTemplateAttachment";
            base.Action = "Deleted";
            base.User = null;
        }

        #endregion

    }
}
