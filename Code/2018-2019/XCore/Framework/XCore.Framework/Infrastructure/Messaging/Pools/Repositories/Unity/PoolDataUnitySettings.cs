using XCore.Framework.Infrastructure.Messaging.Pools.Contracts;

namespace XCore.Framework.Infrastructure.Messaging.Pools.Repositories.Unity
{
    public class PoolDataUnitySettings : IPoolDataUnitySettings
    {
        #region IQueueDataUnity

        public virtual string DBConnectionName { get; set; } = "XCore.DB";
        public virtual string QueueTableName { get; set; } = "MessagesPool";

        #endregion
    }
}
