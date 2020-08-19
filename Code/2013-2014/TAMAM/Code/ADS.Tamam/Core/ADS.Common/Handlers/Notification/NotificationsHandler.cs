using System;
using System.Linq;
using System.Collections.Generic;

using ADS.Common.Utilities;
using ADS.Common.Contracts.Notification;
using ADS.Common.Models.Domain.Notification;

namespace ADS.Common.Handlers.Notification
{
    public class NotificationsHandler : INotificationsHandler
    {
        # region Properties

        public bool Initialized { get; private set; }

        public string Name
        {
            get { return "NotificationsHandler"; }
        }

        private INotificationsDataHandler notificationsDataHandler;
        private List<INotificationTypeHandler> notificationTypeHandlers;

        # endregion

        # region Constructor

        public NotificationsHandler( INotificationsDataHandler dataHandler , List<INotificationTypeHandler> typeHandlers )
        {
            XLogger.Info( string.Empty );

            try
            {
                notificationsDataHandler = dataHandler;
                notificationTypeHandlers = typeHandlers;
                Initialized = dataHandler != null && dataHandler.Initialized &&
                              typeHandlers.Count( x => x.Initialized == false ) == 0;
            }
            catch ( Exception x )
            {
                XLogger.Error( "Exception: " + x );
            }
        }

        # endregion

        # region Publics

        public bool Notify( NotificationMessage message , INotificationPolicy policy )
        {
            if ( policy != null ) policy.ProcessNotification( message );
            return Broker.NotificationsSender.Notify( message );
        }
        public bool Notify( NotificationMessage message )
        {
            return Notify( message , null );
        }
        public bool DeleteRawNotification( string targetId )
        {
            return Broker.NotificationsSender.DeleteRawNotification( targetId );
        }

        public NotificationDetailedMessage GetNotification( Guid id )
        {
            XLogger.Info( string.Empty );

            return notificationsDataHandler.GetNotification( id );
        }
        public NotificationDetailedMessage GetNotification( Guid id , string personId )
        {
            XLogger.Info( string.Empty );
            return notificationsDataHandler.GetNotification( id , personId );
        }
        public NotificationDetailedMessage GetNotification( string code , string personId )
        {
            XLogger.Info( string.Empty );
            return notificationsDataHandler.GetNotification( code , personId );
        }
        public List<NotificationDetailedMessage> GetNotificationsByPerson( string personId )
        {
            XLogger.Info( string.Empty );

            return notificationsDataHandler.GetNotificationsByPerson( personId );
        }
        public List<NotificationDetailedMessage> GetNotificationsByTarget( string targetId )
        {
            XLogger.Info( string.Empty );

            return notificationsDataHandler.GetNotificationsByTarget( targetId );
        }
        public int GetPendingNotificationsCountByPerson( string personId )
        {
            XLogger.Info( string.Empty );

            return notificationsDataHandler.GetPendingNotificationsCountByPerson( personId );
        }
        public bool MarkNotificationAsRead( Guid notificationId )
        {
            XLogger.Info( string.Empty );

            return notificationsDataHandler.MarkNotificationAsRead( notificationId );
        }

        public bool MarkNotificationAsHandled( Guid notificationId )
        {
            XLogger.Info( "" );

            return notificationsDataHandler.MarkNotificationAsHandled( notificationId );
        }
        public bool DeleteDetailedNotification( Guid id )
        {
            XLogger.Info( "" );

            return notificationsDataHandler.DeleteDetailedNotification( id );
        }
        public bool MarkNotificationAsHandledByTarget( string targetId )
        {
            XLogger.Info( string.Empty );
            return notificationsDataHandler.MarkNotificationAsHandledByTarget( targetId );
        }

        public bool Handle( INotificationTypeMetadata metadata )
        {
            XLogger.Info( string.Empty );

            INotificationTypeHandler handler = notificationTypeHandlers.FirstOrDefault( x => x.Metadata == metadata.Metadata );
            if ( handler == null || handler.Initialized == false ) return false;
            return handler.Handle( metadata );

            return false;
        }

        public string GetAssociatedDetails( Guid targetId , string metadata )
        {
            XLogger.Info( string.Empty );

            INotificationTypeHandler handler = notificationTypeHandlers.FirstOrDefault( x => x.Metadata == metadata );
            if ( handler == null || handler.Initialized == false ) return string.Empty;
            return handler.GetAssociatedDetails( targetId );
        }

        # endregion
    }
}