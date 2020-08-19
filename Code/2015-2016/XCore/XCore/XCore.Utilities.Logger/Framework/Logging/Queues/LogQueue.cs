using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XCore.Utilities.Infrastructure.Messaging.Queues.Handlers;
using XCore.Utilities.Infrastructure.Messaging.Queues.Models;
using XCore.Utilities.Logger.Framework.Logging.Queues.Support;

namespace XCore.Utilities.Logger.Framework.Logging.Queues
{
    public class LogQueue : QueueHandler<LogQueueRepository , LogQueueDataUnitySettings>
    {
        #region cst.

        public LogQueue( MQCriteria _criteria ) : base( _criteria )
        {
        }
        public LogQueue( MQCriteria _criteria , LogMessageQueuingSettings settings ) : base( _criteria, settings )
        {
        }

        #endregion
    }
}
