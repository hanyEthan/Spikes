using System;
using System.ServiceProcess;

using ADS.Common.Utilities;
using ADS.Common.Bases.Events.Schedular;

namespace ADS.Common.Services.Events
{
    public partial class EventsService : ServiceBase
    {
        #region props

        private EventsSchedular Schedular { get; set; }
        
        #endregion

        #region cst

        public EventsService()
        {
            InitializeComponent();
        }
        
        #endregion
        #region Events

        protected override void OnStart( string[] args )
        {
//#if DEBUG
//            Debugger.Launch();
//#endif

            try
            {
                XLogger.Trace( "" );

                this.Schedular = new EventsSchedular();
                var state = this.Schedular.Start();
            }
            catch ( Exception x )
            {
                XLogger.Error( "Exception : " + x );
                throw;
            }

        }
        protected override void OnStop()
        {
            try
            {
                XLogger.Trace( "" );

                // TODO ...
                var state = this.Schedular.Stop();

            }
            catch ( Exception x )
            {
                XLogger.Error( "Exception : " + x );
                throw;
            }
        }
        
        #endregion
    }
}