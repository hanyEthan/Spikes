using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using XCore.Framework.Framework.Unity.Contracts;
using XCore.Framework.Infrastructure.Context.Execution.Support;
using XCore.Framework.Infrastructure.Entities.Repositories.Models;
using XCore.Services.Security.Core.Models.Domain;
using XCore.Services.Security.Core.Models.Support;

namespace XCore.Services.Security.Core.Contracts
{
   public interface IAppHandler : IUnityService
    {
        #region App
        Task<ExecutionResponse<SearchResults<App>>> Get(AppSearchCriteria criteria, RequestContext requestContext);
        Task<ExecutionResponse<App>> Register(App app, RequestContext requestContext);
        Task<ExecutionResponse<App>> Edit(App app, RequestContext requestContext);
        Task<ExecutionResponse<bool>> UnregisterApp(int id, RequestContext requestContext);
        Task<ExecutionResponse<bool>> UnregisterApp(string code, RequestContext requestContext);
        Task<ExecutionResponse<bool>> ActivateApp(int id, RequestContext requestContext);
        Task<ExecutionResponse<bool>> ActivateApp(string code, RequestContext requestContext);
        Task<ExecutionResponse<bool>> DeactivateApp(int id, RequestContext requestContext);
        Task<ExecutionResponse<bool>> DeactivateApp(string code, RequestContext requestContext);
        Task<ExecutionResponse<bool>> IsUnique(App App, RequestContext requestContext);
        Task<ExecutionResponse<bool>> IsExists(AppSearchCriteria criteria, RequestContext requestContext);

        #endregion
    }
}
