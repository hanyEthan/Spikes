using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using XCore.Framework.Infrastructure.Config.Contracts;
using XCore.Framework.Infrastructure.Entities.Validation.Contracts;
using XCore.Services.Clients.Extensions.DepencyInjection;
using XCore.Services.Notifications.Core.Contracts;
using XCore.Services.Notifications.Core.DataLayer.Context;
using XCore.Services.Notifications.Core.DataLayer.Contracts;
using XCore.Services.Notifications.Core.DataLayer.Repositories;
using XCore.Services.Notifications.Core.DataLayer.Unity;
using XCore.Services.Notifications.Core.Handlers;
using XCore.Services.Notifications.Core.Models.Domain;
using XCore.Services.Notifications.Core.Models.Support;
using XCore.Services.Notifications.Core.Models.Support.Config;
using XCore.Services.Notifications.Core.Validators;


namespace XCore.Services.Notifications.Core.Extentions.DepencyInjection
{
    public static class DIExtensions
    {
        #region publics.

        public static IServiceCollection AddXCoreNotificationService(this IServiceCollection services, IConfiguration configuration)
        {
            return services.AddXCoreServices(configuration)
                           .AddNotificationsDL(configuration)
                           .AddNotificationsBL(configuration);
        }

        #endregion
        #region helpers.

        private static IServiceCollection AddNotificationsDL(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddSingleton<IConfigProvider<NotificationsServiceConfig>, NotificationsServiceRemoteConfigProvider>();

            var sp = services.BuildServiceProvider();
            var configProvider = sp.GetService<IConfigProvider<NotificationsServiceConfig>>();
            var connectionString = configProvider?.GetConfigAsync().GetAwaiter().GetResult()?.DefaultConnectionString;

            return services.AddDbContextPool<NotificationsDataContext>(options => options.UseSqlServer(connectionString))
                           .AddScoped<IMessageTemplateRepository, MessageTemplateRepository>()
                           .AddScoped<IInternalNotificationRepository,InternalNotificationRepository>()
                           .AddScoped<INotificationsDataUnity, NotificationsDataUnity>();
                           
                    
        }
        private static IServiceCollection AddNotificationsBL(this IServiceCollection services, IConfiguration configuration)
        {
            return services.AddScoped<IModelValidator<MessageTemplate>, MessageTemplateValidator>()
                            .AddScoped<IModelValidator<ResolveRequest>, ResolveRequestValidator>()
                            .AddScoped<IModelValidator<InternalNotification>, InternalNotificationValidator>()
                            .AddScoped<IInternalNotificationHandler, InternalNotificationHandler>()
                           .AddScoped<IMessageTemplatesHandler, MessageTemplatesHandler>();
                           
            
        }

        #endregion
    }
}
