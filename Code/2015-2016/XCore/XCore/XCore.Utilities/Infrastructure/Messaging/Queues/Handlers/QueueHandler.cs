using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XCore.Utilities.Infrastructure.Messaging.Queues.Contracts;
using XCore.Utilities.Infrastructure.Messaging.Queues.Models;
using XCore.Utilities.Infrastructure.Messaging.Queues.Models.Enums;
using XCore.Utilities.Infrastructure.Messaging.Queues.Repositories;
using XCore.Utilities.Infrastructure.Messaging.Queues.Repositories.Unity;
using XCore.Utilities.Utilities;

namespace XCore.Utilities.Infrastructure.Messaging.Queues.Handlers
{
    #region QueueHandler

    public class QueueHandler : QueueHandler<QueueRepository, QueueDataUnitySettings>
    {
        #region cst.

        public QueueHandler( MQCriteria _criteria ) : base( _criteria )
        {
        }
        public QueueHandler( MQCriteria _criteria , LogMessageQueuingSettings settings ) : base( _criteria, settings )
        {
        }

        #endregion
    }

    #endregion
    #region QueueHandler<TRepo, TSettings>

    public class QueueHandler<TRepo, TSettings> : IQueueSender, IQueueListener where TRepo : IQueueRepository
                                                                               where TSettings : IQueueDataUnitySettings, new()
    {
        #region props.

        public bool Initialized { get; private set; }

        private bool _ListeningTaskCancellationToken;
        private Task _ListeningTask;
        private MQCriteria criteria;

        private TimeSpan _ListeningInterval = new TimeSpan( 0 , 0 , 10 );
        public TimeSpan ListeningInterval { get { return _ListeningInterval; } set { _ListeningInterval = value; } }

        public bool AutoDeleteRecievedMessages { get; set; }

        public MQListenerStatus Status { get; private set; }

        public event EventHandler<List<MQMessage>> NewMessages;

        public LogMessageQueuingSettings Settings { get; set; }
        private double CurrentInterval { get; set; }

        #endregion
        #region cst.

        public QueueHandler( MQCriteria _criteria ) : this( _criteria , LogMessageQueuingSettings.Default )
        {
        }
        public QueueHandler( MQCriteria _criteria , LogMessageQueuingSettings settings )
        {
            this.Settings = settings;
            this.CurrentInterval = this.Settings.InitialInterval;

            Initialize( _criteria );
        }

        #endregion

        #region IQueueListener

        public void Delete( int messageId )
        {
            using ( var dataHandler = new QueueDataUnity<TRepo, TSettings>() )
            {
                dataHandler.MQ.Delete( messageId );
                dataHandler.Save();
            }
        }
        public void Delete( List<int> messagesId )
        {
            using ( var dataHandler = new QueueDataUnity<TRepo, TSettings>() )
            {
                foreach ( var messageId in messagesId )
                {
                    dataHandler.MQ.Delete( messageId );
                }
                dataHandler.Save();
            }
        }

        public List<MQMessage> GetAll()
        {
            using ( var dataHandler = new QueueDataUnity<TRepo, TSettings>() )
            {
                return dataHandler.MQ.GetAll();
            }
        }
        public MQMessage GetNext()
        {
            using ( var dataHandler = new QueueDataUnity<TRepo, TSettings>() )
            {
                return dataHandler.MQ.GetNext( criteria );
            }
        }
        public List<MQMessage> GetNext( int count )
        {
            using ( var dataHandler = new QueueDataUnity<TRepo, TSettings>() )
            {
                return dataHandler.MQ.GetNext( criteria , count );
            }
        }

        public int PeekCount()
        {
            using ( var dataHandler = new QueueDataUnity<TRepo, TSettings>() )
            {
                return dataHandler.MQ.GetCount();
            }
        }

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
                NLogger.Error( "Exception : " + x );
                return false;
            }

        }
        public bool StopListening()
        {
            if ( this.Status != MQListenerStatus.Listening ) return false;

            Status = MQListenerStatus.Stopping;
            return _ListeningTaskCancellationToken = true;
        }

        #endregion
        #region IQueueSender

        public bool Send( MQMessage message )
        {
            try
            {
                using ( var dataHandler = new QueueDataUnity<TRepo, TSettings>() )
                {
                    dataHandler.MQ.Send( message );
                    dataHandler.Save();

                    return message.Id > 0;
                }
            }
            catch ( Exception x )
            {
                NLogger.Error( "Exception : " + x );
                return false;
            }
        }
        public bool Send( List<MQMessage> messages )
        {
            try
            {
                using ( var dataHandler = new QueueDataUnity<TRepo, TSettings>() )
                {
                    dataHandler.MQ.Send( messages );
                    dataHandler.Save();

                    return true;
                }
            }
            catch ( Exception x )
            {
                NLogger.Error( "Exception : " + x );
                return false;
            }
        }
        public bool Resend( MQMessage message )
        {
            try
            {
                using ( var dataHandler = new QueueDataUnity<TRepo, TSettings>() )
                {
                    dataHandler.MQ.Update( message );
                    dataHandler.Save();
                }
                return true;
            }
            catch ( Exception x )
            {
                NLogger.Error( "Exception : " + x );
                return false;
            }
        }
        public bool Resend( List<MQMessage> messages )
        {
            try
            {
                using ( var dataHandler = new QueueDataUnity<TRepo, TSettings>() )
                {
                    foreach ( var message in messages )
                    {
                        dataHandler.MQ.Update( message );
                    }
                    dataHandler.Save();
                }
                return true;
            }
            catch ( Exception x )
            {
                NLogger.Error( "Exception : " + x );
                return false;
            }
        }

        #endregion

        #region Helper

        private void Initialize( MQCriteria _criteria )
        {
            try
            {
                NLogger.Trace( "" );
                this.criteria = _criteria;
                this._ListeningTask = new Task( ListenForMessages );

                this.Initialized = true;
                this.Status = MQListenerStatus.Idle;
            }
            catch ( Exception x )
            {
                Initialized = false;
                NLogger.Error( "Exception : " + x );
            }
        }
        private async void ListenForMessages()
        {
            while ( true )
            {
                var messagesExist = CheckMassages();

                #region sleep ...

                if ( _ListeningTaskCancellationToken )    // listening canceled ?
                {
                    Status = MQListenerStatus.Idle;
                    _ListeningTaskCancellationToken = false;
                    break;
                }

                #region Calculate Interval

                if ( Settings.IntervalMode == IntervalMode.Dynamic )
                {
                    if ( messagesExist )
                    {
                        CurrentInterval = Math.Max( Settings.MinInterval , Math.Floor( CurrentInterval / 2 ) );
                    }
                    else
                    {
                        CurrentInterval = Math.Min( Settings.MaxInterval , Math.Ceiling( CurrentInterval * 2 ) );
                    }
                }
                else
                {
                    CurrentInterval = CurrentInterval;
                }

                #endregion
                #region LOG
                //XLogger.Info( "Sleeping for : " + ListeningInterval.ToString() );
                #endregion
                await Task.Delay( ListeningInterval );

                #endregion
            }
        }
        private bool CheckMassages()
        {
            try
            {
                var messages = MessagesGet();

                bool status = true;

                status = status && MessagesLock( messages );
                status = status && MessagesEvent( messages );
                status = status && MessagesUnLock( messages );

                return true;
            }
            catch ( Exception x )
            {
                NLogger.Error( "Exception : " + x );
                return false;
            }
        }

        private List<MQMessage> MessagesGet()
        {
            var messages = new List<MQMessage>();

            if ( this.Settings.PopMode == PopMode.Single )
            {
                var message = GetNext();
                if ( message == null ) return null;

                messages.Add( message );
            }
            else
            {
                messages = GetNext( this.Settings.BatchSize );
                if ( messages == null || messages.Count == 0 ) return null;
            }

            return messages;
        }
        private bool MessagesEvent( List<MQMessage> messages )
        {
            if ( messages == null ) return false;
            this.NewMessages( this , messages );

            return true;
        }
        private bool MessagesLock( List<MQMessage> messages )
        {
            if ( messages == null ) return false;

            foreach ( var message in messages )
            {
                message.Status = MQMessageStatus.InProcess;
            }

            return MessageUpdate( messages );
        }
        private bool MessagesUnLock( List<MQMessage> messages )
        {
            if ( messages == null ) return false;

            var messagesToReset = new List<MQMessage>();

            foreach ( var message in messages )
            {
                if ( message.Status != MQMessageStatus.InProcess ) continue;

                message.Status = MQMessageStatus.UnProcessed;
                messagesToReset.Add( message );
            }

            return MessageUpdate( messages );
        }
        private bool MessageUpdate( List<MQMessage> messages )
        {
            if ( messages == null ) return false;
            if ( messages.Count == 0 ) return true;

            for ( int i = 0 ; i < 10 ; i++ )  // 10 retries
            {
                try
                {
                    using ( var dataHandler = new QueueDataUnity<TRepo , TSettings>() )
                    {
                        foreach ( var message in messages )
                        {
                            dataHandler.MQ.Update( message );
                        }

                        dataHandler.Save();
                    }

                    return true;
                }
                catch ( Exception x )
                {
                    NLogger.Error( "Exception : " + x );

                    #region sleep for a fraction of second ...

                    Task.Delay( 250 );

                    #endregion

                    continue;                                   // next retry ...
                }
            }

            return false;   // failed in all retries.
        }

        #endregion
        #region nested.

        public enum PopMode
        {
            Single = 1,
            Batch = 2,
        }
        public enum IntervalMode
        {
            Fixed = 1,
            Dynamic = 2
        }
        public class LogMessageQueuingSettings
        {
            #region props.
            public PopMode PopMode { get; set; } = PopMode.Single;
            public int BatchSize { get; set; } = 10;
            public IntervalMode IntervalMode { get; set; } = IntervalMode.Fixed;
            public double MinInterval { get; set; } = 1;    // 1 Second
            public double MaxInterval { get; set; } = 60;   // 1 Minute
            public double InitialInterval { get; set; } = 10; //  10 Seconds 

            #endregion
            #region props.

            public static LogMessageQueuingSettings Default { get; set; }

            #endregion

            #region cst.
            public LogMessageQueuingSettings()
            {
            }
            public LogMessageQueuingSettings( PopMode popMode , int batchSize , IntervalMode intervalMode , double minInterval , double maxInterval , double initialInterval )
            {
                this.PopMode = popMode;
                this.BatchSize = batchSize;
                this.IntervalMode = intervalMode;
                this.MinInterval = minInterval;
                this.MaxInterval = maxInterval;
                this.InitialInterval = initialInterval;
            }
            static LogMessageQueuingSettings()
            {
                Default = Default ?? new LogMessageQueuingSettings();
            }

            #endregion
        }

        #endregion
    }

    #endregion
}
