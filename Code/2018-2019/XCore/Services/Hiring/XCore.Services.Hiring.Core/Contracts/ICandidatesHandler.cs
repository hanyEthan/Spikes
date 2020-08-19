using System.Threading.Tasks;
using XCore.Framework.Framework.Unity.Contracts;
using XCore.Framework.Infrastructure.Context.Execution.Support;
using XCore.Framework.Infrastructure.Entities.Repositories.Models;
using XCore.Services.Hiring.Core.Models.Domain;
using XCore.Services.Hiring.Core.Models.Search;

namespace XCore.Services.Hiring.Core.Contracts
{
    public interface ICandidatesHandler : IUnityService
    {
        Task<ExecutionResponse<SearchResults<Candidate>>> Get (CandidatesSearchCriteria criteria, RequestContext requestContext);
        Task<ExecutionResponse<Candidate>> Create (Candidate request, RequestContext requestContext);
        Task<ExecutionResponse<Candidate>> Edit(Candidate candidate, RequestContext requestContext);
        Task<ExecutionResponse<bool>> Delete(Candidate candidate, RequestContext requestContext);
        Task<ExecutionResponse<bool>> Delete(int id, RequestContext requestContext);
        Task<ExecutionResponse<bool>> Delete(string code, RequestContext requestContext);
        Task<ExecutionResponse<bool>> Activate(int id, RequestContext requestContext);
        Task<ExecutionResponse<bool>> Deactivate(int id, RequestContext requestContext);
        Task<ExecutionResponse<bool>> Activate(string code, RequestContext requestContext);
        Task<ExecutionResponse<bool>> Deactivate(string code, RequestContext requestContext);
        Task<ExecutionResponse<bool>> IsExists(CandidatesSearchCriteria criteria, RequestContext requestContext);
        Task<ExecutionResponse<bool>> IsUnique(Candidate candidate, RequestContext requestContext);
    }
}
