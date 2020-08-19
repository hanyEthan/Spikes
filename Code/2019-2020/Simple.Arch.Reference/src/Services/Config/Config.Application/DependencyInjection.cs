using System.Reflection;
using Mcs.Invoicing.Core.Framework.Infrastructure.Context.Behaviours;
using Mcs.Invoicing.Services.Audit.Client.Sdk.Configurations;
using Mcs.Invoicing.Services.Config.Application.Services.ConfigItems.Commands.CreateConfigItem;
using Mcs.Invoicing.Services.Config.Application.Services.ConfigItems.Queries.AnyConfigItems;
using Mcs.Invoicing.Services.Config.Application.Services.ConfigItems.Queries.ListConfigItems;
using Mcs.Invoicing.Services.Config.Application.Services.Modules.Queries.AnyModules;
using Mcs.Invoicing.Services.Config.Domain.Entities;
using Mcs.Invoicing.Services.Config.Domain.Support;
using Mcs.Invoicing.Services.Core.Framework.Infrastructure.Models.Mappers;
using MediatR;
using MediatR.Pipeline;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Mcs.Invoicing.Services.Config.Application
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddApplication(this IServiceCollection services, IConfiguration configuration)
        {
            return services.AddMediatRSupport(configuration)
                           .AddMappings(configuration);
        }

        #region MediatR

        private static IServiceCollection AddMediatRSupport(this IServiceCollection services, IConfiguration configuration)
        {
            return services.AddMediatR(Assembly.GetExecutingAssembly())
                           .AddTransient(typeof(IRequestPreProcessor<>), typeof(RequestLogger<>))
                           .AddTransient(typeof(IPipelineBehavior<,>), typeof(RequestPerformanceBehaviour<,>))
                           .AddTransient(typeof(IPipelineBehavior<,>), typeof(RequestValidationBehavior<,>));
        }

        #endregion
        #region Mapping

        private static IServiceCollection AddMappings(this IServiceCollection services, IConfiguration configuration)
        {
            return services.AddSingleton<IModelMapper<AnyModulesQuery, ModulesSearchCriteria>, AnyModulesQueryMapper>()
                           .AddSingleton<IModelMapper<AnyConfigItemsQuery, ConfigItemsSearchCriteria>, AnyConfigItemsQueryMapper>()
                           .AddSingleton<IModelMapper<CreateConfigItemCommand, ConfigItem>, CreateConfigItemCommandMapper > ()
                           .AddSingleton<IModelMapper<ListConfigItemsQuery, ConfigItemsSearchCriteria>, ListConfigItemsQueryMapper>();
        }

        #endregion
    }
}
