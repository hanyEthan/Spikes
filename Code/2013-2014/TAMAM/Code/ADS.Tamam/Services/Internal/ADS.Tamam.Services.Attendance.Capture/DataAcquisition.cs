using System;
using System.Threading;
using System.Diagnostics;
using System.ServiceProcess;

using ADS.Common.Handlers;
using ADS.Common.Utilities;
using ADS.Tamam.Services.DataAcquisition.Handlers;

namespace ADS.Tamam.Services.DataAcquisition
{
    public partial class DataAcquisition : ServiceBase
    {
        #region props ...
        
        private Thread _AttendanceCaptureThread = null;
        private PullMechanismHandler _AttendanceCaptureHandler = null;

        #endregion
        #region cst ...

        public DataAcquisition()
        {
            InitializeComponent();
            InitializeServiceName();
            InitializeWorkingThread();
        }
        
        #endregion
        #region events ...

        protected override void OnStart( string[] args )
        {
//#if DEBUG
//            Debugger.Launch();
//#endif
            
            // log any uncaptured exceptions ...
            AppDomain.CurrentDomain.UnhandledException += new UnhandledExceptionEventHandler( currentDomain_UnhandledException );
            
            // start attendance capture ...
            _AttendanceCaptureThread.Start();
        }
        protected override void OnStop()
        {
            _AttendanceCaptureThread.Abort();
        }
        
        #endregion

        #region events

        private void currentDomain_UnhandledException( object sender , UnhandledExceptionEventArgs e )
        {
            XLogger.Error( ( ( Exception ) e.ExceptionObject ).ToString() );
        }
        
        #endregion
        #region helpers ...

        private void InitializeServiceName()
        {
            this.ServiceName = System.Configuration.ConfigurationManager.AppSettings["ServiceName"];
        }
        private void InitializeWorkingThread()
        {
            _AttendanceCaptureHandler = new PullMechanismHandler();
            _AttendanceCaptureThread = new Thread( StartPull );
        }
        private TimeSpan GetPullInterval()
        {
            try
            {
                string pullIntervalValue = Broker.ConfigurationHandler.GetValue(ADS.Common.Constants.TamamCaptureConfig.Section ,ADS.Common.Constants.TamamCaptureConfig.WorkerPullInterval );
                return TimeSpan.Parse( pullIntervalValue );
            }
            catch ( Exception x )
            {
                XLogger.Error( "Configuration Error : Exeption : " + x );
                throw;
            }
        }

        private void StartPull()
        {
            var pullInterval = GetPullInterval();

            while ( true )
            {
                _AttendanceCaptureHandler.TransferData();
                Thread.Sleep( pullInterval );
            }
        }
        
        #endregion
    }
}
