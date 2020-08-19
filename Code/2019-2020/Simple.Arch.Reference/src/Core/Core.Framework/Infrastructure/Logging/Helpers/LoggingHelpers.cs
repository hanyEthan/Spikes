using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Serilog;

namespace Mcs.Invoicing.Core.Framework.Infrastructure.Logging.Helpers
{
    public static class LoggingHelpers
    {
        #region props.

        public static string ServiceName
        {
            get
            {
                var assemblyName = Assembly.GetEntryAssembly().GetName().Name;
                var serviceBaseName = string.Join('.', assemblyName.Split('.')?.Take(4));
                return serviceBaseName;
            }
        }

        #endregion
        #region statics.

        public static void ConfigureLogging()
        {
            var environment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");

            var configuration = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .AddJsonFile($"appsettings.{environment}.json", optional: true)
                .Build();

            Log.Logger = new LoggerConfiguration()

                .Enrich.WithProperty("Environment", environment)
                .Enrich.WithProperty("MicroserviceSource", ServiceName)
                .ReadFrom.Configuration(configuration)
                .CreateLogger();
        }
        public static void EnrichFromRequest(IDiagnosticContext diagnosticContext, HttpContext httpContext)
        {
            var request = httpContext.Request;

            // Set all the common properties available for every request
            diagnosticContext.Set("Host", request.Host);
            diagnosticContext.Set("Protocol", request.Protocol);
            diagnosticContext.Set("Scheme", request.Scheme);

            // Only set it if available. You're not sending sensitive data in a query string right?!
            if (request.QueryString.HasValue)
            {
                diagnosticContext.Set("QueryString", request.QueryString.Value);
            }

            // Set the content-type of the Response at this point
            diagnosticContext.Set("ContentType", httpContext.Response.ContentType);

            //// Retrieve the IEndpointFeature selected for the request
            //var endpoint = httpContext.GetEndpoint();
            //if (endpoint is object) // endpoint != null
            //{
            //    diagnosticContext.Set("EndpointName", endpoint.DisplayName);
            //}
        }

        #endregion
    }
}
