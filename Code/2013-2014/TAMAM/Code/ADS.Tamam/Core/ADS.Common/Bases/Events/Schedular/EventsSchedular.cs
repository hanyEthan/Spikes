using System;
using System.Collections.Generic;

using ADS.Common.Bases.Events.Contracts;
using ADS.Common.Bases.Events.Models;
using ADS.Common.Bases.MessageQueuing.Handlers.DB;
using ADS.Common.Utilities;

namespace ADS.Common.Bases.Events.Schedular
{
    public class EventsSchedular
    {
        #region props ...

        public List<IEventWorker> Workers { get; set; }
        public bool IsHealthy { get; set; }
        
        #endregion
        #region cst ...

        public EventsSchedular()
        {
            Workers = new List<IEventWorker>();
            IsHealthy = true;
        }

        #endregion
        #region publics

        public bool Start()
        {
            return AssignWorkers();
        }
        public bool Stop()
        {
            try
            {
                foreach ( var worker in Workers )
                {
                    worker.Stop();
                }

                Workers.Clear();
                return true;
            }
            catch ( Exception x )
            {
                XLogger.Error( "Exception : " + x );
                return false;
            }
        }

        #endregion
        #region Helpers

        private bool AssignWorkers()
        {
            try
            {
                // TODO : this should be an async thread, constantly analyzing the queues, and creating / assigning the workers accordingly ...

                var worker = StartNewWorker();
                if ( worker != null )
                {
                    Workers.Add( worker );
                }
                else
                {
                    IsHealthy = false;
                }

                return IsHealthy;
            }
            catch ( Exception x )
            {
                XLogger.Error( "Exception : " + x );
                return false;
            }
        }
        private IEventWorker StartNewWorker()
        {
            var worker = new EventsWorker<MQDBHandler<EventCell> , EventsProcessor , EventCell>();
            var state = worker.Initialized && worker.Start();

            return state ? worker : null;
        }
        
        #endregion
    }
}
