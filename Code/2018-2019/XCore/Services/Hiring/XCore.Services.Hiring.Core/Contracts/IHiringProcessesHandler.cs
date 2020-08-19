using System.Threading.Tasks;
using XCore.Framework.Framework.Unity.Contracts;
using XCore.Framework.Infrastructure.Context.Execution.Support;
using XCore.Framework.Infrastructure.Entities.Repositories.Models;
using XCore.Services.Hiring.Core.Models.Domain;
using XCore.Services.Hiring.Core.Models.Search;

namespace XCore.Services.Hiring.Core.Contracts
{
    public interface IHiringProcessesHandler : IUnityService
    {
        Task<ExecutionResponse<SearchResults<HiringProcess>>> Get (HiringProcessesSearchCriteria criteria, RequestContext requestContext);
        Task<ExecutionResponse<HiringProcess>> Create (HiringProcess request, RequestContext requestContext);
        Task<ExecutionResponse<HiringProcess>> Edit(HiringProcess hiringProcess, RequestContext requestContext);
        Task<ExecutionResponse<bool>> Delete(HiringProcess hiringProcess, RequestContext requestContext);
        Task<ExecutionResponse<bool>> Delete(int id, RequestContext requestContext);
        Task<ExecutionResponse<bool>> Delete(string code, RequestContext requestContext);
        Task<ExecutionResponse<bool>> Activate(int id, RequestContext requestContext);
        Task<ExecutionResponse<bool>> Deactivate(int id, RequestContext requestContext);
        Task<ExecutionResponse<bool>> Activate(string code, RequestContext requestContext);
        Task<ExecutionResponse<bool>> Deactivate(string code, RequestContext requestContext);
        Task<ExecutionResponse<bool>> IsExists(HiringProcessesSearchCriteria criteria, RequestContext requestContext);
        Task<ExecutionResponse<bool>> IsUnique(HiringProcess hiringProcess, RequestContext requestContext);
    }
}
