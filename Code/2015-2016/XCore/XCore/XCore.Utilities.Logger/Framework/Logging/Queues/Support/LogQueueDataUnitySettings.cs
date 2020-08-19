using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XCore.Utilities.Infrastructure.Messaging.Queues.Repositories.Unity;

namespace XCore.Utilities.Logger.Framework.Logging.Queues.Support
{
    public class LogQueueDataUnitySettings : QueueDataUnitySettings
    {
        #region IQueueDataUnity

        public override string DBConnectionName { get; set; } = "Core.Components.LogQueue";
        public override string QueueTableName { get; set; } = "LogQueue";

        #endregion
    }
}
