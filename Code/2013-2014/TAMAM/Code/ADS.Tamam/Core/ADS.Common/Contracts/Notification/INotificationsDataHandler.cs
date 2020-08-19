using System;
using System.Collections.Generic;
using ADS.Common.Models.Domain;
using ADS.Common.Models.Domain.Notification;

namespace ADS.Common.Contracts.Notification
{
    public interface INotificationsDataHandler : IBaseHandler
    {
        NotificationDetailedMessage GetNotification( Guid id );
        NotificationDetailedMessage GetNotification( Guid id , string personId );
        NotificationDetailedMessage GetNotification( string code , string personId );

        List<NotificationDetailedMessage> GetNotificationsByPerson( string personId );
        List<NotificationDetailedMessage> GetNotificationsByTarget( string targetId );
        int GetPendingNotificationsCountByPerson( string personId );
        
        bool MarkNotificationAsRead( Guid id );
        bool MarkNotificationAsHandled( Guid id );
        bool DeleteDetailedNotification( Guid id );
        bool MarkNotificationAsHandledByTarget( string targetId );
    }
}
