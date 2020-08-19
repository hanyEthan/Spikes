using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Mcs.Invoicing.Services.Config.Api.IOC.Versioning
{
    public static class VersioningStartupHelpers
    {
        public static IServiceCollection AddVersioning(this IServiceCollection services, IConfiguration configuration)
        {
            return services.AddApiVersioning(options =>
            {
                options.AssumeDefaultVersionWhenUnspecified = true;
            });
        }
    }
}
