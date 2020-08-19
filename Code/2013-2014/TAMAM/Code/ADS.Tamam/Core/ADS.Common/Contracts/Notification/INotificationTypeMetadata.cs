using System;

namespace ADS.Common.Contracts.Notification
{
    public interface INotificationTypeMetadata
    {
        Guid TargetId { get; set; }
        bool Approved { get; set; }
        string Comment { get; set; }
        Guid ReviewerId { get; set; }

        string Metadata { get; set; }
    }
}