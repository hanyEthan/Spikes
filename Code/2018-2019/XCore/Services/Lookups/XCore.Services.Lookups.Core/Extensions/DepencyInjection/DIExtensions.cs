using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using XCore.Framework.Infrastructure.Config.Contracts;
using XCore.Framework.Infrastructure.Entities.Validation.Contracts;
using XCore.Services.Clients.Extensions.DepencyInjection;
using XCore.Services.Lookups.Core.Contracts;
using XCore.Services.Lookups.Core.DataLayer.Context;
using XCore.Services.Lookups.Core.DataLayer.Contracts;
using XCore.Services.Lookups.Core.DataLayer.Repositories;
using XCore.Services.Lookups.Core.DataLayer.Unity;
using XCore.Services.Lookups.Core.Handlers;
using XCore.Services.Lookups.Core.Support.Config;

namespace XCore.Services.Lookups.Core.Extensions.DepencyInjection
{
    public static class DIExtensions
    {
        #region publics.

        public static IServiceCollection AddXCoreAuditService(this IServiceCollection services, IConfiguration configuration)
        {
            return services.AddXCoreServices(configuration)
                           .AddDL(configuration)
                           .AddBL(configuration);
        }

        #endregion
        #region helpers.

        private static IServiceCollection AddDL(this IServiceCollection services, IConfiguration configuration)
        {
            // connection string ...
            services.AddSingleton<IConfigProvider<LookupsServiceConfig>, LookupsServiceRemoteConfigProvider>();

            var sp = services.BuildServiceProvider();
            var configProvider = sp.GetService<IConfigProvider<LookupsServiceConfig>>();
            var connectionString = configProvider?.GetConfigAsync().GetAwaiter().GetResult()?.DefaultConnectionString;

            // ...
            return services.AddDbContextPool<LookupsDataContext>(options => options.UseSqlServer(connectionString))
                           .AddScoped<ILookupCategoryRepository, LookupCategoryRepository>()
                           .AddScoped<ILookupRepository, LookupRepository>()
                           .AddScoped<ILookupsDataUnity, LookupsDataUnity>();
        }
        private static IServiceCollection AddBL(this IServiceCollection services, IConfiguration configuration)
        {
            return services.AddScoped<ILookupsHandler, LookupsHandler>();
        }

        #endregion
    }
}
