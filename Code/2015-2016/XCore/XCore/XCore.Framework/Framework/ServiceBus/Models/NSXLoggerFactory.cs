using System;
using NServiceBus.Logging;

namespace XCore.Framework.Framework.ServiceBus.Models
{
    #region factory

    class NSXLoggerFactory : ILoggerFactory
    {
        LogLevel level;

        public NSXLoggerFactory( LogLevel level )
        {
            this.level = level;
        }

        public ILog GetLogger( Type type )
        {
            return GetLogger( type.FullName );
        }

        public ILog GetLogger( string name )
        {
            return new NSXLogger( name , level );
        }
    }

    #endregion
}
