using System;
using System.Linq;
using System.ServiceProcess;
using System.Threading.Tasks;
using ADS.Common;
using ADS.Common.Context;
using ADS.Common.Handlers;
using ADS.Common.Utilities;
using ADS.Tamam.Common.Data.Context;
using ADS.Tamam.Common.Handlers;
using System.Diagnostics;

namespace ADS.Tamam.Services.LeaveCredits
{
    public partial class LeavesCreditService : ServiceBase
    {
        # region Fields

        private Task _MainTask;

        # endregion
        # region Cst..

        public LeavesCreditService()
        {
            InitializeComponent();
        }

        # endregion
        # region Start

        protected override void OnStart(string[] args)
        {
            XLogger.Trace( "");
            try
            {
//#if DEBUG
//                Debugger.Launch();
//#endif

                if (!TamamServiceBroker.Status.Initialized ) throw new Exception("TamamService Broker is not initialized.");

                _MainTask = new Task( Process );
                _MainTask.Start();
            }
            catch (Exception x)
            {
                XLogger.Error("Exception : ", x);
            }
        }

        # endregion
        # region Stop

        protected override void OnStop()
        {
            XLogger.Info("");
        }

        # endregion
        # region internals

        internal bool TransferCredits()
        {
            XLogger.Info( "" );

            try
            {
                var response = TamamServiceBroker.LeavesHandler.TransferCredits( SystemRequestContext.Instance );
                if ( response.Type != ResponseState.Success )
                {
                    XLogger.Error( "Exception: " + response.MessageDetailed.Select( x => x.Meta ) );
                    return false;
                }

                return true;
            }
            catch ( Exception x )
            {
                XLogger.Error( "Exception : " , x );
                return false;
            }
        }
        private async void Process()
        {
            var interval = GetWorkingInterval();

            while ( true )
            {
                try
                {
                    XLogger.Info( "Main Task Started." );

                    TransferCredits();
                }
                catch ( Exception x )
                {
                    XLogger.Error( "Exception : " , x );
                }

                XLogger.Info( "Main Task: Delay for: [{0}]" , interval.ToString() );
                await Task.Delay( interval );
            }
        }
        private TimeSpan GetWorkingInterval()
        {
            try
            {
                string interval = Broker.ConfigurationHandler.GetValue(Constants.TamamLeaveCreditConfig.SectionTamam_LeaveCredits, Constants.TamamLeaveCreditConfig.InternalServiceTransferCreditsInterval);
                return TimeSpan.Parse( interval );
            }
            catch ( Exception x )
            {
                XLogger.Error( "Exeption : " + x );
                throw;
            }
        }

        # endregion
    }
}