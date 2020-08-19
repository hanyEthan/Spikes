using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using XCore.Services.Docs.Core.Contracts;
using XCore.Services.Docs.Core.DataLayer.Context;
using XCore.Services.Docs.Core.DataLayer.Contracts;
using XCore.Services.Docs.Core.DataLayer.Unity;
using XCore.Services.Docs.Core.Handlers;
using XCore.Services.Config.Core.DataLayer.Repositories;
using XCore.Services.Clients.Extensions.DepencyInjection;
using XCore.Framework.Infrastructure.Config.Contracts;
using XCore.Services.Docs.Core.Support.Config;
using XCore.Services.Docs.Core.Validators;
using XCore.Framework.Infrastructure.Entities.Validation.Contracts;
using XCore.Services.Docs.Core.Models;

namespace XCore.Services.Docs.Core.Extensions.DepencyInjection
{
    public static class DIExtensions
    {
        #region publics.

        public static IServiceCollection AddXCoreDocumentService(this IServiceCollection services, IConfiguration configuration)
        {

            return services.AddXCoreServices(configuration)
                           .AddDocumentDL(configuration)
                           .AddDocumentBL(configuration);
        }

        #endregion
        #region helpers.

        private static IServiceCollection AddDocumentDL(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddSingleton<IConfigProvider<DocumentServiceConfig>, DocumentServiceRemoteConfigProvider>();

            var sp = services.BuildServiceProvider();
            var configProvider = sp.GetService<IConfigProvider<DocumentServiceConfig>>();
            var connectionString = configProvider?.GetConfigAsync().GetAwaiter().GetResult()?.DefaultConnectionString;

            return services.AddDbContextPool<DocumentDataContext>(options => options.UseSqlServer(connectionString))

                          .AddScoped<IDocumentRepository, DocumentRepository>()
                           .AddScoped<IDocumentDataUnity, DocumentDataUnity>();
        }
        private static IServiceCollection AddDocumentBL(this IServiceCollection services, IConfiguration configuration)
        {
            return services.AddScoped<IModelValidator<Document>, DocumentValidator>()
                            .AddScoped<IDocumentHandler, DocumentHandler>();
        }

        #endregion
    }
}
