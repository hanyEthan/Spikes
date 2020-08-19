using System;
using System.Linq;
using System.Collections.Generic;
using XCore.Utilities.Infrastructure.Messaging.Queues.Models;
using XCore.Utilities.Infrastructure.Messaging.Queues.Models.Enums;

namespace XCore.Utilities.Infrastructure.Messaging.Queues.Contracts
{
    public interface IQueueListener
    {
        bool Initialized { get; }
        event EventHandler<List<MQMessage>> NewMessages;

        MQListenerStatus Status { get; }
        bool AutoDeleteRecievedMessages { get; set; }
        TimeSpan ListeningInterval { get; set; }
        MQMessage GetNext();
        List<MQMessage> GetAll();
        int PeekCount();
        void Delete( int messageId );
        void Delete( List<int> messagesId );
        bool StartListening();
        bool StopListening();
    }
}
