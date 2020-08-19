using System;

namespace ADS.Common.Contracts.Notification
{
    public interface INotificationTypeHandler : IBaseHandler
    {
        bool Handle(INotificationTypeMetadata metadata);
        string GetAssociatedDetails( Guid targetId );
        string Metadata { get; }
    }
}
