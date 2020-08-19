using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XCore.Utilities.Infrastructure.Messaging.Queues.Models;

namespace XCore.Utilities.Infrastructure.Messaging.Queues.Contracts
{
    public interface IQueueSender
    {
        bool Initialized { get; }
        bool Send( MQMessage message );
        bool Send( List<MQMessage> messages );
        bool Resend( MQMessage message );
        bool Resend( List<MQMessage> messages );
    }
}
