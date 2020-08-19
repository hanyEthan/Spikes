using System.Collections.Generic;
using XCore.Utilities.Infrastructure.Messaging.Pools.Models;
using XCore.Utilities.Infrastructure.Entities.Repositories.Handlers;

namespace XCore.Utilities.Infrastructure.Messaging.Pools.Contracts
{
    public interface IMessagePoolRepository : IRepository<PoolMessage>
    {
        List<PoolMessage> Get( PoolMessageSearchCriteria filters );
        bool Hold( List<PoolMessage> Messages );
        bool Restore( List<string> ids );
        bool Delete( List<string> ids );
    }
}
