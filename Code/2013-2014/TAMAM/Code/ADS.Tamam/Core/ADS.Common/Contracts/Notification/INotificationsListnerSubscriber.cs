using ADS.Common.Models.Domain.Notification;

namespace ADS.Common.Contracts.Notification
{
    public interface INotificationsListnerSubscriber : IBaseHandler
    {
        void PrepareSubscriber();
        SubscriberStatus Send( NotificationMessage message );
        string Token { get; }
        SubscriberPriority Priority { get; }
    }
}