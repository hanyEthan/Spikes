using System;
using System.Diagnostics;
using System.ServiceProcess;
using ADS.Common.Handlers;
using ADS.Common.Utilities;

namespace ADS.Common.Services.Notifications
{
    public partial class NotificationsService : ServiceBase
    {
        public NotificationsService()
        {
            InitializeComponent();
            this.ServiceName = System.Configuration.ConfigurationManager.AppSettings ["ServiceName"];
        }

        protected override void OnStart(string[] args)
        {
            XLogger.Trace( "" );
//#if DEBUG
//            Debugger.Launch();
//#endif

            try
            {
                if ( Broker.Initialized )
                {
                    Broker.NotificationsListner.StartListening();
                }
            }
            catch (Exception x)
            {
                XLogger.Error("Exception : ", x);
            }
        }
        protected override void OnStop()
        {
            XLogger.Trace( "" );
        }
    }
}