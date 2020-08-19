using XCore.Utilities.Infrastructure.Messaging.Queues.Models;
using XCore.Utilities.Infrastructure.Messaging.Queues.Models.Enums;

namespace XCore.Utilities.Infrastructure.Messaging.Queues.Contracts
{
    public interface IQueueSubscriber
    {
        SubscriberStatus Handle( MQMessage message );
        string GetSubscriberToken();
        void PrepareSubscriber();
    }
}
