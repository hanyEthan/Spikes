using XCore.Framework.Infrastructure.Messaging.Pools.Models;

namespace XCore.Framework.Infrastructure.Messaging.Pools.Contracts
{
    public interface IMessagePoolSender
    {
        int Create( PoolMessage message );
    }
}
