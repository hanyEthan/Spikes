using System;
using System.Configuration;
using System.Globalization;

namespace XCore.Utilities.Utilities
{
    public static class XConfig
    {
        public static string GetString( string key )
        {
            return ConfigurationManager.AppSettings[key];
        }
        public static bool? GetBool( string key )
        {
            bool value;
            string confic = GetString( key );
            return bool.TryParse( confic , out value ) ? ( bool? ) value : null;
        }
        public static int? GetInt( string key )
        {
            int value;
            string confic = GetString( key );
            return int.TryParse( confic , out value ) ? ( int? ) value : null;
        }
        public static TimeSpan? GetTimeSpan( string key )
        {
            TimeSpan value;
            string confic = GetString( key );
            return TimeSpan.TryParseExact( confic , "hh':'mm':'ss" , CultureInfo.InvariantCulture , out value ) ? ( TimeSpan? ) value : null;
        }

        public static string GetConnectionString( string key )
        {
            return ConfigurationManager.ConnectionStrings[key].ConnectionString;
        }
    }
}
