using System;
using System.Threading.Tasks;
using ADS.Common.Bases.Events.Contracts;
using ADS.Common.Bases.Events.Models;
using ADS.Common.Bases.MessageQueuing.Contracts;
using ADS.Common.Bases.MessageQueuing.Contracts;
using ADS.Common.Bases.MessageQueuing.Models;
using ADS.Common.Utilities;

namespace ADS.Common.Bases.Events.Schedular
{
    internal class EventsWorker<TMQListener , TMQSubscriber , TContent> : IEventWorker
        where TContent : IMQMessageContent
        where TMQListener : IMQListener<TContent> , new()
        where TMQSubscriber : IMQSubscriber<TContent> , new()
    {
        #region props ...

        public Guid Id { get; set; }
        public bool Initialized { get; private set; }
        public string Name { get { return "EventsWorker"; } }
        public EventsWorkerStatus Status { get; private set; }

        public TMQListener Listener { get; set; }
        public TMQSubscriber Processor { get; set; }

        #endregion
        #region cst ...

        public EventsWorker()
        {
            try
            {
                // ...
                this.Id = Guid.NewGuid();
                this.Listener = new TMQListener();
                this.Processor = new TMQSubscriber();

                // ...
                this.Listener.NewMessage += Listener_NewMessage;

                // ...
                this.Initialized = this.Listener.Initialized && this.Processor.Initialized;
                this.Status = EventsWorkerStatus.InActive;
            }
            catch ( Exception x )
            {
                XLogger.Error( "Exception : " + x );
                Initialized = false;
            }
        }

        #endregion
        #region publics

        public bool Start()
        {
            if ( !this.Initialized || this.Status == EventsWorkerStatus.Disabled ) return false;
            if ( this.Status == EventsWorkerStatus.Active ) return true;

            var state = Listener.StartListening();
            this.Status = state ? EventsWorkerStatus.Active : this.Status;

            return state;
        }
        public bool Stop()
        {
            if ( !this.Initialized || this.Status == EventsWorkerStatus.Disabled ) return false;
            if ( this.Status != EventsWorkerStatus.Active ) return false;
            if ( this.Listener.Status != MQListenerStatus.Listening ) return false;

            var state = Listener.StopListening();

            #region waiting to stop

            for ( int i = 0 ; i < 10 ; i++ )
            {
                if ( Listener.Status == MQListenerStatus.Idle ) return true;
                Task.Delay( 500 );    // wait for 0.5 seconds ...
            }
            return false;    // timeout, still stopping ...
            
            #endregion
        }

        #endregion
        #region helpers

        void Listener_NewMessage( object sender , MQMessage e )
        {
            var state = Processor.Handle( e.ContentGet<TContent>() );
            if ( state ) Listener.Delete( e.Id );
        }

        #endregion
    }
}
