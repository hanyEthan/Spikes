using XCore.Framework.Infrastructure.Messaging.Queues.Models;
using XCore.Framework.Infrastructure.Messaging.Queues.Models.Enums;

namespace XCore.Framework.Infrastructure.Messaging.Queues.Contracts
{
    public interface IQueueSubscriber
    {
        SubscriberStatus Handle( MQMessage message );
        string GetSubscriberToken();
        void PrepareSubscriber();
    }
}
