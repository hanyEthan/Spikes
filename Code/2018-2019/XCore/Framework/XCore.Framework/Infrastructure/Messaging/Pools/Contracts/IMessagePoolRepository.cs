using System.Collections.Generic;
using XCore.Framework.Infrastructure.Entities.Repositories.Handlers;
using XCore.Framework.Infrastructure.Messaging.Pools.Models;

namespace XCore.Framework.Infrastructure.Messaging.Pools.Contracts
{
    public interface IMessagePoolRepository : IRepository<PoolMessage>
    {
        List<PoolMessage> Get( PoolMessageSearchCriteria filters );
        bool Hold( List<PoolMessage> Messages );
        bool Restore( List<string> ids );
        bool Delete( List<string> ids );
    }
}
