using System;
using System.Collections.Generic;
using System.IO;

using Easy.Logger;
using log4net;
using log4net.Appender;
using log4net.Core;
using log4net.Layout;
using XCore.Utilities.Logger.Framework.Logging.Appenders;
using XCore.Utilities.Logger.Framework.Logging.Models.Enums;
using XCore.Utilities.Utilities;

namespace XCore.Utilities.Logger
{
    internal static class Logger
    {
        #region Properties

        private const string LoggerName = "XLogging";
        public static System.Collections.Generic.List<Tuple<string , string>> Levels = new System.Collections.Generic.List<Tuple<string , string>>();
        private static readonly ILog logger = LogManager.GetLogger( LoggerName /*System.Reflection.MethodBase.GetCurrentMethod().DeclaringType*/);
        private static readonly Dictionary<int , Level> LevelMapper = new Dictionary<int , Level>()
        {
            { 0, Level.All },
            { 30000, Level.Debug },
            { 40000, Level.Info },
            { 60000, Level.Warn },
            { 70000, Level.Error },
            { 110000, Level.Fatal },
        };

        #endregion
        #region cst.
        static Logger()
        {
            HandleReferences();
            ConfigureLogger();
            log4net.GlobalContext.Properties[Properties.AppId] = XConfig.GetString( "AdvancedLogger.Context.AppId" ) ?? "App";
            log4net.GlobalContext.Properties[Properties.ModuleId] = XConfig.GetString( "AdvancedLogger.Context.ModuleId" ) ?? "Module";
        }

        #endregion
        #region Internals

        public static class Enums
        {
            [Flags]
            public enum LogType { File = 1, EventLog = 2, Console = 4, Context = 5, EnterpriseLibrary = 6 }
            public enum LogSettings { Normal = 1, TransientCaller = 2 }
            public enum LogStatus { Trace = 0, Info = 1, Warning = 2, Error = 3, }
            public enum LogFormat { Text, Xml, }
        }
        public static class Settings
        {
            public static bool Enabled = true;
            public static bool Split = true;
            public static readonly bool Async;
            public static Enums.LogType Type = Enums.LogType.File;
            public static Enums.LogStatus Sensitivity = Enums.LogStatus.Trace;
            public static bool SafeMode = false;
            public static string Source = "";
            public static int LogAge = 0;

            public static string Target = @"c:\x\logs.txt";
            public static readonly Enums.LogFormat Format = Enums.LogFormat.Text;
        }

        #endregion
        #region Logging ...

        public static bool Trace( string message , string context = null , Exception exception = null )
        {
            try
            {
                SetThreadProperties( context );
                logger.Debug( message , exception );
                return false;
            }
            catch ( Exception ex )
            {
                NLogger.Trace( message );
                NLogger.Error( message , ex );
                return false;
            }
        }
        public static bool Info( string message , string context = null , Exception exception = null )
        {
            try
            {
                SetThreadProperties( context );
                logger.Info( message , exception );
                return false;
            }
            catch ( Exception ex )
            {
                NLogger.Info( message );
                NLogger.Error( message , ex );
                return false;
            }
        }
        public static bool Warning( string message , string context = null , Exception exception = null )
        {
            try
            {
                SetThreadProperties( context );
                logger.Warn( message , exception );
                return false;
            }
            catch ( Exception ex )
            {
                NLogger.Warning( message );
                NLogger.Error( message , ex );
                return false;
            }
        }
        public static bool Error( string message , string context = null , Exception exception = null )
        {
            try
            {
                SetThreadProperties( context );
                logger.Error( message , exception );
                return false;
            }
            catch ( Exception ex )
            {
                NLogger.Error( message , exception );
                NLogger.Error( message , ex );
                return false;
            }
        }

        #endregion
        #region Helpers.

