using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Serilog;

namespace Mcs.Invoicing.PublicPortals.Bff.Api
{
    public class Program
    {
        private static string MicroserviceName
        {
            get
            {
                var assemblyName = Assembly.GetExecutingAssembly().GetName().Name;
                var serviceBaseName = string.Join('.', assemblyName.Split('.')?.Take(4));
                return serviceBaseName;
            }
        }

        public static void Main(string[] args)
        {
            ConfigureLogging();
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) => Host.CreateDefaultBuilder(args)

             .ConfigureAppConfiguration((host, config) =>
             {
                 config
                 .AddJsonFile("appsettings.json", true, true)
                 .AddJsonFile($"configuration.{host.HostingEnvironment.EnvironmentName.ToLower()}.json", false, true);
             })
            .UseSerilog()

            .ConfigureWebHostDefaults(webBuilder =>
            {
                webBuilder.UseStartup<Startup>();
            });

        private static void ConfigureLogging()
        {
            var environment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");

            var configuration = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .AddJsonFile($"appsettings.{environment}.json", optional: true)
                .Build();

            Log.Logger = new LoggerConfiguration()
                .ReadFrom.Configuration(configuration)
                .CreateLogger();
        }

    }
}
