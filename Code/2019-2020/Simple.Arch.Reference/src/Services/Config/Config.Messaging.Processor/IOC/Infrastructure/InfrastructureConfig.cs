using Mcs.Invoicing.Services.Config.Application;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Mcs.Invoicing.Services.Config.Infrastructure;

namespace Config.Messaging.Processor.IOC.Infrastructure
{
    public static class InfrastructureConfig
    {
        public static IServiceCollection AddAuditInfrastructure(this IServiceCollection services, IConfiguration configuration)
        {
            return services.AddApplication(configuration)
                           .AddInfrastructure(configuration);
        }
    }
}
