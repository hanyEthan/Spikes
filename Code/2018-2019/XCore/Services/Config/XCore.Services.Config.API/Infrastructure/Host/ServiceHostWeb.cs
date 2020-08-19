using System;
using System.IO;
using Microsoft.AspNetCore.Hosting;
using XCore.Framework.Utilities;

namespace XCore.Services.Config.API.Infrastructure.Host
{
    public class ServiceHostWeb
    {
        #region props.

        public bool Initialized { get; private set; }

        private IWebHost _WebHost;

        #endregion
        #region cst.

        public ServiceHostWeb(int port , string basePath = null)
        {
            this.Initialized = Initialize(port, basePath);
        }

        #endregion
        #region helpers.

        private bool Initialize(int port, string basePath = null)
        {
            try
            {
                basePath = string.IsNullOrWhiteSpace(basePath)
                         ? Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location)
                         : basePath;

                this._WebHost = new WebHostBuilder()
                                .UseKestrel()
                                .UseUrls($"http://*:{port}/")
                                .UseContentRoot(Directory.GetCurrentDirectory())
                                 //.ConfigureLogging((hostingContext, logging) =>
                                 //{
                                 //    AddLogProvider(logging);
                                 //})
                                .UseStartup<ServiceHostWebStartup>()
                                .Build();

                return true;
            }
            catch (Exception x)
            {
                XLogger.Error($"Exception : {x}");
                return false;
            }
        }
        //private static void AddLogProvider(ILoggingBuilder logging)
        //{
        //    string LogFile = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), "app.log");

        //    var logger = new LoggerConfiguration()
        //   .WriteTo.RollingFile(LogFile)
        //   .CreateLogger();
        //    logging.AddProvider(new SerilogLoggerProvider(logger));
        //}

        #endregion
        #region publics.

        public void Start()
        {
            _WebHost.Run();
        }
        public async void Stop()
        {
            await _WebHost.StopAsync();
        }

        #endregion
    }
}
