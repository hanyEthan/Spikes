using Mcs.Invoicing.Core.Framework.Infrastructure.Logging.Helpers;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Serilog;

namespace Mcs.Invoicing.Services.Config.Api.IOC.Logging
{
    public static class LoggingConfig
    {
        public static IServiceCollection AddSerilogSupport(this IServiceCollection services, IConfiguration configuration)
        {
            return services;
        }
        public static IApplicationBuilder UseSerilogMiddlewareSupport(this IApplicationBuilder app)
        {
            app.UseSerilogRequestLogging(x =>
            {
                x.EnrichDiagnosticContext = LoggingHelpers.EnrichFromRequest;
            });

            return app;
        }
    }
}
