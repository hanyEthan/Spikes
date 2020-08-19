using System;
using System.Collections.Generic;
using ADS.Common.Models.Domain.Notification;

namespace ADS.Common.Contracts.Notification
{
    public interface INotificationsListnerDataHandler : IBaseHandler
    {
        bool DeleteNotification( Guid id );
        NotificationMessage GetNextNotification();
        List<NotificationMessage> GetNextNotifications();
    }
}