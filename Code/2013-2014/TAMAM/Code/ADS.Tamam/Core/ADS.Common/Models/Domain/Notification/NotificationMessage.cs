using System;
using System.Collections.Generic;
using ADS.Common.Models.Enums;

namespace ADS.Common.Models.Domain.Notification
{
    [Serializable]
    public abstract class NotificationMessage
    {
        // properties
        public Guid Id { get; set; }
        public string Code { get; set; }
        public string Message { get; set; }
        public string MessageCultureVariant { get; set; }
        public string MessageHTML { get; set; }
        public NotificationType Type { get; set; }
        public string ActionName { get; set; }
        public string ActionNameCultureVariant { get; set; }
        public string ActionUrl { get; set; }
        public string PersonId { get; set; }
        public string TargetId { get; set; }
        public DateTime CreationTime { get; set; }
        public string SubscribersTokens { get; set; }
        public string DelayTime { get; set; }

        public string CCs { get; set; }
        public string AttachmentsSerialized { get; set; }

        public List<NotificationAttachment> Attachments { get; set; }

        public NotificationTargetType TargetType { get; set; }

        // methods..
        public abstract NotificationDetailedMessage GetDetailedMessage();
    }
}
