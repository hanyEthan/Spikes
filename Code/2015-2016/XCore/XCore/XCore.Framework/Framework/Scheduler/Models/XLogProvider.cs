using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Quartz.Logging;
using XCore.Utilities.Logger;
using XCore.Utilities.Utilities;

namespace XCore.Framework.Framework.Scheduler.Models
{
    public class XLogProvider : ILogProvider
    {
        public Logger GetLogger( string name )
        {
            return ( level , func , exception , parameters ) =>
            {
                if ( level >= LogLevel.Info && func != null )
                {
                    var line = string.Format( $"Quarts : " + func() , parameters );

                    switch ( level )
                    {
                        case LogLevel.Trace:
                        case LogLevel.Debug:
                            XLogger.Trace( line );
                            break;
                        case LogLevel.Info:
                            XLogger.Info( line );
                            break;
                        case LogLevel.Warn:
                            XLogger.Warning( line );
                            break;
                        case LogLevel.Error:
                        case LogLevel.Fatal:
                            XLogger.Error( line );
                            break;
                        default:
                            break;
                    }
                }
                return true;
            };
        }

        public IDisposable OpenNestedContext( string message )
        {
            throw new NotImplementedException();
        }

        public IDisposable OpenMappedContext( string key , string value )
        {
            throw new NotImplementedException();
        }
    }
}
