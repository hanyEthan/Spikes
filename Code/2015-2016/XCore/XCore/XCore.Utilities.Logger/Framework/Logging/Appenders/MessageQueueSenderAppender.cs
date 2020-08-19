using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using log4net.Appender;
using log4net.Core;
using XCore.Utilities.Infrastructure.Messaging.Queues.Handlers;
using XCore.Utilities.Infrastructure.Messaging.Queues.Models;
using XCore.Utilities.Logger.Framework.Logging.Mappers;
using XCore.Utilities.Logger.Framework.Logging.Queues;
using XCore.Utilities.Logger.Framework.Logging.Queues.Support;

namespace XCore.Utilities.Logger.Framework.Logging.Appenders
{
    public class MessageQueueSenderAppender : AppenderSkeleton
    {
        #region props.

        private static LogQueue Queue;

        #endregion
        #region cst.

        static MessageQueueSenderAppender()
        {
            var settings = new QueueHandler<LogQueueRepository , LogQueueDataUnitySettings>.LogMessageQueuingSettings()
            {
            };

            Queue = new LogQueue( null , settings );
        }

        #endregion

        #region AppenderSkeleton

        protected override void Append( LoggingEvent loggingEvent )
        {
            var message = VMappers.Logging.Map( loggingEvent );
            var QM = VMappers.Logging.Map( message );

            var response = Queue.Send( new List<MQMessage>() { QM } );
        }
        protected override void Append( LoggingEvent[] loggingEvents )
        {
            var messages = VMappers.Logging.Map( loggingEvents.ToList() );
            var QMs = VMappers.Logging.Map( messages );

            var response = Queue.Send( QMs );
        }

        #endregion
    }
}
