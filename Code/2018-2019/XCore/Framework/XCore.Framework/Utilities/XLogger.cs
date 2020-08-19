using System;
using System.Collections.Generic;
using System.IO;
using System.Reactive.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Serilog;
using Serilog.Events;
using Serilog.Formatting.Compact;
using Serilog.Parsing;
using XCore.Framework.Framework.ServiceBus.Handlers;
//using XCore.Framework.Framework.ServiceBus.MST.Contracts;
//using XCore.Framework.Framework.ServiceBus.MST.Support;

namespace XCore.Framework.Utilities
{
    public static class XLogger
    {
        #region props.

        private static LogMode? Mode { get; set; }
        private static Serilog.Core.Logger NativeLogger { get; set; }
        private static ServiceBus ServiceBus { get; set; }

        #endregion
        #region cst.

        static XLogger()
        {
            Initialize(new CoreNativeConfigProvider(),
                       new DefaultNativeConfigProvider());
                       //,ServiceBusConfigurationProvider.Instance);
        }

        #endregion
        #region publics.

        public static void Verbose(string message)
        {
            switch (Mode)
            {
                case LogMode.Remote:
                case LogMode.Native:
                default:
                    {
                        NativeLogger.Verbose(message);
                    }
                    break;
            }
        }
        public static void Trace(string message)
        {
            switch (Mode)
            {
                case LogMode.Remote:
                case LogMode.Native:
                default:
                    {
                        NativeLogger.Debug(message);
                    }
                    break;
            }
        }
        public static void Info(string message)
        {
            switch (Mode)
            {
                case LogMode.Remote:
                case LogMode.Native:
                default:
                    {
                        NativeLogger.Information(message);
                    }
                    break;
            }
        }
        public static void Warning(string message)
        {
            switch (Mode)
            {
                case LogMode.Remote:
                case LogMode.Native:
                default:
                    {
                        NativeLogger.Warning(message);
                    }
                    break;
            }
        }
        public static void Error(string message)
        {
            switch (Mode)
            {
                case LogMode.Remote:
                case LogMode.Native:
                default:
                    {
                        NativeLogger.Error(message);
                    }
                    break;
            }
        }
        public static void Fatal(string message)
        {
            switch (Mode)
            {
                case LogMode.Remote:
                case LogMode.Native:
                default:
                    {
                        NativeLogger.Fatal(message);
                    }
                    break;
            }
        }
        public static bool Relay(string message)
        {
            try
            {
                //using (TextReader textReader = new StringReader(message))
                //using (var reader = new Serilog.Formatting.Compact.Reader.LogEventReader(textReader))
                //{
                //    while (reader.TryRead(out Serilog.Events.LogEvent logEvent))
                //    {
                //        NativeLogger.Write(logEvent);
                //    }
                //}

                var logEvent = XSerialize.JSON.Deserialize<XLogEvent>(message);
                var logEventNative = logEvent?.Map();

                NativeLogger.Write(logEventNative);

                return true;
            }
            catch (Exception x)
            {
                return false;
            }
        }

        public static bool Configure(LoggerConfig config)
        {
            try
            {
                if (config == null) return false;

                Mode = config.Mode;
                return Initialize(new CoreNativeConfigProvider(config.NativeConfig),
                                  new DefaultNativeConfigProvider());
                                  //,ServiceBusConfigurationProvider.Instance);
            }
            catch (Exception x)
            {
                //return false;
                throw;
            }
        }
        public static void Dispose()
        {
            try
            {
                if (Mode == LogMode.Native)
                {
                    NativeLogger.Dispose();
                }
            }
            catch (Exception x)
            {
                //return false;
                throw;
            }
        }

        #endregion

        #region Helpers

        private static bool Initialize(INativeConfigProvider nativeConfigProvider, INativeConfigProvider backupNativeConfigProvider/*, IServiceBusConfiguration serviceBusConfigProvider = null*/)
        {
            try
            {
                ConfigureMode(nativeConfigProvider, backupNativeConfigProvider);

                switch (Mode)
                {
                    case LogMode.Native:
                        {
                            return ConfigureNativeLogger(nativeConfigProvider, backupNativeConfigProvider);
                        }
                    case LogMode.Remote:
                        //{
                        //    return ConfigureServiceBus(serviceBusConfigProvider);
                        //}
                    default:
                        {
                            throw new Exception("Invalid logging mode.");
                        }
                }
            }
            catch (Exception x)
            {
                return false;
            }
        }
        private static bool ConfigureNativeLogger(INativeConfigProvider nativeConfigProvider, INativeConfigProvider backupNativeConfigProvider = null)
        {
            try
            {
                var configProvider = (nativeConfigProvider?.Exists()).GetValueOrDefault()
                                   ? nativeConfigProvider?.GetConfig()
                                   : backupNativeConfigProvider?.GetConfig();

                NativeLogger = new Serilog.LoggerConfiguration()
                                          .ReadFrom.Configuration(configProvider)
                                          //.Enrich.WithProperty("App", ".")
                                          .CreateLogger();

                return true;
            }
            catch (Exception x)
            {
                return false;
            }
        }
        //private static bool ConfigureServiceBus(IServiceBusConfiguration serviceBusConfigProvider)
        //{
        //    try
        //    {
        //        NativeLogger = new Serilog.LoggerConfiguration()
        //                                  .WriteTo.Observers(events => events
        //                                  .Do(evnt =>
        //                                  {
        //                                      //var stringBuilder = new StringBuilder();
        //                                      //using (var writer = new StringWriter(stringBuilder))
        //                                      //{
        //                                      //    var serializer = new CompactJsonFormatter();
        //                                      //    serializer.Format(evnt, writer);
        //                                      //}
        //                                      //PublishLog(stringBuilder.ToString());

