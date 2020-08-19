 using System.Threading.Tasks;
using XCore.Framework.Framework.Unity.Contracts;
using XCore.Framework.Infrastructure.Context.Execution.Support;
using XCore.Framework.Infrastructure.Entities.Repositories.Models;
using XCore.Services.Hiring.Core.Models.Domain;
using XCore.Services.Hiring.Core.Models.Search;

namespace XCore.Services.Hiring.Core.Contracts
{
    public interface IApplicationsHandler : IUnityService
    {
        Task<ExecutionResponse<SearchResults<Application>>> Get (ApplicationsSearchCriteria criteria, RequestContext requestContext);
        Task<ExecutionResponse<Application>> Create (Application request, RequestContext requestContext);
        Task<ExecutionResponse<Application>> Edit(Application application, RequestContext requestContext);
        Task<ExecutionResponse<bool>> Delete(Application application, RequestContext requestContext);
        Task<ExecutionResponse<bool>> Delete(int id, RequestContext requestContext);
        Task<ExecutionResponse<bool>> Delete(string code, RequestContext requestContext);
        Task<ExecutionResponse<bool>> Activate(int id, RequestContext requestContext);
        Task<ExecutionResponse<bool>> Deactivate(int id, RequestContext requestContext);
        Task<ExecutionResponse<bool>> Activate(string code, RequestContext requestContext);
        Task<ExecutionResponse<bool>> Deactivate(string code, RequestContext requestContext);
        Task<ExecutionResponse<bool>> IsExists(ApplicationsSearchCriteria criteria, RequestContext requestContext);
        Task<ExecutionResponse<bool>> IsUnique(Application application, RequestContext requestContext);
    }
}
