using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using XCore.Framework.Infrastructure.Config.Contracts;
using XCore.Framework.Infrastructure.Entities.Validation.Contracts;
using XCore.Services.Clients.Extensions.DepencyInjection;
using XCore.Services.Security.Core.Contracts;
using XCore.Services.Security.Core.DataLayer.Context;
using XCore.Services.Security.Core.DataLayer.Contracts;
using XCore.Services.Security.Core.DataLayer.Repositories;
using XCore.Services.Security.Core.DataLayer.Unity;
using XCore.Services.Security.Core.Handlers;
using XCore.Services.Security.Core.Models.Domain;
using XCore.Services.Security.Core.Support.Config;
using XCore.Services.Security.Core.Validators;

namespace XCore.Services.Security.Core.Extentions.DepencyInjection
{
    public static class DIExtensions
    {
        #region publics.

        public static IServiceCollection AddXCoreSecurityService(this IServiceCollection services, IConfiguration configuration)
        {
            return services.AddXCoreServices(configuration)
                           .AddSecurityDL(configuration)
                           .AddSecurityBL(configuration);
                           //.AddSecurityEvents(configuration);
        }

        #endregion
        #region helpers.

        private static IServiceCollection AddSecurityDL(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddSingleton<IConfigProvider<SecurityServiceConfig>, SecurityServiceRemoteConfigProvider>();

            var sp = services.BuildServiceProvider();
            var configProvider = sp.GetService<IConfigProvider<SecurityServiceConfig>>();
            var connectionString = configProvider?.GetConfigAsync().GetAwaiter().GetResult()?.DefaultConnectionString;

            return services.AddDbContextPool<SecurityDataContext>(options => options.UseSqlServer(connectionString))
                           .AddScoped<IAppsRepository, AppsRepository>()
                           .AddScoped<IRoleRepository, RoleRepository>()
                           .AddScoped<IPrivilegeRepository, PrivilegeRepository>()
                           .AddScoped<ITargetRepository, TargetRepository>()
                           .AddScoped<IActorRepository, ActorRepository>()
                           .AddScoped<IClaimRepository, ClaimRepository>()
                           .AddScoped<ISecurityDataUnity, SecurityDataUnity>();
        }
        private static IServiceCollection AddSecurityBL(this IServiceCollection services, IConfiguration configuration)
        {
            return services.AddScoped<IModelValidator<App>, AppsValidators>()
                           .AddScoped<IModelValidator<Actor>, ActorsValidators>()
                           .AddScoped<IModelValidator<Role>, RolesValidators>()
                           .AddScoped<IModelValidator<Claim>, ClaimsValidators>()
                           .AddScoped<IAppHandler, AppHandler>()
                           .AddScoped<IActorHandler, ActorHandler>()
                           .AddScoped<IRoleHandler, RoleHandler>()
                           .AddScoped<IClaimHandler, ClaimHandler>()
                           .AddScoped<IPrivilegeHandler, PrivilegeHandler>()
                           .AddScoped<ITargetHandler, TargetHandler>();





        }
        //private static IServiceCollection AddSecurityEvents(this IServiceCollection services, IConfiguration configuration)
        //{
        //    //return services.AddScoped<ISecurityEventsPublisher, SecurityEventsPublisher>();
        //    return services.AddSingleton<IServiceBus, ServiceBus>();
        //}

        #endregion
    }
}
