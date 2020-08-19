using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using ADS.Common.Contracts.Notification;
using ADS.Common.Models.Domain.Notification;
using ADS.Common.Utilities;

namespace ADS.Common.Handlers.Notification
{
    public class NotificationsListner : INotificationsListner
    {
        # region Properties

        public bool Initialized { get; private set; }
        public string Name { get { return "NotificationsListner"; } }
        public List<INotificationsListnerSubscriber> Subscribers { get; set; }

        private INotificationsListnerDataHandler notificationsListnerDataHandler;
        private Task _MainTask;

        # endregion
        # region Constructor

        public NotificationsListner( INotificationsListnerDataHandler dataHandler )
        {
            XLogger.Info( string.Empty );

            try
            {
                notificationsListnerDataHandler = dataHandler;
                Initialized = notificationsListnerDataHandler != null && notificationsListnerDataHandler.Initialized;

                Subscribers = new List<INotificationsListnerSubscriber>();
                _MainTask = new Task( Process );
            }
            catch ( Exception x )
            {
                XLogger.Error( "Exception : " + x );
            }
        }

        # endregion

        #region Internals

        internal void CheckNotifications()
        {
            XLogger.Trace( "" );

            try
            {
                if ( Subscribers == null || Subscribers.Count == 0 )
                {
                    XLogger.Warning( "Notifications Subscribers is not loaded correctly.." );
                    return;
                }

                // Sort Subscribers according to their Priority (High -> Medium -> Low)..
                Subscribers = Subscribers.OrderBy( x => x.Priority ).ToList();

                var messages = notificationsListnerDataHandler.GetNextNotifications().Where( x => string.IsNullOrEmpty( x.DelayTime ) || x.CreationTime.Add( TimeSpan.Parse( x.DelayTime ) ) <= DateTime.Now ).ToList();
                if ( messages == null || messages.Count == 0 ) return;   // nothing recieved.

                foreach ( var nextMessage in messages )
                {
                    var subscriberResponses = new List<SubscriberStatus>();

                    foreach ( var subscriber in Subscribers )
                    {
                        if ( !string.IsNullOrEmpty( nextMessage.SubscribersTokens ) && !nextMessage.SubscribersTokens.Contains( subscriber.Token ) ) continue;

                        var status = subscriber.Send( nextMessage );
                        subscriberResponses.Add( status );

                        if ( status == SubscriberStatus.Failed ) break;
                    }

                    // Check if RAW message ready to be deleted..
                    var isReady = IsReadyToBeDeleted( subscriberResponses );
                    if ( isReady ) notificationsListnerDataHandler.DeleteNotification( nextMessage.Id );
                }
            }
            catch ( Exception x )
            {
                XLogger.Error( "Exception : " , x );
            }
        }
        private async void Process()
        {
            // prepare subscribers..
            if ( Subscribers != null && Subscribers.Count > 0 ) { foreach ( var S in Subscribers ) S.PrepareSubscriber(); }

            var interval = GetWorkingInterval();
            while ( true )
            {
                try
                {
                    XLogger.Info( "Main Task Started." );

                    CheckNotifications();
                }
                catch ( Exception ex )
                {
                    XLogger.Error( string.Empty , ex );
                }

                XLogger.Info( "Main Task: Delay for: [{0}]" , interval.ToString() );
                await Task.Delay( interval );
            }
        }
        private TimeSpan GetWorkingInterval()
        {
            try
            {
                string interval = Broker.ConfigurationHandler.GetValue( Constants.SectionBroker , Constants.NotificationsListenerInterval );
                return TimeSpan.Parse( interval );
            }
            catch ( Exception x )
            {
                XLogger.Error( "Configuration Error : Exeption : " + x );
                throw;
            }
        }
        private bool IsReadyToBeDeleted( List<SubscriberStatus> statuses )
        {
            return statuses.Count( x => x == SubscriberStatus.Failed ) == 0;
        }

        #endregion

        # region Publics

        public bool StartListening()
        {
            XLogger.Info( string.Empty );
            try
            {
                _MainTask.Start();

                return true;
            }
            catch ( Exception x )
            {
                XLogger.Error( "Exception : " , x );
                return false;
            }
        }

        # endregion
    }
}