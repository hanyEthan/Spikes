using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using XCore.Framework.Infrastructure.Config.Contracts;
using XCore.Framework.Infrastructure.Entities.Validation.Contracts;
using XCore.Services.Audit.Core.Contracts;
using XCore.Services.Audit.Core.DataLayer.Context;
using XCore.Services.Audit.Core.DataLayer.Contracts;
using XCore.Services.Audit.Core.DataLayer.Unity;
using XCore.Services.Audit.Core.Handlers;
using XCore.Services.Audit.Core.Models;
using XCore.Services.Audit.Core.Support.Config;
using XCore.Services.Audit.Core.Validators;
using XCore.Services.Clients.Extensions.DepencyInjection;
using XCore.Services.Config.Core.DataLayer.Repositories;

namespace XCore.Services.Audit.Core.Extensions.DepencyInjection
{
    public static class DIExtensions
    {
        #region publics.

        public static IServiceCollection AddXCoreAuditService(this IServiceCollection services, IConfiguration configuration)
        {
            return services.AddXCoreServices(configuration)
                           .AddAuditDL(configuration)
                           .AddAuditBL(configuration);
        }

        #endregion
        #region helpers.

        private static IServiceCollection AddAuditDL(this IServiceCollection services, IConfiguration configuration)
        {
            // connection string ...
            services.AddSingleton<IConfigProvider<AuditServiceConfig>, AuditServiceRemoteConfigProvider>();

            var sp = services.BuildServiceProvider();
            var configProvider = sp.GetService<IConfigProvider<AuditServiceConfig>>();
            var connectionString = configProvider?.GetConfigAsync().GetAwaiter().GetResult()?.DefaultConnectionString;
            var readconnectionString = configProvider?.GetConfigAsync().GetAwaiter().GetResult()?.ReadConnectionString;

            // ...
            return services.AddDbContextPool<AuditDataContext>(options => options.UseSqlServer(connectionString))
                           .AddDbContextPool<AuditReadDataContext>(options => options.UseSqlServer(readconnectionString))
                           .AddScoped<IAuditRepository, AuditRepository>()
                           .AddScoped<IAuditReadRepository, AuditReadRepository>()
                           .AddScoped<IAuditDataUnity, AuditDataUnity>();
        }
        private static IServiceCollection AddAuditBL(this IServiceCollection services, IConfiguration configuration)
        {
            return services.AddScoped<IModelValidator<AuditTrail>, AuditTrailValidator>()
                           .AddScoped<IAuditHandler, AuditHandler>();
        }

        #endregion
    }
}
