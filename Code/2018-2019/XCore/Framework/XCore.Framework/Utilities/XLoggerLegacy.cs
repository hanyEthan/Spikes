using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using XCore.Framework.Utilities;

namespace XCore.Framework.Utilities
{
    public static class XLoggerLegacy
    {
        #region nested.

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
            public static bool LogCleaningEnabled = false;
            public static int LogAge = 0;

            public static string Target = @"c:\x\logs.txt";
            public static readonly Enums.LogFormat Format = Enums.LogFormat.Text;
        }

        private static class Configuration
        {
            private static XConfig xconfig = new XConfig(XCoreConstants.Config.FileName);


            public static bool GetStatus()
            {
                try
                {
                    var config = xconfig.GetSection<XLoggerConfigSettings>("XLogger");

                    Settings.Target = string.IsNullOrWhiteSpace(config?.Location) ? Settings.Target : config.Location;
                    Settings.SafeMode = config?.SafeMode == null ? Settings.SafeMode : config.SafeMode.Value;
                    Settings.Source = string.IsNullOrWhiteSpace(config?.AppSource) ? Settings.Source : config.AppSource;
                    Settings.Type = config?.FileSystemMode != true ? Settings.Type : Settings.Type | Enums.LogType.File;
                    Settings.Sensitivity = config?.Sensitivity == null ? Settings.Sensitivity : config.Sensitivity.Value;
                    Settings.LogCleaningEnabled = config?.AutoClean == null ? Settings.LogCleaningEnabled : config.AutoClean.Value;
                    Settings.LogAge = config?.AutoCleanAge == null ? Settings.LogAge : config.AutoCleanAge.Value;

                    return true;
                }
                catch { return false; }
            }
        }

        private interface ILogTarget
        {
            void Log( XLogEntry entry , ILogFormatter formatter );
            bool Clean( int logAge );
        }
        private interface ILogFormatter
        {
            string Format( XLogEntry logEntry );
        }

        private class XLogEntry
        {
            public DateTime Time { get; set; }
            public string Message { get; set; }
            public string Context { get; set; }
            public string Source { get; set; }
            public Enums.LogStatus Status { get; set; }
            public XLoggerLegacy.Enums.LogSettings LogSettings { get; set; }

            public XLogEntry( DateTime time , string message , string context , Enums.LogStatus status )
            {
                Time = time;
                Message = message;
                Context = context;
                Source = Settings.Source;
                Status = status;
                LogSettings = Enums.LogSettings.Normal;
            }
            public XLogEntry( DateTime time , string message , string context , Enums.LogStatus status , Enums.LogSettings logSettings )
                : this( time , message , context , status )
            {
                LogSettings = logSettings;
            }
        }

        private class TextFormatter : ILogFormatter
        {
            public string Format( XLogEntry logEntry )
            {
                string source = string.IsNullOrEmpty( logEntry.Source ) ? "" : string.Format( "[{0}] :: " , logEntry.Source );
                return logEntry == null ? "" : string.Format( CultureInfo.InvariantCulture , "[{0}] [{1}] {2}{3} ... {4}" , logEntry.Time.ToString( "dd.MM.yyyy HH:mm:ss" , CultureInfo.InvariantCulture ) , logEntry.Status , source , logEntry.Context , logEntry.Message );
            }
        }

        private class XmlFormatter : ILogFormatter
        {
            public string Format( XLogEntry logEntry )
            {
                throw new NotImplementedException();
            }
        }

        private class FileLog : ILogTarget
        {
            #region Properties

            private bool _Enabled = true;

            #endregion
            #region cst

            public FileLog()
            {
                _Enabled = Initialize();
                Log( null );
            }

            #endregion

            public void Log( XLogEntry entry , ILogFormatter formatter )
            {
                if ( !_Enabled ) return;
                SplitLog();
                Log( formatter.Format( entry ) );
            }
            public bool Clean( int logAge )
            {
                try
                {
                    var FI = new FileInfo( Settings.Target );
                    return DeleteOldFiles( FI.DirectoryName , logAge );
                }
                catch
                {
                    return false;
                }
            }

            #region Helpers

