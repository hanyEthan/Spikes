using System.Collections.Generic;
using XCore.Framework.Infrastructure.Entities.Repositories.Handlers;
using XCore.Framework.Infrastructure.Messaging.Queues.Models;

namespace XCore.Framework.Infrastructure.Messaging.Queues.Contracts
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
