using ADS.Common.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading.Tasks;

namespace ADS.Tamam.Services.MainCalculation
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        static void Main()
        {
            AppDomain.CurrentDomain.UnhandledException += CurrentDomain_UnhandledException;

            ServiceBase [] ServicesToRun;
            ServicesToRun = new ServiceBase [] 
            { 
                new MainCalculationService() 
            };
            ServiceBase.Run ( ServicesToRun );
        }

        static void CurrentDomain_UnhandledException( object sender , UnhandledExceptionEventArgs e )
        {
            System.Diagnostics.Debugger.Launch ();
            XLogger.Error (  e.ExceptionObject.ToString() );
        }
    }
}
