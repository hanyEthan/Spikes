using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XCore.Utilities.Infrastructure.Messaging.Queues.Models;
using XCore.Utilities.Infrastructure.Messaging.Queues.Models.Enums;
using XCore.Utilities.Logger.Framework.Logging.Contracts;
using XCore.Utilities.Logger.Framework.Logging.Handlers;
using XCore.Utilities.Logger.Framework.Logging.Models;
using XCore.Utilities.Utilities;

namespace XCore.Utilities.Logger.Framework.Logging.Queues
{
    public class LogQueueSubscriber : ILogQueueSubscriber
    {
        #region props.

        private static ILoggingHandler LoggingHandler;
        public const string TOKEN = "logging";

        #endregion
        #region cst.

        public LogQueueSubscriber()
        {
            LoggingHandler = new LoggingHandler();
        }
        
        #endregion
        #region ILoggingSubsriber

        public SubscriberStatus Handle( MQMessage message )
        {
            try
            {
                #region LOG
                NLogger.Trace( "Logging Subscriber started handling the message" );
                #endregion
                var logMessage = XSerialize.JSON.Deserialize<LogMessage>( message.MetaData );
                if ( logMessage == null || logMessage == default( LogMessage ) )
                {
                    #region LOG
                    NLogger.Error( "Failed to de-serialize message" );
                    #endregion
                    return SubscriberStatus.Failed;
                }
                var res = true;
                res = SaveLog( logMessage ) == SubscriberStatus.Succeed;

                #region LOG
                NLogger.Trace( "Message processing ended " + ( res ? "successfully" : "with errors" ) );
                #endregion
                return res ? SubscriberStatus.Succeed : SubscriberStatus.Failed;
            }
            catch ( Exception x )
            {
                NLogger.Error( string.Format( "Exception : LoggingSubsriber.Handle : " + x ) );
                return SubscriberStatus.Failed;
            }

        }
        public SubscriberStatus Handle( List<MQMessage> messages )
        {
            try
            {
                var res = true;
                res = SaveLog( messages.Select( x => XSerialize.JSON.Deserialize<LogMessage>( x.MetaData ) ).ToList() ) == SubscriberStatus.Succeed;

                return res ? SubscriberStatus.Succeed : SubscriberStatus.Failed;
            }
            catch ( Exception x )
            {
                return SubscriberStatus.Failed;
            }
        }
        public void PrepareSubscriber()
        {

        }
        public string GetSubscriberToken()
        {
            return TOKEN;
        }

        #endregion
        #region helpers.

        private SubscriberStatus SaveLog( LogMessage logMessage )
        {
            //Log to main database using Bulk Log procedure
            var result = LoggingHandler.AddLogs( new List<LogMessage>() { logMessage } );

            return result == true ? SubscriberStatus.Succeed : SubscriberStatus.Failed;
        }
        private SubscriberStatus SaveLog( List<LogMessage> logMessages )
        {
            //Log to main database using Bulk Log procedure
            var result = LoggingHandler.AddLogs( logMessages );

            return result == true ? SubscriberStatus.Succeed : SubscriberStatus.Failed;
        }

        #endregion
    }
}