        //                                      PublishLog(XSerialize.JSON.Serialize(evnt));
        //                                  })
        //                                  .Subscribe())
        //                                  .CreateLogger();

        //        ServiceBus = new ServiceBus(serviceBusConfigProvider);
        //        ServiceBus.Start();

        //        return true;
        //    }
        //    catch (Exception x)
        //    {
        //        return false;
        //    }
        //}
        private static void ConfigureMode(INativeConfigProvider nativeConfigProvider, INativeConfigProvider backupNativeConfigProvider)
        {
            // ...
            if (Mode.HasValue) return;

            // ...
            var mode = nativeConfigProvider?.GetXConfig()?.GetValue<string>("XLogger:Mode")
                    ?? backupNativeConfigProvider?.GetXConfig()?.GetValue<string>("XLogger:Mode");

            // ...
            Mode = int.TryParse(mode, out int modeInt) ? (LogMode)modeInt : LogMode.Native;
        }

        private static async Task<bool> PublishLog(string message)
        {
            try
            {
                if (Mode != LogMode.Remote) return false;
                await ServiceBus.Publish<ILogMessage, LogMessage>(Map(message));

                return true;
            }
            catch (Exception)
            {
                throw;
            }
        }
        private static LogMessage Map(string from)
        {
            if (from == null) return null;

            var to = new LogMessage();

            to.Message = from;

            return to;
        }

        #endregion
        #region nested.

        #region native . logger . config

        public interface INativeConfigProvider
        {
            IConfiguration GetConfig();
            XConfig GetXConfig();
            bool Exists();
        }
        public class CoreNativeConfigProvider : INativeConfigProvider
        {
            #region props.

            private const string ConfigSection = "Serilog";
            private XConfig Config;

            #endregion
            #region cst.

            public CoreNativeConfigProvider()
            {
                this.Config = new XConfig();
            }
            public CoreNativeConfigProvider(string jsonNativeConfig)
            {
                if (string.IsNullOrWhiteSpace(jsonNativeConfig)) throw new Exception("Invalid config");
                this.Config = new XConfig("in.memory.json", jsonNativeConfig);
            }

            #endregion
            #region INativeConfigProvider

            public IConfiguration GetConfig()
            {
                return this.Config.Configuration;
            }
            public XConfig GetXConfig()
            {
                return this.Config;
            }
            public bool Exists()
            {
                return this.Config.Exists(ConfigSection);
            }

            #endregion
        }
        public class DefaultNativeConfigProvider : INativeConfigProvider
        {
            #region props.

            private const string ConfigSection = "Serilog";
            private const string json = @"";
            private XConfig Config;

            #endregion
            #region cst.

            public DefaultNativeConfigProvider()
            {
                this.Config = new XConfig("in.memory.json", GetDefaultConfig());
            }

            #endregion
            #region INativeConfigProvider

            public IConfiguration GetConfig()
            {
                return this.Config.Configuration;
            }
            public XConfig GetXConfig()
            {
                return this.Config;
            }
            public bool Exists()
            {
                return this.Config.Exists(ConfigSection);
            }

            #endregion
            #region helpers.

            private string GetDefaultConfig()
            {
                using (Stream stream = Assembly.GetExecutingAssembly().GetManifestResourceStream(FormatResourceName(Assembly.GetExecutingAssembly(), @"Utilities/XLogger.defaults.json")))
                {
                    if (stream == null) return null;

                    using (StreamReader reader = new StreamReader(stream))
                    {
                        return reader.ReadToEnd();
                    }
                }
            }
            private static string FormatResourceName(Assembly assembly, string resourceName)
            {
                return assembly.GetName().Name + "." + resourceName.Replace(" ", "_")
                                                                   .Replace("\\", ".")
                                                                   .Replace("/", ".");
            }

            #endregion
        }

        #endregion
        #region logger . config

        public enum LogMode
        {
            Native = 0,
            Remote = 1,
        }
        public class LoggerConfig
        {
            public LogMode Mode { get; set; }
            public string NativeConfig { get; set; }
        }

        #endregion
        #region logger . service bus

        public interface ILogMessage
        {
            string Message { get; set; }
        }
        public class LogMessage : ILogMessage
        {
            public string Message { get; set; }
        }
        public class XLogEvent
        {
            #region props.

            public DateTimeOffset Timestamp { get; set; }
            public LogEventLevel Level { get; set; }
            public XMessageTemplate MessageTemplate { get; set; }
            public IReadOnlyDictionary<string, LogEventPropertyValue> Properties { get; set; }
            public IEnumerable<LogEventProperty> PropertiesList { get; set; }
            public Exception Exception { get; set; }

            #endregion
            #region cst.

            public XLogEvent()
            {
            }

            #endregion
            #region publics.

            public Serilog.Events.LogEvent Map()
            {
                return new Serilog.Events.LogEvent(this.Timestamp, 
                                                   this.Level, 
                                                   this.Exception, 
                                                   new Serilog.Events.MessageTemplate(this.MessageTemplate.text,this.MessageTemplate.Tokens), 
                                                   this.PropertiesList ?? new List<LogEventProperty>());
            }

            #endregion
            #region nested.

            public class XMessageTemplate
            {
                public string text { get; set; }
                public IEnumerable<TextToken> Tokens { get; set; }
            }

            #endregion
        }

        #endregion

        #endregion
    }
}
