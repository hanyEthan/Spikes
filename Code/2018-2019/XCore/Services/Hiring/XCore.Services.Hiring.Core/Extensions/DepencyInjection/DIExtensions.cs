using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using XCore.Framework.Infrastructure.Config.Contracts;
using XCore.Framework.Infrastructure.Entities.Validation.Contracts;
using XCore.Services.Hiring.Core.Contracts;
using XCore.Services.Hiring.Core.Validators;
using XCore.Services.Hiring.Core.DataLayer.Context;
using XCore.Services.Hiring.Core.DataLayer.Contracts;
using XCore.Services.Hirings.Core.DataLayer.Unity;
using XCore.Services.Hiring.Core.DataLayer.Repositories;
using XCore.Services.Attachments.Core.DataLayer.Contracts;
using XCore.Services.Hiring.Core.Models.Domain;
using XCore.Services.Hiring.Core.Handlers;
using XCore.Services.Hiring.Core.Support.Config;
using XCore.Services.Clients.Extensions.DepencyInjection;

namespace XCore.Services.Hiring.Core.Extensions.DepencyInjection
{
    public static class DIExtensions
    {
        #region publics.

        public static IServiceCollection AddXCoreHiringService(this IServiceCollection services, IConfiguration configuration)
        {
            return services.AddXCoreServices(configuration)
                           .AddHiringDL(configuration)
                           .AddHiringBL(configuration);
        }

        #endregion
        #region helpers.

        private static IServiceCollection AddHiringDL(this IServiceCollection services, IConfiguration configuration)
        {
            // connection string ...
            services.AddSingleton<IConfigProvider<HiringServiceConfig>, HiringServiceRemoteConfigProvider>();

            var sp = services.BuildServiceProvider();
            var configProvider = sp.GetService<IConfigProvider<HiringServiceConfig>>();
            var connectionString = configProvider?.GetConfigAsync().GetAwaiter().GetResult()?.DefaultConnectionString;

            // ...
            return services.AddDbContextPool<HiringDataContext>(options => options.UseSqlServer(connectionString))
                           .AddScoped<IAdvertisementRepository, AdvertisementRepository>()
                           .AddScoped<IApplicationRepository, ApplicationRepository>()
                           .AddScoped<ICandidateRepository, CandidateRepository>()
                           .AddScoped<IHiringProcessRepository, HiringProcessRepository>()
                           .AddScoped<IOrganizationRepository, OrganizationRepository>()
                           .AddScoped<ISkillRepository, SkillRepository>()
                           .AddScoped<IHiringDataUnity, HiringDataUnity>();
        }
        private static IServiceCollection AddHiringBL(this IServiceCollection services, IConfiguration configuration)
        {
            return services
                .AddScoped<IModelValidator<Advertisement>, AdvertisementValidator>()
                           .AddScoped<IModelValidator<Application>, ApplicationValidator>()
                           .AddScoped<IModelValidator<Candidate>, CandidateValidator>()
                           .AddScoped<IModelValidator<HiringProcess>, HiringProcessValidator>()
                           .AddScoped<IModelValidator<Organization>, OrganizationValidator>()
                           .AddScoped<IModelValidator<Skill>, SkillValidator>()
                           .AddScoped<IAdvertisementsHandler, AdvertisementsHandler>()
                           .AddScoped<IApplicationsHandler, ApplicationsHandler>()
                           .AddScoped<ICandidatesHandler, CandidatesHandler>()
                           .AddScoped<IHiringProcessesHandler, HiringProcessesHandler>()
                           .AddScoped<IOrganizationsHandler, OrganizationsHandler>()
                           .AddScoped<ISkillsHandler, SkillsHandler>();
        }

        #endregion
    }
}
