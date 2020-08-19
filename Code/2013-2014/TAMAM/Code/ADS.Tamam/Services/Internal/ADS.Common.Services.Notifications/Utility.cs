﻿using System;
using System.Configuration;
using System.Reflection;

namespace ADS.Common.Services.Notifications
{
    internal class Utility
    {
        internal static string GetConfigurationValue( string key )
        {
            Assembly service = Assembly.GetAssembly( typeof( NotificationsService ) );
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
