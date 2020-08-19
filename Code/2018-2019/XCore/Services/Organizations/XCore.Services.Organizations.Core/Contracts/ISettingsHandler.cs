using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using XCore.Framework.Framework.Unity.Contracts;
using XCore.Framework.Infrastructure.Context.Execution.Support;
using XCore.Framework.Infrastructure.Entities.Repositories.Models;
using XCore.Services.Organizations.Core.Models.Domain;
using XCore.Services.Organizations.Core.Models.Support;


namespace XCore.Services.Organizations.Core.Contracts
{
    public interface ISettingsHandler : IUnityService
    {
      
        Task<ExecutionResponse<Settings>> Create(Settings Settings, RequestContext requestContext);
        Task<ExecutionResponse<Settings>> Edit(Settings Settings, RequestContext requestContext);
        Task<ExecutionResponse<bool>> Delete(Settings settings, RequestContext requestContext);
        Task<ExecutionResponse<bool>> Delete(int id, RequestContext requestContext);
        Task<ExecutionResponse<bool>> Delete(string code, RequestContext requestContext);
        Task<ExecutionResponse<bool>> Activate(int id, RequestContext requestContext);
        Task<ExecutionResponse<bool>> Activate(string code, RequestContext requestContext);
        Task<ExecutionResponse<bool>> Deactivate(int id, RequestContext requestContext);
        Task<ExecutionResponse<bool>> Deactivate(string code, RequestContext requestContext);
        Task<ExecutionResponse<SearchResults<Settings>>> Get(SettingsSearchCriteria criteria, RequestContext requestContext);
        Task<ExecutionResponse<bool>> IsUnique(Settings Settings, RequestContext requestContext);
        Task<ExecutionResponse<bool>> IsExists(SettingsSearchCriteria criteria, RequestContext requestContext);
        
    }
}
