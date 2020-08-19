using System;
using ADS.Common.Models.Enums;

namespace ADS.Common.Models.Domain.Notification
{
    public abstract class NotificationDetailedMessage
    {
        public Guid Id { get; set; }
        public string Code { get; set; }
        public string Message { get; set; }
        public string MessageCultureVariant { get; set; }
        public NotificationType Type { get; set; }
        public string ActionName { get; set; }
        public string ActionNameCultureVariant { get; set; }
        public string ActionUrl { get; set; }
        public string PersonId { get; set; }
        public string TargetId { get; set; }
        public DateTime CreationTime { get; set; }
        public NotificationStatus Status { get; set; }
        public bool IsRead { get; set; }
        public string Metadata { get; set; }


        public string CreationTimeFormatted
        {
            get
            {
                return CreationTime.ToString( "dd/MM - hh:mm tt" );
            }
        }
    }
}
