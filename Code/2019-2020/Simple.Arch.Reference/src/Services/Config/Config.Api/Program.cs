using Mcs.Invoicing.Core.Framework.Infrastructure.Logging.Helpers;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using Serilog;

namespace Config.Api
{
    public class Program
    {
        public static void Main(string[] args)
        {
            try
            {
                LoggingHelpers.ConfigureLogging();

                #region LOG
                Log.Information("{ServiceName} : Starting-up service.", LoggingHelpers.ServiceName);
                #endregion

                CreateHostBuilder(args).Build().Run();
            }
            catch (System.Exception x)
            {
                #region LOG
                Log.Fatal(x, "{ServiceName} : Failed to start service", LoggingHelpers.ServiceName);
                #endregion
                throw;
            }
            finally
            {
                Log.CloseAndFlush();
            }
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>()
                              .UseSerilog();
                });
    }
}
