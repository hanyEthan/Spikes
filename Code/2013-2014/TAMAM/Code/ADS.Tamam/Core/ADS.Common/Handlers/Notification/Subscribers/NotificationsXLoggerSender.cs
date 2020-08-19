using System;

using ADS.Common.Contracts.Notification;
using ADS.Common.Models.Domain.Notification;
using ADS.Common.Utilities;

namespace ADS.Common.Handlers.Notification
{
    public class NotificationsXLoggerSender : INotificationsListnerSubscriber
    {
        # region Properties

        public bool Initialized { get; private set; }
        public string Name { get { return "LoggerSubscriber"; } }

        # endregion

        # region Constructor

        public NotificationsXLoggerSender()
        {
            XLogger.Trace( "" );
            try
            {
                Initialized = true;
            }
            catch ( Exception x )
            {
                XLogger.Error( "Exception: " + x );
            }
        }

        # endregion

        # region Publics

        public SubscriberStatus Send( NotificationMessage message )
        {
            try
            {
                var saved = XLogger.Info( string.Format( "{0}  -  {1}  -  {2}  -  {3}" , message.Id , message.Message , message.CreationTime , message.ActionName ) );
                return saved ? SubscriberStatus.Succeed : SubscriberStatus.Failed;
            }
            catch ( Exception x )
            {
                XLogger.Error( "Exception : " + x );
                return SubscriberStatus.Failed;
            }

        }
        public void PrepareSubscriber() { }

        public string Token { get { return NotificationSubscribersTokens.XLoggerSubscriber; } }
        public SubscriberPriority Priority { get { return SubscriberPriority.Low; } }

        # endregion
    }
}