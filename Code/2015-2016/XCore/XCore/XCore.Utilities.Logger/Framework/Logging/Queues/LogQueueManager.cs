using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XCore.Utilities.Infrastructure.Messaging.Queues.Contracts;
using XCore.Utilities.Infrastructure.Messaging.Queues.Models;
using XCore.Utilities.Infrastructure.Messaging.Queues.Models.Enums;
using XCore.Utilities.Logger.Framework.Logging.Contracts;
using XCore.Utilities.Utilities;
using QueueSettings = XCore.Utilities.Infrastructure.Messaging.Queues.Handlers.QueueHandler<XCore.Utilities.Logger.Framework.Logging.Queues.Support.LogQueueRepository , XCore.Utilities.Logger.Framework.Logging.Queues.Support.LogQueueDataUnitySettings>.LogMessageQueuingSettings;

namespace XCore.Utilities.Logger.Framework.Logging.Queues
{
    public class LogQueueManager : ILogQueueManager
    {
        #region props.

        public bool initialized { get; private set; }
        private IQueueListener QListener;
        private IQueueSender   QSender;
        private List<ILogQueueSubscriber> Subscribers;
        private Dictionary<string , SubscriberStatus> Status;   // TODO Need To Be Enhanced - add subscriber ids to new column in message , separated

        #endregion
        #region cst.

        public LogQueueManager( MQCriteria criteria , QueueSettings settings )
        {
            try
            {
                QListener = new LogQueue( criteria , settings );
                QSender = new LogQueue( criteria , settings );
                Subscribers = new List<ILogQueueSubscriber>();
                QListener.NewMessages += MQListener_NewMessage;

                initialized = QListener.Initialized;
            }
            catch ( Exception x )
            {
                initialized = false;
                NLogger.Error( string.Format( "Exception : " + x ) );
            }
        }

        #endregion

        #region ListeningEvent

        private void MQListener_NewMessage( Object sender , List<MQMessage> messages )
        {
            if ( Subscribers != null && Subscribers.Count > 0 )
            {
                if ( messages == null || messages.Count == 0 )
                {
                    NLogger.Trace( string.Format( "NotificationListener - Queue  Message is null " ) );
                    return;
                }
                if ( string.IsNullOrWhiteSpace( messages[0].SubscribersTokens ) )
                {
                    NLogger.Trace( string.Format( "NotificationListener - Queue  Message SubscribersTokens is null " ) );
                    ResendMessage( messages , MQMessageStatus.DataError );
                    return;
                }
                var SubscribersTokens = XSerialize.JSON.Deserialize<List<string>>( messages[0].SubscribersTokens );
                if ( SubscribersTokens == null || SubscribersTokens.Count == 0 )
                {
                    NLogger.Trace( string.Format( "NotificationListener - Queue  Message SubscribersTokens is null " ) );
                    ResendMessage( messages , MQMessageStatus.DataError );
                    return;
                }
                Status = new Dictionary<string , SubscriberStatus>();
                foreach ( var token in SubscribersTokens )
                {
                    var subscriber = Subscribers.FirstOrDefault( s => s.GetSubscriberToken() == token );
                    if ( subscriber != null )
                    {
                        Status.Add( subscriber.GetSubscriberToken() , subscriber.Handle( messages ) );
                    }
                }
                // check if all subscribers available to handle this message or not before deleting 
                var isAllSubscribersHandled = new HashSet<string>( SubscribersTokens ).Except( Subscribers.Select( s => s.GetSubscriberToken() ).ToList() ).Count() == 0;
                var delete = !Status.Any( s => s.Value == SubscriberStatus.Failed ) && isAllSubscribersHandled;
                if ( delete )
                {
                    QListener.Delete( messages.Select( x => x.Id ).ToList() );
                }
                else
                {
                    var succeeded = Status.Where( s => s.Value == SubscriberStatus.Succeed ).Select( s => s.Key ).ToList();
                    if ( succeeded != null && succeeded.Count > 0 )
                        SubscribersTokens.RemoveAll( sb => succeeded.Any( s => s == sb ) );
                    //message.SubscribersTokens = XSerialize.Serialize(SubscribersTokens);
                    ResendMessage( messages , MQMessageStatus.SubscriberFailed );
                }

            }
        }
        private void ResendMessage( List<MQMessage> messages , MQMessageStatus status )
        {
            foreach ( var message in messages )
            {
                message.RetrialsCounter++;
                message.Status = status;
                message.CreatedDate = DateTime.UtcNow;
            }
            QSender.Resend( messages );
        }

        #endregion
        #region ILogQueueManager

        public void startListening()
        {
            QListener.StartListening();
        }
        public void stopListening()
        {
            QListener.StopListening();
        }
        public void AddSubscriber( ILogQueueSubscriber subscriber )
        {
            Subscribers.Add( subscriber );
        }
        public void AddSubscribers( List<ILogQueueSubscriber> subscribers )
        {
            Subscribers.AddRange( subscribers );
        }
        
        #endregion
    }
}
