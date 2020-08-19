using System.Threading.Tasks;
using XCore.Framework.Framework.Unity.Contracts;
using XCore.Framework.Infrastructure.Context.Execution.Support;
using XCore.Framework.Infrastructure.Entities.Repositories.Models;
using XCore.Services.Hiring.Core.Models.Domain;
using XCore.Services.Hiring.Core.Models.Search;

namespace XCore.Services.Hiring.Core.Contracts
{
    public interface ISkillsHandler : IUnityService
    {
        Task<ExecutionResponse<SearchResults<Skill>>> Get (SkillsSearchCriteria criteria, RequestContext requestContext);
        Task<ExecutionResponse<Skill>> Create (Skill request, RequestContext requestContext);        
        Task<ExecutionResponse<Skill>> Edit(Skill skill, RequestContext requestContext);
        Task<ExecutionResponse<bool>> Delete(Skill skill, RequestContext requestContext);
        Task<ExecutionResponse<bool>> Delete(int id, RequestContext requestContext);
        Task<ExecutionResponse<bool>> Delete(string code, RequestContext requestContext);
        Task<ExecutionResponse<bool>> Activate(int id, RequestContext requestContext);
        Task<ExecutionResponse<bool>> Deactivate(int id, RequestContext requestContext);
        Task<ExecutionResponse<bool>> Activate(string code, RequestContext requestContext);
        Task<ExecutionResponse<bool>> Deactivate(string code, RequestContext requestContext);
        Task<ExecutionResponse<bool>> IsExists(SkillsSearchCriteria criteria, RequestContext requestContext);
        Task<ExecutionResponse<bool>> IsUnique(Skill skill, RequestContext requestContext);
    }
}
