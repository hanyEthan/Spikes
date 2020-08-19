using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration.Install;
using System.Linq;
using System.Threading.Tasks;
using ADS.Common.Utilities;

namespace ADS.Common.Services.Events
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
