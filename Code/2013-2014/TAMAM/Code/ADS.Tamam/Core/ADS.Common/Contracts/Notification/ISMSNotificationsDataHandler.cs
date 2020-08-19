using ADS.Common.Models.Domain;

namespace ADS.Common.Contracts.Notification
{
    public interface ISMSNotificationsDataHandler
    {
        bool CreateMessage( SMSMessage message);
    }
}
