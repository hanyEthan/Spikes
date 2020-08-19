using System.Threading.Tasks;
using XCore.Framework.Framework.Unity.Contracts;
using XCore.Framework.Infrastructure.Context.Execution.Support;
using XCore.Framework.Infrastructure.Entities.Repositories.Models;
using XCore.Services.Organizations.Core.Models.Domain;
using XCore.Services.Organizations.Core.Models.Support;

namespace XCore.Services.Organizations.Core.Contracts
{
    public interface IRoleHandler : IUnityService
    {
        Task<ExecutionResponse<Role>> Create(Role Role, RequestContext requestContext);
        Task<ExecutionResponse<Role>> Edit(Role Role, RequestContext requestContext);
        Task<ExecutionResponse<bool>> Delete(Role Role, RequestContext requestContext);
        Task<ExecutionResponse<bool>> Delete(int id, RequestContext requestContext);
        Task<ExecutionResponse<bool>> Delete(string code, RequestContext requestContext);
        Task<ExecutionResponse<bool>> Activate(int id, RequestContext requestContext);
        Task<ExecutionResponse<bool>> Activate(string code, RequestContext requestContext);
        Task<ExecutionResponse<bool>> Deactivate(int id, RequestContext requestContext);
        Task<ExecutionResponse<bool>> Deactivate(string code, RequestContext requestContext);
        Task<ExecutionResponse<SearchResults<Role>>> Get(RoleSearchCriteria criteria, RequestContext requestContext);
        Task<ExecutionResponse<bool>> IsUnique(Role Role, RequestContext requestContext);
        Task<ExecutionResponse<bool>> IsExists(RoleSearchCriteria criteria, RequestContext requestContext);
    }
}
