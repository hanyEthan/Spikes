﻿using ADS.Tamam.Services.Integration.Common;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration.Install;
using System.Linq;
using System.Threading.Tasks;

namespace ADS.Tamam.Services.Integration
{
    [RunInstaller(true)]
    public partial class ProjectInstaller : System.Configuration.Install.Installer
    {
        public ProjectInstaller()
        {
            InitializeComponent();

            try
            {
                serviceInstaller.ServiceName = Utility.GetConfigurationValue("ServiceName");
            }
            catch (Exception ex)
            {
                ADS.Common.Utilities.XLogger.Error(ex.ToString());
            }
        }
    }
}
