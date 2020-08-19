using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using XCore.Framework.Infrastructure.Entities.Validation.Contracts;
using XCore.Services.Personnel.Core.Contracts.Departments;
using XCore.Services.Personnel.Core.Contracts.Organizations;
using XCore.Services.Personnel.Core.Contracts.Personnels;
using XCore.Services.Personnel.Core.Contracts.Accounts;
using XCore.Services.Personnel.Core.Contracts.Settings;
using XCore.Services.Personnel.Core.Handlers.Departments;
using XCore.Services.Personnel.Core.Handlers.Organizations;
using XCore.Services.Personnel.Core.Handlers.Personnels;
using XCore.Services.Personnel.Core.Handlers.Accounts;
using XCore.Services.Personnel.Core.Handlers.Settings;
using XCore.Services.Personnel.Core.Validators;
using XCore.Services.Personnel.Models.Departments;
using XCore.Services.Personnel.Models.Organizations;
using XCore.Services.Personnel.Models.Personnels;
using XCore.Services.Personnel.Models.Settings;
using XCore.Services.Personnel.Models.Accounts;
using XCore.Services.Clients.Extensions.DepencyInjection;
using XCore.Framework.Infrastructure.Config.Contracts;
using XCore.Services.Personnel.Core.Support.Config;
using XCore.Services.Personnel.DataLayer.Context;
using XCore.Services.Personnel.DataLayer.Contracts.Departments;
using XCore.Services.Personnel.DataLayer.Contracts.Organizations;
using XCore.Services.Personnel.DataLayer.Contracts.Personnels;
using XCore.Services.Personnel.DataLayer.Contracts.Accounts;
using XCore.Services.Personnel.DataLayer.Contracts.Settings;
using XCore.Services.Personnel.DataLayer.Contracts;
using XCore.Services.Personnel.DataLayer.Repositories.Departments;
using XCore.Services.Personnel.DataLayer.Repositories.Organizations;
using XCore.Services.Personnel.DataLayer.Repositories.Personnels;
using XCore.Services.Personnel.DataLayer.Repositories.Accounts;
using XCore.Services.Personnel.DataLayer.Repositories.Settings;
using XCore.Services.Personnel.DataLayer.Unity;

namespace XCore.Services.Personnel.Core.Extensions.DepencyInjection
{
    public static class DIExtensions
    {
        #region publics.

        public static IServiceCollection AddXCorePersonnelService(this IServiceCollection services, IConfiguration configuration)
        {
            return services.AddXCoreServices(configuration)
                           .AddConfigurationDL(configuration)
                           .AddConfigurationBL(configuration);

        }

        #endregion
        #region helpers.

        private static IServiceCollection AddConfigurationDL(this IServiceCollection services, IConfiguration configuration)
        {
            // connection string ...
            services.AddSingleton<IConfigProvider<PersonnelServiceConfig>, PersonnelServiceRemoteConfigProvider>();

            var sp = services.BuildServiceProvider();
            var configProvider = sp.GetService<IConfigProvider<PersonnelServiceConfig>>();
            var connectionString = configProvider?.GetConfigAsync().GetAwaiter().GetResult()?.DefaultConnectionString;

            // ...
            return services.AddDbContextPool<PersonnelDataContext>(options => options.UseSqlServer(connectionString))
                            .AddScoped<IDepartmentRepository, DepartmentRepository>()
                        .AddScoped<IOrganizationRepository, OrganizationRepository>()
                        .AddScoped<IPersonnelRepository, PersonnelRepository>()
                        .AddScoped<IPersonnelAccountRepository, PersonalAccountRepository>()
                        .AddScoped<IOrganizationAccountRepository, OrganizationAccountRepository>()
                        .AddScoped<ISettingRepository, SettingRepository>()
                        .AddScoped<IPersonnelDataUnity, PersonnelDataUnity>();

        }
       
        private static IServiceCollection AddConfigurationBL(this IServiceCollection services, IConfiguration configuration)
        {
          
            return services.AddScoped<IModelValidator<Department>, DepartmentValidator>()
                         .AddScoped<IModelValidator<Organization>, OrganizationValidator>()
                         .AddScoped<IModelValidator<Person>, PersonnelValidator>()
                         .AddScoped<IModelValidator<OrganizationAccount>, OrganizationAccountValidator>()
                         .AddScoped<IModelValidator<PersonnelAccount>, PersonnelAccountValidator>()
                         .AddScoped<IModelValidator<Setting>, SettingValidator>()
                         .AddScoped<IDepartmentHandler, DepartmentHandler>()
                         .AddScoped<IOrganizationHandler, OrganizationHandler>()
                         .AddScoped<IPersonnelHandler, PersonnelHandler>()
                         .AddScoped< IPersonnelAccountHandler, PersonnelAccountHandler >()
                         .AddScoped<IOrganizationAccountHandler, OrganizationAccountHandler>()
                         .AddScoped<ISettingHandler, SettingHandler>();
        }
        
        #endregion
    }
}
