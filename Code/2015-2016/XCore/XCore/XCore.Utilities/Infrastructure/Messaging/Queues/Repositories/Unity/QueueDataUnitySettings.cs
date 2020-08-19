using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XCore.Utilities.Infrastructure.Messaging.Queues.Contracts;

namespace XCore.Utilities.Infrastructure.Messaging.Queues.Repositories.Unity
{
    public class QueueDataUnitySettings : IQueueDataUnitySettings
    {
        #region IQueueDataUnity

        public virtual string DBConnectionName { get; set; } = "XCore.DB";
        public virtual string QueueTableName { get; set; } = "MQMessage";

        #endregion
    }
}
