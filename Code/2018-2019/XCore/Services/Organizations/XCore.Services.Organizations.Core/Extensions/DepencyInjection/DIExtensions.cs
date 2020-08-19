using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using XCore.Framework.Infrastructure.Config.Contracts;
using XCore.Framework.Infrastructure.Entities.Validation.Contracts;
using XCore.Services.Clients.Extensions.DepencyInjection;
using XCore.Services.Organizations.Core.Contracts;
using XCore.Services.Organizations.Core.DataLayer.Context;
using XCore.Services.Organizations.Core.DataLayer.Contracts;
using XCore.Services.Organizations.Core.DataLayer.Repositories;
using XCore.Services.Organizations.Core.DataLayer.Unity;
using XCore.Services.Organizations.Core.Handlers;
using XCore.Services.Organizations.Core.Models.Domain;
using XCore.Services.Organizations.Core.Support.Config;
using XCore.Services.Organizations.Core.Validators;

namespace XCore.Services.Core.Extensions.DepencyInjection
{
    public static class DIExtensions
    {
        #region publics.

        public static IServiceCollection AddXCoreOrganizationService(this IServiceCollection services, IConfiguration configuration)
        {
            return services.AddXCoreServices(configuration)
                           .AddOrganizationDL(configuration)
                           .AddOrganizationBL(configuration);
        }

        #endregion
        #region helpers.

        private static IServiceCollection AddOrganizationDL(this IServiceCollection services, IConfiguration configuration)
        {
            // connection string ...
            services.AddSingleton<IConfigProvider<OrganizationServiceConfig>, OrganizationServiceRemoteConfigProvider>();

            var sp = services.BuildServiceProvider();
            var configProvider = sp.GetService<IConfigProvider<OrganizationServiceConfig>>();
            var connectionString = configProvider?.GetConfigAsync().GetAwaiter().GetResult()?.DefaultConnectionString;

            // ...
            return services.AddDbContextPool<OrganizationDataContext>(options => options.UseSqlServer(connectionString))
                           .AddScoped<IOrganizationRepository, OrganizationRepository>()
                           .AddScoped<IDepartmentRepository, DepartmentRepository>()
                            .AddScoped<ISettingsRepository, SettingsRepository>()
                            .AddScoped<IVenueRepository, VenueRepository>()
                            .AddScoped<ICityRepository, CityRepository>()
                            .AddScoped<IRoleRepository, RoleRepository>()
                            .AddScoped<IEventRepository, EventRepository>()
                            .AddScoped<IOrganizationDelegationRepository, OrganizationDelegationRepository>()
                           .AddScoped<IOrganizationDataUnity, OrganizationDataUnity>();
        }
        private static IServiceCollection AddOrganizationBL(this IServiceCollection services, IConfiguration configuration)
        {
            return services.AddScoped<IModelValidator<Organization>, OrganizationValidator>()
                           .AddScoped<IModelValidator<Department>, DepartmentValidator>()
                           .AddScoped<IModelValidator<Settings>, SettingsValidator>()
                           .AddScoped<IModelValidator<Venue>, VenueValidator>()
                           .AddScoped<IModelValidator<City>, CityValidator>()
                           .AddScoped<IModelValidator<Role>, RoleValidator>()
                           .AddScoped<IModelValidator<Event>, EventValidator>()
                           .AddScoped<IModelValidator<Settings>, SettingsValidator>()
                           .AddScoped<IModelValidator<OrganizationDelegation>, OrganizationDelegationValidator>()
                           .AddScoped<IOrganizationHandler, OrganizationsHandler>()
                           .AddScoped<IDepartmentHandler, DepartmentsHandler>()
                           .AddScoped<ISettingsHandler, SettingsHandler>()
                           .AddScoped<IOrganizationDelegationHandler, DelegatesHandler>()
                           .AddScoped<IVenueHandler, VenueHandler>()
                           .AddScoped<IRoleHandler, RoleHandler>()
                           .AddScoped<IEventHandler, EventHandler>()
                           .AddScoped<ICityHandler, CityHandler>();



        }

        #endregion
    }
}
