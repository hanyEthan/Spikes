using System;
using System.Collections.Generic;

using ADS.Common.Contracts;
using ADS.Common.Bases.MessageQueuing.Contracts;
using ADS.Common.Bases.MessageQueuing.Models;

namespace ADS.Common.Bases.MessageQueuing.Contracts
{
    public interface IMQListener<T> : IBaseHandler where T : IMQMessageContent
    {
        MQListenerStatus Status { get; }

        bool AutoDeleteRecievedMessages { get; set; }
        TimeSpan ListeningInterval { get; set; }
        event EventHandler<MQMessage> NewMessage;

        MQMessage GetNext();
        List<MQMessage> GetAll();
        MQMessage PeekNext();
        int PeekCount();
        bool Delete( Guid messageId );

        bool StartListening();
        bool StopListening();
    }
}
