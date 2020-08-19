using System;
using ADS.Common.Contracts.Notification;
using ADS.Common.Utilities;
using ADS.Common.Models.Domain.Notification;

namespace ADS.Common.Handlers.Notification
{
    public class NotificationsSender : INotificationsSender
    {
        # region Properties

        public bool Initialized { get; private set; }

        public string Name
        {
            get { return "NotificationsSender"; }
        }

        private INotificationsSenderDataHandler notificationsSenderDataHandler;

        # endregion

        # region Constructor

        public NotificationsSender(INotificationsSenderDataHandler dataHandler)
        {
            XLogger.Info(string.Empty);

            try
            {
                notificationsSenderDataHandler = dataHandler;
                Initialized = notificationsSenderDataHandler != null && notificationsSenderDataHandler.Initialized;
            }
            catch (Exception x)
            {
                XLogger.Error("Exception: " + x);
            }
        }

        # endregion

        # region Publics

        public bool Notify(NotificationMessage message)
        {
            XLogger.Info(string.Empty);

            return notificationsSenderDataHandler.Notify(message);
        }

        public bool DeleteRawNotification(string targetId)
        {
            XLogger.Info(string.Empty);

            return notificationsSenderDataHandler.DeleteRawNotification(targetId);
        }

        # endregion
    }
}