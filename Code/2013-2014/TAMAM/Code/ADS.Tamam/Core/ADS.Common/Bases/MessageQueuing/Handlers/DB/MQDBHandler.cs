using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using ADS.Common.Bases.MessageQueuing.Contracts;
using ADS.Common.Bases.MessageQueuing.Handlers.DB.DataLayer;
using ADS.Common.Bases.MessageQueuing.Contracts;
using ADS.Common.Handlers;
using ADS.Common.Utilities;
using ADS.Common.Bases.MessageQueuing.Models;

namespace ADS.Common.Bases.MessageQueuing.Handlers.DB
{
    public class MQDBHandler<T> : IMQSender , IMQListener<T> where T : IMQMessageContent
    {
        #region props ...

        public string Name { get { return "MQDBHandler"; } }
        public bool Initialized { get; private set; }
        public MQListenerStatus Status { get; private set; }
        
        private TimeSpan _ListeningInterval = new TimeSpan( 0 , 0 , 10 );
        public TimeSpan ListeningInterval { get { return _ListeningInterval; } set { _ListeningInterval = value; } }

        public bool AutoDeleteRecievedMessages { get; set; }

        private MQDBDataLayer<T> _DataLayer { get; set; }
        private Task _ListeningTask;

        private bool _ListeningTaskCancellationToken;

        #endregion
        #region cst.

        public MQDBHandler()
        {
            try
            {
                XLogger.Trace( "" );

                this._ListeningTask = new Task( ListenForMessages );

                this._DataLayer = new MQDBDataLayer<T>();
                this.Initialized = this._DataLayer.Initialized;
                this.Status = MQListenerStatus.Idle;
            }
            catch ( Exception x )
            {
                Initialized = false;
                XLogger.Error( "Exception : " + x );
            }
        }
        public MQDBHandler( bool autoDeleteRecievedMessages , TimeSpan listeningInterval ) : this()
        {
            this.AutoDeleteRecievedMessages = autoDeleteRecievedMessages;
            this.ListeningInterval = listeningInterval;
        }
        public MQDBHandler( bool autoDeleteRecievedMessages ) : this()
        {
            this.AutoDeleteRecievedMessages = autoDeleteRecievedMessages;
        }
        public MQDBHandler( TimeSpan listeningInterval ) : this()
        {
            this.ListeningInterval = listeningInterval;
        }

        #endregion

        #region IMQListener

        public bool StartListening()
        {
            try
            {
                if ( !this.Initialized ) return false;
                if ( this.Status == MQListenerStatus.Listening ) return true;

                _ListeningTaskCancellationToken = false;
                _ListeningTask.Start();

                this.Status = MQListenerStatus.Listening;
                return true;
            }
            catch ( Exception x )
            {
                XLogger.Error( "Exception : " + x );
                return false;
            }
        }
        public bool StopListening()
        {
            if ( this.Status != MQListenerStatus.Listening ) return false;

            Status = MQListenerStatus.Stopping;
            return _ListeningTaskCancellationToken = true;
        }
        public event EventHandler<MQMessage> NewMessage;