            private void Log( string entry )
            {
                try
                {
                    File.AppendAllText( Settings.Target , ( entry ?? "" ) + "\r\n" );
                }
                catch
                {
                    //if (Settings.SafeMode) throw;
                    return;
                }
            }
            private bool Initialize()
            {
                try
                {
                    var file = new FileInfo( Settings.Target );
                    if ( !file.Directory.Exists ) file.Directory.Create();
                    if ( !file.Exists )
                    {
                        using ( file.Create() ) { }
                        file.CreationTime = DateTime.UtcNow;
                    }

                    return true;
                }
                catch
                {
                    //if (!Settings.SafeMode) throw;
                    return false;
                }
            }
            private void SplitLog()
            {
                try
                {
                    var _File = new FileInfo( Settings.Target );
                    if ( _Enabled && _File.Exists && _File.CreationTime.Date.CompareTo( DateTime.UtcNow.Date ) != 0 )
                    {
                        string backupFileName = string.Format( @"{0}\{1}_{2}{3}" , _File.DirectoryName , _File.Name.Substring( 0 , _File.Name.IndexOf( _File.Extension ) ) , _File.CreationTime.Date.ToString( "yyyyMMdd" ) , _File.Extension );
                        //MG,27/11/2014.Cehck the existence of 'backupFileName' before call 'MoveTo' or move to will raise exception :'Cannot create a file when that file already exists.'
                        if ( !File.Exists( backupFileName ) )
                            _File.MoveTo( backupFileName );

                        Initialize();
                    }
                }
                catch { return; }
            }

            private static bool DeleteOldFiles( string directory , int ageInHours )
            {
                try
                {
                    var files = Directory.GetFiles( directory );
                    foreach ( string file in files )
                    {
                        var creationTime = File.GetCreationTime( file );
                        if ( creationTime < DateTime.UtcNow.AddHours( -1 * Math.Abs( ageInHours ) ) )
                        {
                            File.Delete( file );
                        }
                    }

                    return true;
                }
                catch ( Exception x )
                {
                    XLogger.Error( "Exception : " + x );
                    return false;
                }
            }

            #endregion
        }

        public class XLoggerConfigSettings
        {
            [JsonProperty("location")]
            public string Location { get; set; }

            [JsonProperty("safeMode")]
            public bool? SafeMode { get; set; }

            [JsonProperty("appSource")]
            public string AppSource { get; set; }

            [JsonProperty("fileSystemMode")]
            public bool? FileSystemMode { get; set; }

            [JsonProperty("sensitivity")]
            public XLoggerLegacy.Enums.LogStatus? Sensitivity { get; set; }

            [JsonProperty("autoClean")]
            public bool? AutoClean { get; set; }

            [JsonProperty("autoCleanAge")]
            public int? AutoCleanAge { get; set; }
        }

        #endregion
        #region Events

        private delegate void LogMessageHandler( XLogEntry entry , ILogFormatter formatter );
        private static event LogMessageHandler LogMessage;

        private delegate bool CleanLogHandler( int logAge );
        private static event CleanLogHandler CleanLog;

        #endregion
        #region Properties

        private static ILogFormatter _Formatter;
        private static object _lock = new object();

        #endregion

        static XLoggerLegacy()
        {
            Initialize();
        }

        #region Logging ...

        public static bool Trace( string message )
        {
            return Trace( message , XLoggerLegacy.Enums.LogSettings.Normal );
        }
        public static bool Trace( string message , params string[] parameters )
        {
            return Trace( HandleStringParameters( message , parameters ) );
        }
        public static bool Trace( string message , XLoggerLegacy.Enums.LogSettings settings )
        {
            Log( new XLogEntry( DateTime.UtcNow, message , "" , Enums.LogStatus.Trace , settings ) );
            return true;
        }

        public static bool Info( string message )
        {
            return Info( message , XLoggerLegacy.Enums.LogSettings.Normal );
        }
        public static bool Info( string message , params string[] parameters )
        {
            return Info( HandleStringParameters( message , parameters ) );
        }
        public static bool Info( string message , XLoggerLegacy.Enums.LogSettings settings )
        {
            Log( new XLogEntry( DateTime.UtcNow, message , "" , Enums.LogStatus.Info , settings ) );
            return true;
        }

        public static bool Warning( string message )
        {
            return Warning( message , Enums.LogSettings.Normal );
        }
        public static bool Warning( string message , params string[] parameters )
        {
            return Warning( HandleStringParameters( message , parameters ) );
        }
        public static bool Warning( string message , XLoggerLegacy.Enums.LogSettings settings )
        {
            Log( new XLogEntry( DateTime.UtcNow, message , "" , Enums.LogStatus.Warning , settings ) );
            return true;
        }

