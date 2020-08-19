using XCore.Utilities.Infrastructure.Messaging.Pools.Models;

namespace XCore.Utilities.Infrastructure.Messaging.Pools.Contracts
{
    public interface IMessagePoolSender
    {
        int Create( PoolMessage message );
    }
}
