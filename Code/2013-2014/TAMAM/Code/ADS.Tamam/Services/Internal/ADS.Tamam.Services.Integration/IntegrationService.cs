using System;
using System.ServiceProcess;
using System.Diagnostics;
using System.Threading;

using ADS.Common.Handlers;
using ADS.Common.Utilities;
using ADS.Tamam.Common.Data.Context;
using ADS.Tamam.Common.Data.Contracts;
using ADS.Tamam.Modules.Integration;

namespace ADS.Tamam.Services.Integration
{
    public partial class IntegrationService : ServiceBase
    {
        #region props ...

        private Thread _IntegrationThread = null;

        #endregion

        public IntegrationService()
        {
            InitializeComponent();
            InitializeServiceName();
            InitializeWorkingThread();
        }

        #region Events

        protected override void OnStart(string[] args)
        {
//#if DEBUG
//            Debugger.Launch();
//#endif

            // log any uncaptured exceptions ...
            AppDomain.CurrentDomain.UnhandledException += new UnhandledExceptionEventHandler(currentDomain_UnhandledException);

            _IntegrationThread.Start();
        }
        protected override void OnStop()
        {
            _IntegrationThread.Abort();
        }

        #endregion
        #region helpers

        private void InitializeServiceName()
        {
            this.ServiceName = System.Configuration.ConfigurationManager.AppSettings["ServiceName"];
        }
        private void InitializeWorkingThread()
        {
            _IntegrationThread = new Thread(StartIntegration);
        }
        private TimeSpan GetWorkingInterval()
        {
            try
            {
                string pullIntervalValue = Broker.ConfigurationHandler.GetValue("Tamam.Integration", "Worker.Interval");
                return TimeSpan.Parse(pullIntervalValue);
            }
            catch (Exception x)
            {
                XLogger.Error("Configuration Error : Exeption : " + x);
                throw;
            }
        }

        private void StartIntegration()
        {
            var interval = GetWorkingInterval();

            IIntegrationHandler handler = new IntegrationHandler();
            if (!handler.Initialized) throw new ApplicationException("Error Occured, please review service logs ...");

            while (true)
            {
                try
                {
                    var response = handler.Synchronize(SystemRequestContext.Instance);
                }
                catch (Exception ex)
                {
                    XLogger.Error("handler.Synchronize", ex);
                }

                XLogger.Info("Integration Service will sleep for " + interval.Minutes + "Min");
                XLogger.Info("Planned restart at " + DateTime.Now.Add(interval));
                Thread.Sleep(interval);
            }
        }


        private void currentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            XLogger.Error(((Exception)e.ExceptionObject).ToString());
        }

        #endregion
    }
}