        public static bool Error( string message )
        {
            return Error( message , XLoggerLegacy.Enums.LogSettings.Normal );
        }
        public static bool Error( string message , params string[] parameters )
        {
            return Error( HandleStringParameters( message , parameters ) );
        }
        public static bool Error( string message , XLoggerLegacy.Enums.LogSettings settings )
        {
            Log( new XLogEntry( DateTime.UtcNow, message , "" , Enums.LogStatus.Error , settings ) );
            return false;
        }

        public static bool Error( string message , Exception exception )
        {
            return Error( message , exception , "" );
        }
        public static bool Error( string message , Exception exception , string context )
        {
            try
            {
                string exceptionString = exception.ToString();
                Exception exp = exception;
                while ( true )
                {
                    if ( exp.InnerException != null )
                    {
                        exceptionString += "\r\nINNER: " + exp.InnerException;
                        exp = exp.InnerException;
                    }
                    else break;
                }

                Log( new XLogEntry( DateTime.UtcNow, ( message ?? "" ) + exceptionString , context , Enums.LogStatus.Error ) );
                return false;

            }
            catch
            {
                return false;
            }
        }

        private static bool Log( IList<XLogEntry> logEntries )
        {
            if ( logEntries == null ) return true;

            for ( int i = 0 ; i < logEntries.Count ; i++ )
            {
                Log( logEntries[i] );
            }

            return true;
        }
        private static void Log( XLogEntry logEntry )
        {
            try
            {
                if ( !Settings.Enabled || logEntry.Status < Settings.Sensitivity || LogMessage == null ) return;

                // context ...
                logEntry.Context = GetContext( logEntry.LogSettings );

                // log ...

                lock ( _lock )
                {
                    LogMessage( logEntry , _Formatter );
                }

            }
            catch
            {
                return;
            }
        }

        #endregion
        #region Helpers

        private static bool Initialize()
        {
            // initialize config ...
            bool status = Configuration.GetStatus();

            // initialize formatter
            switch ( Settings.Format )
            {
                case Enums.LogFormat.Text: _Formatter = new TextFormatter(); break;
                case Enums.LogFormat.Xml: _Formatter = new XmlFormatter(); break;
            }

            // initialize log targets ...
            try
            {
                ILogTarget logger = ( Settings.Type & Enums.LogType.File ) == Enums.LogType.File ? ( ILogTarget ) new FileLog() : null;
                //( Settings.Type & Enums.LogType.EnterpriseLibrary ) == Enums.LogType.EnterpriseLibrary ? ( ILogTarget ) new EnterpriseLibraryLog() : null;

                LogMessage += logger.Log;
                CleanLog += logger.Clean;

                //if ( status ) CleanLogsAsync();

                return status;
            }
            catch ( Exception x )
            {
                return Error( "XLogger ... Exception: " + x , Enums.LogSettings.TransientCaller );
            }
        }

        private static string GetContext()
        {
            return GetContext( Enums.LogSettings.Normal );
        }
        private static string GetContext( Enums.LogSettings logSettings )
        {
            int stackDepth = logSettings == Enums.LogSettings.TransientCaller ? 6 : 4;

            var x = new StackTrace();

            string className = new StackTrace().GetFrame( stackDepth ).GetMethod().ReflectedType.FullName;
            if ( className.EndsWith( typeof( XLogger ).Name ) )
            {
                className = new StackTrace().GetFrame( ++stackDepth ).GetMethod().ReflectedType.FullName;
            }

            string methodName = new StackTrace().GetFrame( stackDepth ).GetMethod().Name;

            return string.Format( "{0}.{1}" , className.Substring( className.LastIndexOf( '.' ) + 1 ) , methodName );
        }

        private static string GetCallStack()
        {
            var trace = new StringBuilder();
            var stackTrace = new StackTrace();

            for ( int i = 3 ; i <= stackTrace.FrameCount && i < 13 ; i++ )
            {
                trace.Append( stackTrace.GetFrame( i ).GetMethod().Module.Name + " - " + stackTrace.GetFrame( i ).GetMethod().Name + ", " );
            }
            return trace.ToString();
        }
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
                Error( "XLogger.HandleStringParameters ... Exception: " + x , Enums.LogSettings.TransientCaller );
            }

            return message;
        }

        private static async void CleanLogsAsync()
        {
            if ( Settings.LogCleaningEnabled )
            {
                await Task.Run( () => CleanLogs() );
            }
        }
        private static async Task CleanLogs()
        {
            try
            {
                while ( true )
                {
                    await Task.Delay( new TimeSpan( 24 , 0 , 0 ) );
                    CleanLog( Settings.LogAge );
                }
            }
            catch
            {
            }
        }

        #endregion
    }
}
