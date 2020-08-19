using System;
using System.Configuration;
using System.Reflection;
using ADS.CommonServices.CentralizedCache;

namespace ADS.Common.Services.Cache
{
    internal class Utility
    {
        internal static string GetConfigurationValue( string key )
        {
            Assembly service = Assembly.GetAssembly( typeof( CentralizedCacheWindowsService ) );
            Configuration config = ConfigurationManager.OpenExeConfiguration( service.Location );
            if ( config.AppSettings.Settings[key] != null )
            {
                return config.AppSettings.Settings[key].Value;
            }
            else
            {
                throw new IndexOutOfRangeException( "Settings collection does not contain the requested key: " + key );
            }
        }
    }
}
