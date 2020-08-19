using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XCore.Utilities.Infrastructure.Messaging.Queues.Models;
using XCore.Utilities.Infrastructure.Messaging.Queues.Models.Enums;

namespace XCore.Utilities.Logger.Framework.Logging.Contracts
{
    public interface ILogQueueSubscriber
    {
        SubscriberStatus Handle( MQMessage message );
        SubscriberStatus Handle( List<MQMessage> messages );
        string GetSubscriberToken();
        void PrepareSubscriber();
    }
}
