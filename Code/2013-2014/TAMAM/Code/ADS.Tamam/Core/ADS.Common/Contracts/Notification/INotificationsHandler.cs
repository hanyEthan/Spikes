using System;
using System.Collections.Generic;
using ADS.Common.Models.Domain.Notification;

namespace ADS.Common.Contracts.Notification
{
    public interface INotificationsHandler : IBaseHandler
    {
        bool Notify( NotificationMessage message , INotificationPolicy policy );
        bool Notify( NotificationMessage message );
        bool DeleteRawNotification( string targetId ); 

        NotificationDetailedMessage GetNotification( Guid id );
        NotificationDetailedMessage GetNotification( Guid id , string personId );
        NotificationDetailedMessage GetNotification( string code , string personId );

        bool Handle(INotificationTypeMetadata metadata);
        string GetAssociatedDetails( Guid targetId, string metadata );

        List<NotificationDetailedMessage> GetNotificationsByPerson(string personId);
        List<NotificationDetailedMessage> GetNotificationsByTarget(string targetId);
        int GetPendingNotificationsCountByPerson( string personId );

        bool MarkNotificationAsRead(Guid id);
        bool MarkNotificationAsHandled(Guid id);
        bool DeleteDetailedNotification( Guid id );
        bool MarkNotificationAsHandledByTarget( string targetId );
    }
}