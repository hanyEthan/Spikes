using System.Collections.Generic;

namespace ADS.Common.Contracts.Notification
{
    public interface INotificationsListner : IBaseHandler
    {
        List<INotificationsListnerSubscriber> Subscribers { get; set; }
        bool StartListening();
    }
}