﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading.Tasks;

namespace ADS.Tamam.Services.DataAcquisition
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        static void Main ()
        {
            ServiceBase [] ServicesToRun;
            ServicesToRun = new ServiceBase [] 
            { 
                new DataAcquisition() 
            };
            ServiceBase.Run ( ServicesToRun );
          
        }
    }
}
