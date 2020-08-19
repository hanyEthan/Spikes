//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;
//using NServiceBus.Logging;
//using XCore.Utilities.Logger;
//using XCore.Utilities.Utilities;

//namespace XCore.Framework.Framework.ServiceBus.Models
//{
//    #region log
//    class NSXLogger : ILog
//    {
//        string name;
//        public bool IsDebugEnabled { get; }
//        public bool IsInfoEnabled { get; }
//        public bool IsWarnEnabled { get; }
//        public bool IsErrorEnabled { get; }
//        public bool IsFatalEnabled { get; }

//        public NSXLogger( string name , LogLevel level )
//        {
//            this.name = name;
//            IsDebugEnabled = LogLevel.Debug >= level;
//            IsInfoEnabled = LogLevel.Info >= level;
//            IsWarnEnabled = LogLevel.Warn >= level;
//            IsErrorEnabled = LogLevel.Error >= level;
//            IsFatalEnabled = LogLevel.Fatal >= level;
//        }

//        void Write( string level , string message , Exception exception )
//        {
//            Console.WriteLine( $"{name}. {level}. {message}. Exception: {exception}" );
//        }
//        void Write( string level , string message )
//        {
//            Console.WriteLine( $"{name}. {level}. {message}." );
//        }

//        void Write( string level , string format , params object[] args )
//        {
//            format = $"{name}. {level}. {format}";
//            Console.WriteLine( format , args );
//        }

//        public void Debug( string message )
//        {
//            if ( IsDebugEnabled )
//            {
//                XLogger.Trace( message );
//            }
//        }

//        public void Debug( string message , Exception exception )
//        {
//            if ( IsDebugEnabled )
//            {
//                var m = string.Format( "{0} {1}" , message , exception.ToString() );
//                XLogger.Trace( m );
//            }
//        }

//        public void DebugFormat( string format , params object[] args )
//        {
//            if ( IsDebugEnabled )
//            {
//                var m = string.Format( format , args );
//                XLogger.Trace( m );
//            }
//        }
//        #endregion
//        public void Info( string message )
//        {
//            if ( IsInfoEnabled )
//            {
//                XLogger.Info( message );
//            }
//        }

//        public void Info( string message , Exception exception )
//        {
//            if ( IsInfoEnabled )
//            {
//                var m = string.Format( "{0} {1}" , message , exception.ToString() );
//                XLogger.Info( m );
//            }
//        }

//        public void InfoFormat( string format , params object[] args )
//        {
//            if ( IsInfoEnabled )
//            {
//                var m = string.Format( format , args );
//                XLogger.Info( m );
//            }
//        }

//        public void Warn( string message )
//        {
//            if ( IsWarnEnabled )
//            {
//                XLogger.Warning( message );
//            }
//        }

//        public void Warn( string message , Exception exception )
//        {
//            if ( IsWarnEnabled )
//            {
//                var m = string.Format( "{0} {1}" , message , exception.ToString() );
//                XLogger.Warning( m );
//            }
//        }

//        public void WarnFormat( string format , params object[] args )
//        {
//            if ( IsWarnEnabled )
//            {
//                var m = string.Format( format , args );
//                XLogger.Warning( m );
//            }
//        }

//        public void Error( string message )
//        {
//            if ( IsErrorEnabled )
//            {
//                XLogger.Error( message );
//            }
//        }

//        public void Error( string message , Exception exception )
//        {
//            if ( IsErrorEnabled )
//            {
//                var m = string.Format( "{0} {1}" , message , exception.ToString() );
//                XLogger.Error( m );
//            }
//        }

//        public void ErrorFormat( string format , params object[] args )
//        {
//            if ( IsErrorEnabled )
//            {
//                var m = string.Format( format , args );
//                XLogger.Error( m );
//            }
//        }

//        public void Fatal( string message )
//        {
//            if ( IsFatalEnabled )
//            {
//                XLogger.Error( message );
//            }
//        }

//        public void Fatal( string message , Exception exception )
//        {
//            if ( IsFatalEnabled )
//            {
//                var m = string.Format( "{0} {1}" , message , exception.ToString() );
//                XLogger.Error( m );
//            }
//        }

//        public void FatalFormat( string format , params object[] args )
//        {
//            if ( IsFatalEnabled )
//            {
//                var m = string.Format( format , args );
//                XLogger.Error( m );
//            }
//        }
//    }
//}
