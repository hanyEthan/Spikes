using ADS.Common.Models.Domain;
using ADS.Common.Models.Domain.Notification;

namespace ADS.Common.Contracts.Notification
{
    public interface INotificationsListSenderDataHandler : IBaseHandler
    {
        bool Save( NotificationDetailedMessage message );
    }
}