        public MQMessage GetNext()
        {
            try
            {
                var message = Pop();
                return PostProcess( message );
            }
            catch ( Exception x )
            {
                XLogger.Error( "Exception : " + x );
                return null;
            }
        }
        public List<MQMessage> GetAll()
        {
            try
            {
                var messages = PopAll();
                return PostProcess( messages );
            }
            catch ( Exception x )
            {
                XLogger.Error( "Exception : " + x );
                return null;
            }
        }
        public MQMessage PeekNext()
        {
            try
            {
                return Pop();
            }
            catch ( Exception x )
            {
                XLogger.Error( "Exception : " + x );
                return null;
            }
        }
        public int PeekCount()
        {
            try
            {
                if ( !Initialized ) return -1;
                return _DataLayer.GetCount();
            }
            catch ( Exception x )
            {
                XLogger.Error( "Exception : " + x );
                return -1;
            }
        }
        public bool Delete( Guid messageId )
        {
            try
            {
                if ( !Initialized ) return false;
                return _DataLayer.Delete( messageId );
            }
            catch ( Exception x )
            {
                XLogger.Error( "Exception : " + x );
                return false;
            }
        }
        public bool Delete( List<Guid> messageIds )
        {
            try
            {
                if ( !Initialized ) return false;
                return _DataLayer.Delete( messageIds );
            }
            catch ( Exception x )
            {
                XLogger.Error( "Exception : " + x );
                return false;
            }
        }
        public bool Unlock( MQMessage message )
        {
            try
            {
                if ( !Initialized || message == null) return false;

                message.Status = MQMessageStatus.UnProcessed;
                return _DataLayer.Edit( message );
            }
            catch ( Exception x )
            {
                XLogger.Error( "Exception : " + x );
                return false;
            }
        }
        public bool Unlock( List<MQMessage> messages )
        {
            try
            {
                if ( !Initialized || messages == null || messages.Count == 0 ) return false;

                for ( int i = 0 ; i < messages.Count ; i++ ) messages[i].Status = MQMessageStatus.UnProcessed;
                return _DataLayer.Delete( messages );
            }
            catch ( Exception x )
            {
                XLogger.Error( "Exception : " + x );
                return false;
            }
        }

        #endregion
        #region IMQSender

        public bool Send( MQMessage message )
        {
            try
            {
                if ( !Initialized ) return false;
                return _DataLayer.Save( message );
            }
            catch ( Exception x )
            {
                XLogger.Error( "Exception : " + x );
                return false;
            }
        }

        #endregion

        #region Helpers

        private async void ListenForMessages()
        {
            while ( true )
            {
                CheckMassages();

                #region sleep ...

                if ( _ListeningTaskCancellationToken )    // listening cancelled ?
                {
                    Status = MQListenerStatus.Idle;
                    _ListeningTaskCancellationToken = false;
                    break;  
                }

                #region LOG
                XLogger.Info( "Sleeping for : [{0}]" , ListeningInterval.ToString() );
                #endregion
                await Task.Delay( ListeningInterval );
                
                #endregion
            }
        }
        private void CheckMassages()
        {
            try
            {
                XLogger.Trace( "" );

                // message ...
                var message = PostProcess( Pop() );
                if ( message == null )
                {
                    #region LOG
                    XLogger.Info( "MQ : no new messages found." );
                    #endregion
                    return;
                }

                // event ...
                #region LOG
                XLogger.Info( "MQ : new message found, will try to fire the event if any one subscribed ..." );
                #endregion

                if ( this.NewMessage != null ) this.NewMessage( this , message );
            }
            catch ( Exception x )
            {
                XLogger.Error( "Exception : " + x );
            }
        }

        private MQMessage Pop()
        {
            try
            {
                if ( !Initialized ) return null;
                return _DataLayer.GetNext();
            }
            catch ( Exception x )
            {
                XLogger.Error( "Exception : " + x );
                return null;
            }
        }
        private List<MQMessage> PopAll()
        {
            try
            {
                if ( !Initialized ) return null;
                return _DataLayer.GetAll();
            }
            catch ( Exception x )
            {
                XLogger.Error( "Exception : " + x );
                return null;
            }
        }
        private MQMessage PostProcess( MQMessage message )
        {
            if ( message == null ) return null;

            bool status = false;

            if ( AutoDeleteRecievedMessages )
            {
                status = _DataLayer.Delete( message.Id );              // delete
            }
            else
            {
                message.Status = MQMessageStatus.InProcess;
                status = _DataLayer.Edit( message );                   // edit
            }

            return status ? message : null;
        }
        private List<MQMessage> PostProcess( List<MQMessage> messages )
        {
            if ( messages == null || messages.Count == 0 ) return messages;

            bool status = false;

            if ( AutoDeleteRecievedMessages )
            {
                status = _DataLayer.Delete( messages );              // delete
            }
            else
            {
                for ( int i = 0 ; i < messages.Count ; i++ ) messages[i].Status = MQMessageStatus.InProcess;
                status = _DataLayer.Edit( messages );                   // edit
            }

            return status ? messages : null;
        }

        #endregion
    }
}
