using ADS.Common.Models.Domain.Notification;

namespace ADS.Common.Contracts.Notification
{
    public interface INotificationsSender : IBaseHandler
    {
        bool Notify(NotificationMessage message);
        bool DeleteRawNotification( string targetId );
    }
}