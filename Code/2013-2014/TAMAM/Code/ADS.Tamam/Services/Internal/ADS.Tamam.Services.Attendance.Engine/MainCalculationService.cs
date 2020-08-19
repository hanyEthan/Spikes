using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.ServiceProcess;
using System.Text;

using ADS.Common.Handlers;
using ADS.Common.Utilities;
using ADS.Tamam.Common.Data.Context;
using ADS.Tamam.Modules.Attendance.Handlers;

namespace ADS.Tamam.Services.MainCalculation
{
    public partial class MainCalculationService : ServiceBase
    {
        #region props ...

        private AttendanceWorkers _AttendanceEngineHandler;
        
        #endregion
        #region cst ...

        public MainCalculationService()
        {
            InitializeComponent();
            InitializeServiceName();

            AttendanceServiceInitialize();
        }
        
        #endregion
        #region events ...
        
        protected override void OnStart( string[] args )
        {
//#if DEBUG
//            Debugger.Launch();
//#endif

            // log any untracked exceptions ...
            AppDomain.CurrentDomain.UnhandledException += new UnhandledExceptionEventHandler( currentDomain_UnhandledException );
            
            // check ...
            if ( !_AttendanceEngineHandler.Initialized )
            {
                ExitCode = -50;
                this.Stop();
                return;
            }

            // start attendance service ...
            AttendanceServiceStart();
        }
        protected override void OnStop()
        {
            AttendanceServiceStop();
        }

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

        private void AttendanceServiceInitialize()
        {
            _AttendanceEngineHandler = new AttendanceWorkers( SystemRequestContext.Instance );
        }
        private void AttendanceServiceStart()
        {
            _AttendanceEngineHandler.StartPull();
            _AttendanceEngineHandler.StartHandleOldData();
            _AttendanceEngineHandler.StartHandleDirty();
            _AttendanceEngineHandler.StartDaily();
            _AttendanceEngineHandler.StartStatsTask();
        }
        private void AttendanceServiceStop()
        {
            if ( _AttendanceEngineHandler != null )
            {
                _AttendanceEngineHandler.Terminate();
            }
        }

        #endregion
    }
}
