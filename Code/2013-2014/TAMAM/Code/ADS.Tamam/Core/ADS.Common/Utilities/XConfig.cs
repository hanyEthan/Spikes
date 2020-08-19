using System.Configuration;

namespace ADS.Common.Utilities
{
    /// <summary>
    /// utility class exposing app.config's "appSettings" 
    /// </summary>
    public static class XConfig
    {
        public static string GetValue( string key )
        {
            return ConfigurationManager.AppSettings[key];
        }
        public static string GetConnectionString( string key )
        {
            return ConfigurationManager.ConnectionStrings[key].ConnectionString;
        }
    }
}