using XCore.Framework.Infrastructure.Messaging.Queues.Contracts;

namespace XCore.Framework.Infrastructure.Messaging.Queues.Repositories.Unity
{
    public class QueueDataUnitySettings : IQueueDataUnitySettings
    {
        #region IQueueDataUnity

        public virtual string DBConnectionName { get; set; } = "XCore.DB";
        public virtual string QueueTableName { get; set; } = "MQMessage";

        #endregion
    }
}
