using System;
using ADS.Common.Contracts.Notification;

namespace ADS.Tamam.Modules.Attendance.NotificationHandlers
{
    public class NotificationReviewMetadata : INotificationTypeMetadata
    {
        // properties
        public Guid TargetId { get; set; }
        public bool Approved { get; set; }
        public string Comment { get; set; }
        public Guid ReviewerId { get; set; }
        public string Metadata { get; set; }

        public NotificationReviewMetadata(Guid TargetId, bool Approved, string Comment, Guid ReviewerId, string Metadata)
        {
            this.TargetId = TargetId;
            this.Approved = Approved;
            this.Comment = Comment;
            this.ReviewerId = ReviewerId;
            this.Metadata = Metadata;
        }
    }
}
