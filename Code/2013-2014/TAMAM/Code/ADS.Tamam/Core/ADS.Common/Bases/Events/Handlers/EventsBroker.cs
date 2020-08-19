using System;
using ADS.Common.Bases.Events.Contracts;
using ADS.Common.Bases.Events.Models;
using ADS.Common.Bases.MessageQueuing.Handlers.DB;
using ADS.Common.Bases.MessageQueuing.Models;
using ADS.Common.Handlers;
using ADS.Common.Utilities;

namespace ADS.Common.Bases.Events.Handlers
{
    public static class EventsBroker
    {
        #region props ...

        internal static EventMode DefaultExecutionMode { get; set; }
        private static MQDBHandler<EventCell> MQ { get; set; }

        #endregion
        #region cst.

        static EventsBroker()
        {
            XLogger.Trace( "EventsBroker ..." );
            try
            {
                DefaultExecutionMode = GetExecutionMode();
                MQ = new MQDBHandler<EventCell>( false , new TimeSpan( 0 , 0 , 10 ) );
            }
            catch ( Exception x )
            {
                XLogger.Error("Exception : " + x);
            }
        }

        #endregion
        #region Publics ...

        public static bool Handle( IEventCell eventCell )
        {
            return HandleEvent( eventCell , DefaultExecutionMode );
        }
        public static bool Handle( IEventCell eventCell , EventMode executionMode )
        {
            return HandleEvent( eventCell , executionMode );
        }

        public static bool Cancel( string eventTargetCode )
        {
            throw new NotImplementedException();
        }
        
        #endregion
        #region Helpers

        private static bool HandleEvent( IEventCell eventCell , EventMode executionMode )
        {
            try
            {
                if ( eventCell == null ) return false;

                return executionMode == EventMode.Async
                       ? SendThroughMQ<EventCell>( ( EventCell ) eventCell )
                       : eventCell.Process();
            }
            catch ( Exception x )
            {
                XLogger.Error( "Exception : " + x );
                return false;
            }
        }
        private static bool SendThroughMQ<T>( T eventCell ) where T : IEventCell
        {
            var message = CreateMQMessage<EventCell>( ( EventCell ) ( object ) eventCell );
            return MQ.Send( message );
        }
        private static MQMessage CreateMQMessage<T>( T eventCell ) where T : IEventCell
        {
            var id = Guid.NewGuid();

            var message = new MQMessage()
            {
                Id = id ,
                Code = id.ToString() ,
                TargetCode = eventCell.TargetId ,
                TargetType = eventCell.TargetType ,
            };

            bool state = message.ContentSet<T>( eventCell );
            return state ? message : null;
        }
        private static EventMode GetExecutionMode()
        {
            EventMode eventsMode = EventMode.Sync;

            var mode = Broker.ConfigurationHandler.GetValue( "Events" , "Events.Execution.Mode" );
            if ( string.IsNullOrEmpty( mode ) ) return eventsMode;
            if ( !Enum.TryParse<EventMode>( mode , out eventsMode ) ) return eventsMode;

            return eventsMode;
        }

        #endregion
    }
}
