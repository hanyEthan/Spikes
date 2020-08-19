using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using XCore.Services.Attachments.Core.Contracts;
using XCore.Services.Attachments.Core.DataLayer.Context;
using XCore.Services.Attachments.Core.DataLayer.Contracts;
using XCore.Services.Attachments.Core.DataLayer.Unity;
using XCore.Services.Attachments.Core.Handlers;
using XCore.Services.Config.Core.DataLayer.Repositories;
using XCore.Services.Clients.Extensions.DepencyInjection;
using XCore.Framework.Infrastructure.Config.Contracts;
using XCore.Services.Attachments.Core.Support.Config;
using XCore.Services.Attachments.Core.Validators;
using XCore.Framework.Infrastructure.Entities.Validation.Contracts;
using XCore.Services.Attachments.Core.Models;
using XCore.Services.Attachments.Core.Models.Support;

namespace XCore.Services.Attachments.Core.Extensions.DepencyInjection
{
    public static class DIExtensions
    {
        #region publics.

        public static IServiceCollection AddXCoreAttachmentsService(this IServiceCollection services, IConfiguration configuration)
        {
            return services.AddXCoreServices(configuration)
                           .AddAttachmentsDL_DB(configuration)
                          // .AddAttachmentsDL_FS(configuration)
                           .AddAttachmentsBL(configuration);
        }

        #endregion
        #region helpers.

        private static IServiceCollection AddAttachmentsDL_DB(this IServiceCollection services, IConfiguration configuration)
        {
            // connection string ...
            services.AddSingleton<IConfigProvider<AttachmentServiceConfig>, AttachmentServiceRemoteConfigProvider>();

            var sp = services.BuildServiceProvider();
            var configProvider = sp.GetService<IConfigProvider<AttachmentServiceConfig>>();
            var connectionString = configProvider?.GetConfigAsync().GetAwaiter().GetResult()?.DefaultConnectionString;

            // ...
            return services.AddDbContextPool<AttachmentDataContext>(options => options.UseSqlServer(connectionString))
                           //.services.AddDbContextPool<AttachmentDataContext>(options => options.UseSqlServer(configuration.GetConnectionString(XCore.Services.Attachments.Core.Models.Constants.DBConnectionString)))
                           .AddScoped<IAttachmentRepository, AttachmentRepository>()
                           .AddScoped<IAttachmentDataUnity, AttachmentDataUnity>();
        }
        private static IServiceCollection AddAttachmentsDL_FS(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddSingleton<IConfigProvider<AttachmentServiceConfig>, AttachmentServiceRemoteConfigProvider>();
            services.AddSingleton<IConfigProvider<FileSystemDirectorySettings>, AttachmentServiceRemoteFileConfigProvider>();

            var provider = services.BuildServiceProvider();
            var configProvider = provider.GetService<IConfigProvider<FileSystemDirectorySettings>>();
            var Directory = configProvider?.GetConfigAsync().GetAwaiter().GetResult();

            return services.AddSingleton(
                  new FileSystemDirectorySettings()
                  {
                      BaseDirectoryPath = Directory?.BaseDirectoryPath, 
                      CreateDirectoryIfNotFound = Directory.CreateDirectoryIfNotFound,
                      TempDirectoryPath = Directory?.TempDirectoryPath
                  })

                  .AddScoped<IAttachmentRepository, AttachmentFileSystemRepository>()
                  .AddScoped<IAttachmentDataUnity, AttachmentDataUnity>();

        }
        private static IServiceCollection AddAttachmentsBL(this IServiceCollection services, IConfiguration configuration)
        {
            return services.AddScoped<IModelValidator<Attachment>, AttachmentValidator>()
                           .AddScoped<IAttachmentsHandler, AttachmentHandler>();
        }

        #endregion
    }
}
