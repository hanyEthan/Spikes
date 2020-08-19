using System;
using System.ComponentModel;
using System.Configuration.Install;
using ADS.Common.Services.Cache;
using ADS.Common.Utilities;

namespace ADS.CommonServices.CentralizedCache
{
    [RunInstaller( true )]
    public partial class ProjectInstaller : Installer
    {
        public ProjectInstaller()
        {
            InitializeComponent();

            try
            {
                serviceInstaller1.ServiceName = Utility.GetConfigurationValue( "ServiceName" );
            }
            catch ( Exception x )
            {
                XLogger.Error( x.ToString() );
            }
        }
    }
}