        private static string HandleStringParameters( string message , string[] parameters )
        {
            try
            {
                if ( parameters != null && parameters.Length != 0 )
                {
                    message = string.Format( message , parameters );
                }
            }
            catch ( Exception x )
            {
                Error( "XLogger2.HandleStringParameters ... Exception: " + x );
            }

            return message;
        }
        private static void HandleReferences()
        {
            var x = typeof( AsyncBufferingForwardingAppender );
        }
        private static void ConfigureLogger()
        {
            ILog lg = LogManager.GetLogger( LoggerName );
            log4net.Repository.Hierarchy.Logger log = ( log4net.Repository.Hierarchy.Logger ) lg.Logger;

            var configFilePath = XConfig.GetString( "AdvancedLogger.Configuration.Location" );
            if ( !string.IsNullOrWhiteSpace( configFilePath ) && File.Exists( configFilePath ) )
            {
                log4net.Config.XmlConfigurator.Configure( new System.IO.FileInfo( configFilePath ) );
            }
            else
            {
                AsyncBufferingForwardingAppender asyncBufferingForwardingAppender = new AsyncBufferingForwardingAppender();
                asyncBufferingForwardingAppender.Name = "asyncBufferingForwardingAppender";
                asyncBufferingForwardingAppender.Lossy = false;
                int num;
                asyncBufferingForwardingAppender.BufferSize = int.TryParse( XConfig.GetString( "AdvancedLogger.Log.Buffer.Size" ) , out num ) ? num : 10;
                asyncBufferingForwardingAppender.IdleTime = ( int.TryParse( XConfig.GetString( "AdvancedLogger.Log.Buffer.FlushTimeout" ) , out num ) ? num : 10 ) * 1000;
                asyncBufferingForwardingAppender.Threshold = int.TryParse( XConfig.GetString( "AdvancedLogger.Log.Sensitevity" ) , out num ) ? ( LevelMapper.ContainsKey( num ) ? LevelMapper[num] : Level.All ) : Level.All;

                #region Message Queue Appender

                var dbEnabled = XConfig.GetString( "AdvancedLogger.Log.Database.Enabled" );
                if ( dbEnabled != null && dbEnabled.ToLower() == "true" )
                {
                    var appender = new MessageQueueSenderAppender();
                    appender.ActivateOptions();
                    asyncBufferingForwardingAppender.AddAppender( appender );
                }

                #endregion
                #region Failover Appender

                var failoverEnabled = XConfig.GetString( "AdvancedLogger.Log.Failover.Enabled" );
                if ( failoverEnabled == null || failoverEnabled.ToLower() == "true" )
                {
                    RollingFileAppender rollingFileAppender = new RollingFileAppender();
                    rollingFileAppender.Name = "RollingFileAppender";
                    rollingFileAppender.File = XConfig.GetString( "AdvancedLogger.Log.Failover.Location" ) ?? ( InitializeLogFile( @"C:\x\Logs\FileLog.txt" ) ? @"C:\x\Logs\FileLog.txt" : string.Empty );
                    rollingFileAppender.AppendToFile = true;
                    rollingFileAppender.RollingStyle = XConfig.GetString( "AdvancedLogger.Log.Failover.Split.Size" ) == "0" ? RollingFileAppender.RollingMode.Date : RollingFileAppender.RollingMode.Composite;  // composite !!
                    rollingFileAppender.MaxSizeRollBackups = -1;
                    rollingFileAppender.MaximumFileSize = $"{XConfig.GetString( "AdvancedLogger.Log.Failover.Split.Size" )}MB";
                    rollingFileAppender.StaticLogFileName = true;  // try false !!

                    PatternLayout layout = new PatternLayout();
                    layout.ConversionPattern = "[%date] (%thread) %-5level %logger [%property{NDC}] - %message%newline";
                    layout.ActivateOptions();
                    rollingFileAppender.Layout = layout;
                    rollingFileAppender.ActivateOptions();

                    asyncBufferingForwardingAppender.AddAppender( rollingFileAppender );
                }

                #endregion
                asyncBufferingForwardingAppender.ActivateOptions();

                log.AddAppender( asyncBufferingForwardingAppender );
                log.Repository.Configured = true;
            }
        }
        private static void SetThreadProperties( string context = null )
        {
            log4net.ThreadContext.Properties[Properties.Context] = context;
        }
        private static bool InitializeLogFile( string filePath )
        {
            try
            {
                var file = new FileInfo( filePath );
                if ( !file.Directory.Exists ) file.Directory.Create();

                return true;
            }
            catch
            {
                return false;
            }
        }

        #endregion
    }
}
