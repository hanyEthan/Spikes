using System;
using ADS.Common.Bases.Events.Models;
using ADS.Common.Bases.MessageQueuing.Contracts;
using ADS.Common.Bases.MessageQueuing.Contracts;
using ADS.Common.Bases.MessageQueuing.Models;
using ADS.Common.Utilities;

namespace ADS.Common.Bases.Events.Schedular
{
    public class EventsProcessor : IMQSubscriber<EventCell>
    {
        #region props ...

        public string Name { get { return "EventsProcessor"; } }
        public bool Initialized { get; private set; }
        
        #endregion
        #region cst ...

        public EventsProcessor()
        {
            this.Initialized = true;
        }
        
        #endregion
        #region IMQSubscriber

        public bool Handle( MQMessage message )
        {
            var eventCell = message.ContentGet<EventCell>();
            return Handle( eventCell );
        }
        public bool Handle<T>( T content ) where T : IMQMessageContent
        {
            try
            {
                if ( content == null || !( content is EventCell ) ) return false;    // validate ...
                return ( ( EventCell ) ( object ) content ).Process();               // process ...
            }
            catch ( Exception x )
            {
                XLogger.Error( "Exception : " + x );
                return false;
            }
        }

        #endregion
    }
}
