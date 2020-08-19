using XCore.Framework.Framework.Unity.Contracts;
using XCore.Framework.Infrastructure.Context.Execution.Support;
using XCore.Framework.Infrastructure.Entities.Repositories.Models;
using XCore.Services.Config.Core.Models.Domain;
using XCore.Services.Config.Core.Models.Support;

namespace XCore.Services.Config.Core.Contracts
{
    public interface IConfigHandlerBase : IUnityService
    {
        #region App

        ExecutionResponse<SearchResults<App>> Get(AppSearchCriteria criteria, RequestContext requestContext);
        ExecutionResponse<App> Create(App app, RequestContext requestContext);
        ExecutionResponse<App> Edit(App app, RequestContext requestContext);
        ExecutionResponse<bool> Delete(App App, RequestContext requestContext);
        ExecutionResponse<bool> DeleteApp(int id, RequestContext requestContext);
        ExecutionResponse<bool> DeleteApp(string code, RequestContext requestContext);
        ExecutionResponse<bool> ActivateApp(int id, RequestContext requestContext);
        ExecutionResponse<bool> ActivateApp(string code, RequestContext requestContext);
        ExecutionResponse<bool> DeactivateApp(int id, RequestContext requestContext);
        ExecutionResponse<bool> DeactivateApp(string code, RequestContext requestContext);
        ExecutionResponse<bool> IsUnique(App App, RequestContext requestContext);
        ExecutionResponse<bool> IsExists(AppSearchCriteria criteria, RequestContext requestContext);

        #endregion
        #region Modules

        ExecutionResponse<SearchResults<Module>> Get(ModuleSearchCriteria criteria, RequestContext requestContext);
        ExecutionResponse<Module> Create(Module module, RequestContext requestContext);
        ExecutionResponse<Module> Edit(Module module, RequestContext requestContext);
        ExecutionResponse<bool> Delete(Module module, RequestContext requestContext);
        ExecutionResponse<bool> DeleteModule(int id, RequestContext requestContext);
        ExecutionResponse<bool> DeleteModule(string code, RequestContext requestContext);

        ExecutionResponse<bool> ActivateModule(int id, RequestContext requestContext);
        ExecutionResponse<bool> ActivateModule(string code, RequestContext requestContext);
        ExecutionResponse<bool> DeactivateModule(int id, RequestContext requestContext);
        ExecutionResponse<bool> DeactivateModule(string code, RequestContext requestContext);
        ExecutionResponse<bool> IsUnique(Module module, RequestContext requestContext);
        ExecutionResponse<bool> IsExists(ModuleSearchCriteria criteria, RequestContext requestContext);

        #endregion
        #region Config

        ExecutionResponse<ConfigItem> Get(ConfigKey key, RequestContext requestContext);
        ExecutionResponse<SearchResults<ConfigItem>> Get(ConfigSearchCriteria criteria, RequestContext requestContext);
        ExecutionResponse<ConfigItem> Create(ConfigItem config, RequestContext requestContext);
        ExecutionResponse<ConfigItem> Edit(ConfigItem config, RequestContext requestContext);
        ExecutionResponse<bool> Set(ConfigKey key, string value, RequestContext requestContext);
        ExecutionResponse<bool> Delete(ConfigItem config, RequestContext requestContext);
        ExecutionResponse<bool> DeleteConfig(int id, RequestContext requestContext);
        ExecutionResponse<bool> DeleteConfig(string code, RequestContext requestContext);
        ExecutionResponse<bool> ActivateConfig(int id, RequestContext requestContext);
        ExecutionResponse<bool> ActivateConfig(string code, RequestContext requestContext);
        ExecutionResponse<bool> DeactivateConfig(int id, RequestContext requestContext);
        ExecutionResponse<bool> DeactivateConfig(string code, RequestContext requestContext);
        ExecutionResponse<bool> IsUnique(ConfigItem config, RequestContext requestContext);
        ExecutionResponse<bool> IsExists(ConfigSearchCriteria criteria, RequestContext requestContext);

        #endregion
    }
}
