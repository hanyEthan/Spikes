using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using XCore.Framework.Framework.ServiceBus.Contracts;
using XCore.Framework.Framework.ServiceBus.Handlers;
using XCore.Framework.Framework.ServiceBus.MST.Support;
using XCore.Framework.Framework.Services.Rest;
using XCore.Framework.Infrastructure.Config.Contracts;
using XCore.Services.Attachments.SDK.Client;
using XCore.Services.Attachments.SDK.Contracts;
using XCore.Services.Attachments.SDK.Models;
using XCore.Services.Audit.SDK.Client;
using XCore.Services.Audit.SDK.Contracts;
using XCore.Services.Audit.SDK.Models;
using XCore.Services.Clients.Common.Framework.Configurations;
using XCore.Services.Configurations.SDK.Client;
using XCore.Services.Configurations.SDK.Contracts;
using XCore.Services.Configurations.SDK.Models;
using XCore.Services.Organizations.SDK.Client;
using XCore.Services.Organizations.SDK.Contracts;
using XCore.Services.Organizations.SDK.Handlers;
using XCore.Services.Organizations.SDK.Models.Support;
using XCore.Services.Docs.SDK.Client;
using XCore.Services.Docs.SDK.Contracts;
using XCore.Services.Docs.SDK.Models;
using XCore.Services.Personnel.SDK.Client;
using XCore.Services.Personnel.SDK.Contracts;
using XCore.Services.Personnel.SDK.Models;
using XCore.Services.Security.SDK;
using XCore.Services.Security.SDK.Client;
using XCore.Services.Security.SDK.Contracts;
using IOrganzationClient = XCore.Services.Organizations.SDK.Contracts.IOrganzationClient;
using OrganizationClient = XCore.Services.Organizations.SDK.Client.OrganizationClient;

namespace XCore.Services.Clients.Extensions.DepencyInjection
{
    public static class DIExtensions
    {
        #region publics.

        public static IServiceCollection AddXCoreServices(this IServiceCollection services, IConfiguration configuration)
        {
            return services.AddConfigurationService(configuration)
                           .AddAuditService(configuration)
                           .AddAttachmentService(configuration)
                           .AddOrganizationService(configuration)
                           .AddDocumentService(configuration)
                           .AddSecurityService(configuration)
                           .AddPersonnelService(configuration);
        }

        #endregion
        #region helpers.

        private static IServiceCollection AddConfigurationService(this IServiceCollection services, IConfiguration configuration)
        {
            return services.AddSingleton<IConfigProvider<ConfigClientData>, Configurations.SDK.Handlers.LocalConfigProvider>()
                           .AddSingleton<IRestHandler<ConfigClientData>, RestHandler<ConfigClientData>>()
                           .AddSingleton<IConfigClient, ConfigClient>();
        }
        private static IServiceCollection AddAuditService(this IServiceCollection services, IConfiguration configuration)
        {
            return services.AddSingleton<IConfigProvider<ServiceBusConfiguration>, RemoteConfigProvider<ServiceBusConfiguration>>()
                           .AddSingleton<IServiceBus, ServiceBus>()

                           .AddSingleton<IConfigProvider<AuditClientConfig>, Audit.SDK.Handlers.AuditClientRemoteConfigProvider>()
                           .AddSingleton<IRestHandler<AuditClientConfig>, RestHandler<AuditClientConfig>>()

                           .AddSingleton<IAuditClient, AuditClient>();
        }
        private static IServiceCollection AddAttachmentService(this IServiceCollection services, IConfiguration configuration)
        {
            return services.AddSingleton<IConfigProvider<AttachmentClientConfig>, Attachments.SDK.Handlers.AttachmentClientRemoteConfigProvider>()
                           .AddSingleton<IRestHandler<AttachmentClientConfig>, RestHandler<AttachmentClientConfig>>()
                           .AddSingleton<IAttachmentClient, AttachmentClient>();
        }
        private static IServiceCollection AddDocumentService(this IServiceCollection services, IConfiguration configuration)
        {
            return services.AddSingleton<IConfigProvider<DocumentClientConfig>, Docs.SDK.Handlers.DocumentClientRemoteConfigProvider>()
                           .AddSingleton<IRestHandler<DocumentClientConfig>, RestHandler<DocumentClientConfig>>()
                           .AddSingleton<IDocumentClient, DocumentClient>();
        }
        private static IServiceCollection AddSecurityService(this IServiceCollection services, IConfiguration configuration)
        {
            try
            {
                return services.AddSingleton<IConfigProvider<ServiceBusConfiguration>, RemoteConfigProvider<ServiceBusConfiguration>>()
                               .AddSingleton<IServiceBus, ServiceBus>()
                               .AddSingleton<IConfigProvider<SecurityClientConfig>, Security.SDK.Handlers.SecurityClientRemoteConfigProvider>()
                               .AddSingleton<IRestHandler<SecurityClientConfig>, RestHandler<SecurityClientConfig>>()
                               .AddSingleton<ISecurityClient, SecurityClient>();

            }
            catch (System.Exception e)
            {

                throw;
            }
            
        }
        private static IServiceCollection AddPersonnelService(this IServiceCollection services, IConfiguration configuration)
        {
            return services.AddSingleton<IConfigProvider<ServiceBusConfiguration>, RemoteConfigProvider<ServiceBusConfiguration>>()
                           .AddSingleton<IServiceBus, ServiceBus>()
                           .AddSingleton<IConfigProvider<PersonnelClientConfig>, Personnel.SDK.Handlers.PersonnelClientRemoteConfigProvider>()
                           .AddSingleton<IRestHandler<PersonnelClientConfig>, RestHandler<PersonnelClientConfig>>()
                           .AddSingleton<IOrganizationAccountClient, OrganizationAccountClient>()
                           .AddSingleton<IPersonnelAccountClient, PersonnelAccountClient>()
                           .AddSingleton<IDepartmentClient, DepartmentClient>()
                           .AddSingleton<IOrganzationClient, OrganizationClient>()
                           .AddSingleton<IPersonnelClient, PersonnelClient>()
                           .AddSingleton<ISettingClient, SettingClient>()
                           ;
        }


    private static IServiceCollection AddOrganizationService(this IServiceCollection services, IConfiguration configuration)
    {
        return services.AddSingleton<IConfigProvider<OrganizationClientConfig>, OrganizationClientRemoteConfigProvider>()
                       .AddSingleton<IRestHandler<OrganizationClientConfig>, RestHandler<OrganizationClientConfig>>()
                       .AddSingleton<IOrganzationClient, OrganizationClient>();
    }
    #endregion
}
}
