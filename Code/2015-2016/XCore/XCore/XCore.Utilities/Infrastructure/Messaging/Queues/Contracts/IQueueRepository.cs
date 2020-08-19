using System.Collections.Generic;
using XCore.Utilities.Infrastructure.Entities.Repositories.Handlers;
using XCore.Utilities.Infrastructure.Messaging.Queues.Models;

namespace XCore.Utilities.Infrastructure.Messaging.Queues.Contracts
{
    public interface IQueueRepository : IRepository<MQMessage>
    {
        MQMessage GetNext();
        MQMessage GetNext( MQCriteria criteria );
        List<MQMessage> GetNext( MQCriteria criteria , int messagesCount );
        MQMessage Get( int Id );
        List<MQMessage> GetAll();
        int GetCount();
        bool Send( MQMessage message );
        bool Send( IList<MQMessage> messages );
    }
}
