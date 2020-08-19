using System.Collections.Generic;
using XCore.Framework.Infrastructure.Messaging.Queues.Models;

namespace XCore.Framework.Infrastructure.Messaging.Queues.Contracts
{
    public interface IQueueManager
    {
        bool initialized { get; }
        bool Send( MQMessage message );
        void startListening();
        void stopListening();
        void AddSubscriber( IQueueSubscriber subscriber );
        void AddSubscribers( List<IQueueSubscriber> subscribers );
    }
}
