using System;
using System.ComponentModel;
using System.Configuration.Install;

using ADS.Common.Utilities;

namespace ADS.Common.Services.Notifications
{
    [RunInstaller(true)]
    public partial class ProjectInstaller : Installer
    {
        public ProjectInstaller()
        {
            InitializeComponent();

            try
            {
                serviceInstaller.ServiceName = Utility.GetConfigurationValue ( "ServiceName" );
            }
            catch ( Exception x )
            {
                XLogger.Error ( x.ToString () );
            }
        }
    }
}
