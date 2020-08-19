using System.Threading.Tasks;
using XCore.Framework.Framework.Unity.Contracts;
using XCore.Framework.Infrastructure.Context.Execution.Support;
using XCore.Framework.Infrastructure.Entities.Repositories.Models;
using XCore.Services.Configurations.Core.Models.Domain;
using XCore.Services.Configurations.Core.Models.Support;

namespace XCore.Services.Configurations.Core.Contracts
{
    public interface IConfigHandler : IUnityService
    {
        #region App

        Task<ExecutionResponse<SearchResults<App>>> Get(AppSearchCriteria criteria, RequestContext requestContext);
        Task<ExecutionResponse<App>> Create(App app, RequestContext requestContext);
        Task<ExecutionResponse<App>> Edit(App app, RequestContext requestContext);
        Task<ExecutionResponse<bool>> Delete(App App, RequestContext requestContext);
        Task<ExecutionResponse<bool>> DeleteApp(int id, RequestContext requestContext);
        Task<ExecutionResponse<bool>> DeleteApp(string code, RequestContext requestContext);
        Task<ExecutionResponse<bool>> ActivateApp(int id, RequestContext requestContext);
        Task<ExecutionResponse<bool>> ActivateApp(string code, RequestContext requestContext);
        Task<ExecutionResponse<bool>> DeactivateApp(int id, RequestContext requestContext);
        Task<ExecutionResponse<bool>> DeactivateApp(string code, RequestContext requestContext);
        Task<ExecutionResponse<bool>> IsUnique(App App, RequestContext requestContext);
        Task<ExecutionResponse<bool>> IsExists(AppSearchCriteria criteria, RequestContext requestContext);

        #endregion
        #region Modules

        Task<ExecutionResponse<SearchResults<Module>>> Get(ModuleSearchCriteria criteria, RequestContext requestContext);
        Task<ExecutionResponse<Module>> Create(Module module, RequestContext requestContext);
        Task<ExecutionResponse<Module>> Edit(Module module, RequestContext requestContext);
        Task<ExecutionResponse<bool>> Delete(Module module, RequestContext requestContext);
        Task<ExecutionResponse<bool>> DeleteModule(int id, RequestContext requestContext);
        Task<ExecutionResponse<bool>> DeleteModule(string code, RequestContext requestContext);

        Task<ExecutionResponse<bool>> ActivateModule(int id, RequestContext requestContext);
        Task<ExecutionResponse<bool>> ActivateModule(string code, RequestContext requestContext);
        Task<ExecutionResponse<bool>> DeactivateModule(int id, RequestContext requestContext);
        Task<ExecutionResponse<bool>> DeactivateModule(string code, RequestContext requestContext);
        Task<ExecutionResponse<bool>> IsUnique(Module module, RequestContext requestContext);
        Task<ExecutionResponse<bool>> IsExists(ModuleSearchCriteria criteria, RequestContext requestContext);

        #endregion
        #region Config

        Task<ExecutionResponse<ConfigItem>> Get(ConfigKey key, RequestContext requestContext);
        Task<ExecutionResponse<SearchResults<ConfigItem>>> Get(ConfigSearchCriteria criteria, RequestContext requestContext);
        Task<ExecutionResponse<ConfigItem>> Create(ConfigItem config, RequestContext requestContext);
        Task<ExecutionResponse<ConfigItem>> Edit(ConfigItem config, RequestContext requestContext);
        Task<ExecutionResponse<bool>> Set(ConfigKey key, string value, RequestContext requestContext);
        Task<ExecutionResponse<bool>> Delete(ConfigItem config, RequestContext requestContext);
        Task<ExecutionResponse<bool>> DeleteConfig(int id, RequestContext requestContext);
        Task<ExecutionResponse<bool>> DeleteConfig(string code, RequestContext requestContext);
        Task<ExecutionResponse<bool>> ActivateConfig(int id, RequestContext requestContext);
        Task<ExecutionResponse<bool>> ActivateConfig(string code, RequestContext requestContext);
        Task<ExecutionResponse<bool>> DeactivateConfig(int id, RequestContext requestContext);
        Task<ExecutionResponse<bool>> DeactivateConfig(string code, RequestContext requestContext);
        Task<ExecutionResponse<bool>> IsUnique(ConfigItem config, RequestContext requestContext);
        Task<ExecutionResponse<bool>> IsExists(ConfigSearchCriteria criteria, RequestContext requestContext);

        #endregion
    }
}
