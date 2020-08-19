using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using XCore.Framework.Infrastructure.Entities.Validation.Contracts;
using XCore.Services.Configurations.Core.Contracts;
using XCore.Services.Configurations.Core.DataLayer.Context;
using XCore.Services.Configurations.Core.DataLayer.Contracts;
using XCore.Services.Configurations.Core.DataLayer.Repositories;
using XCore.Services.Configurations.Core.DataLayer.Unity;
using XCore.Services.Configurations.Core.Handlers;
using XCore.Services.Configurations.Core.Models.Domain;
using XCore.Services.Configurations.Core.Validators;

namespace XCore.Services.Configurations.Core.Extensions.DepencyInjection
{
    public static class DIExtensions
    {
        #region publics.

        public static IServiceCollection AddXCoreConfigurationService(this IServiceCollection services, IConfiguration configuration)
        {
            return services.AddConfigurationDL(configuration)
                           .AddConfigurationBL(configuration);
        }

        #endregion
        #region helpers.

        private static IServiceCollection AddConfigurationDL(this IServiceCollection services, IConfiguration configuration)
        {
            return services.AddDbContextPool<ConfigDataContext>(options => options.UseSqlServer(configuration.GetConnectionString(XCore.Services.Configurations.Core.Models.Support.Constants.DBConnectionString)))
                           .AddScoped<IAppsRepository, AppsRepository>()
                           .AddScoped<IConfigsRepository, ConfigsRepository>()
                           .AddScoped<IModulesRepository, ModulesRepository>()
                           .AddScoped<IConfigurationDataUnity, ConfigurationDataUnity>();
        }
        private static IServiceCollection AddConfigurationBL(this IServiceCollection services, IConfiguration configuration)
        {
            return services.AddScoped<IModelValidator<App>, AppsValidator>()
                           .AddScoped<IModelValidator<Module>, ModuleValidator>()
                           .AddScoped<IModelValidator<ConfigItem>, ConfigValidator>()
                           .AddScoped<IConfigHandler, ConfigHandler>();
        }

        #endregion
    }
}
