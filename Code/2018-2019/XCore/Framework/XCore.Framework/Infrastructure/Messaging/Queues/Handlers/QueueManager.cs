using System;
using System.Collections.Generic;
using System.Linq;
using XCore.Framework.Infrastructure.Messaging.Queues.Contracts;
using XCore.Framework.Infrastructure.Messaging.Queues.Models;
using XCore.Framework.Infrastructure.Messaging.Queues.Models.Enums;
using XCore.Framework.Utilities;

namespace XCore.Framework.Infrastructure.Messaging.Queues.Handlers
{
    public class QueueManager : IQueueManager
    {
        #region props.

        public bool initialized { get; private set; }

        private IQueueListener MQListener;
        private IQueueSender MQSender;
        private List<IQueueSubscriber> Subscribers;
        private Dictionary<string , SubscriberStatus> Status;   // TODO Need To Be Enhanced - add subscriber ids to new column in message , separated

        #endregion
        #region cst.
        public QueueManager( MQCriteria criteria )
        {
            try
            {
                var handler = new QueueHandler( criteria );
                MQListener = handler;
                MQSender = handler;

                Subscribers = new List<IQueueSubscriber>();
                MQListener.NewMessages += MQListener_NewMessages; ;

                initialized = MQListener.Initialized;
            }
            catch ( Exception ex )
            {
                initialized = false;
                XLogger.Error( string.Format( "Exception : {0}" , ex ) );
            }
        }

        #endregion

        #region IQueueManager

        public bool Send( MQMessage message )
        {
            return MQSender.Send( message );
        }
        public void startListening()
        {
            MQListener.StartListening();
        }
        public void stopListening()
        {
            MQListener.StopListening();
        }
        public void AddSubscriber( IQueueSubscriber subscriber )
        {
            Subscribers.Add( subscriber );
        }
        public void AddSubscribers( List<IQueueSubscriber> subscribers )
        {
            Subscribers.AddRange( subscribers );
        }

        #endregion

        #region ListeningEvent

        private void MQListener_NewMessages( Object sender , List<MQMessage> messages )
        {
            if ( Subscribers != null && Subscribers.Count > 0 )
            {
                if ( messages == null || messages.Count == 0 )
                {
                    XLogger.Info( "Queue  Message is null" );
                    return;
                }

                foreach ( var message in messages )
                {
                    if ( string.IsNullOrWhiteSpace( message.SubscribersTokens ) )
                    {
                        XLogger.Info( "Queue  Message SubscribersTokens is null" );
                        ResendMessage( message , MQMessageStatus.DataError );
                        return;
                    }
                    var SubscribersTokens = XSerialize.JSON.Deserialize<List<string>>( message.SubscribersTokens );
                    if ( SubscribersTokens == null || SubscribersTokens.Count == 0 )
                    {
                        XLogger.Info( "Queue  Message SubscribersTokens is null" );
                        ResendMessage( message , MQMessageStatus.DataError );
                        return;
                    }
                    Status = new Dictionary<string , SubscriberStatus>();
                    foreach ( var token in SubscribersTokens )
                    {
                        var subscriber = Subscribers.FirstOrDefault( s => s.GetSubscriberToken() == token );
                        if ( subscriber != null )
                        {
                            Status.Add( subscriber.GetSubscriberToken() , subscriber.Handle( message ) );
                        }
                    }
                    // check if all subscribers available to handle this message or not before deleting 
                    var isAllSubscribersHandled = new HashSet<string>( SubscribersTokens ).Except( Subscribers.Select( s => s.GetSubscriberToken() ).ToList() ).Count() == 0;
                    var delete = !Status.Any( s => s.Value == SubscriberStatus.Failed ) && isAllSubscribersHandled;
                    if ( delete )
                    {
                        MQListener.Delete( message.Id );
                    }
                    else
                    {
                        var succeeded = Status.Where( s => s.Value == SubscriberStatus.Succeed ).Select( s => s.Key ).ToList();
                        if ( succeeded != null && succeeded.Count > 0 )
                            SubscribersTokens.RemoveAll( sb => succeeded.Any( s => s == sb ) );
                        message.SubscribersTokens = XSerialize.JSON.Serialize( SubscribersTokens );
                        ResendMessage( message , MQMessageStatus.SubscriberFailed );
                    }
                }
            }
        }
        private void ResendMessage( MQMessage message , MQMessageStatus status )
        {
            message.RetrialsCounter++;
            message.Status = status;
            message.CreatedDate = DateTime.UtcNow;
            MQSender.Resend( message );
        }

        #endregion
    }
}
