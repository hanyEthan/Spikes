using System.Collections.Generic;
using XCore.Framework.Infrastructure.Messaging.Pools.Models;

namespace XCore.Framework.Infrastructure.Messaging.Pools.Contracts
{
    public interface IMessagePoolReader
    {
        List<PoolMessage> Get( PoolMessageSearchCriteria filters );
        bool Restore( List<string> ids );
        bool Delete( List<string> ids );
    }
}
