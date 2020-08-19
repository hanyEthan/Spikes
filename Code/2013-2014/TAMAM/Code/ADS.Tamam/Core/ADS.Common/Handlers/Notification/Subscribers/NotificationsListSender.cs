using System;

using ADS.Common.Contracts.Notification;
using ADS.Common.Handlers.Data;
using ADS.Common.Models.Domain.Notification;
using ADS.Common.Utilities;

namespace ADS.Common.Handlers.Notification
{
    public class NotificationsListSender : INotificationsListnerSubscriber
    {
        # region Properties

        public bool Initialized { get; private set; }
        public string Name
        {
            get { return "NotificationsListSender"; }
        }

        private INotificationsListSenderDataHandler _DataHandler;

        # endregion
        # region Constructor

        public NotificationsListSender()
        {
            XLogger.Info( "" );

            try
            {
                _DataHandler = new DataHandler();
                Initialized = _DataHandler != null && _DataHandler.Initialized;
            }
            catch ( Exception x )
            {
                XLogger.Error( "Exception : " + x );
            }
        }

        # endregion
        # region Publics

        public SubscriberStatus Send( NotificationMessage message )
        {
            XLogger.Info( "" );

            try
            {
                var notification = message.GetDetailedMessage();
                var saved = _DataHandler.Save( notification );
                return saved ? SubscriberStatus.Succeed : SubscriberStatus.Failed;
            }
            catch ( Exception x )
            {
                XLogger.Error( "Exception : " + x );
                return SubscriberStatus.Failed;
            }
        }
        public void PrepareSubscriber() { }

        public string Token { get { return NotificationSubscribersTokens.TamamSubscriber; } }
        public SubscriberPriority Priority { get { return SubscriberPriority.Medium; } }

        # endregion
    }
}