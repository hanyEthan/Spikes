using System.Threading.Tasks;
using XCore.Framework.Framework.Unity.Contracts;
using XCore.Framework.Infrastructure.Context.Execution.Support;
using XCore.Framework.Infrastructure.Entities.Repositories.Models;
using XCore.Services.Hiring.Core.Models.Domain;
using XCore.Services.Hiring.Core.Models.Search;

namespace XCore.Services.Hiring.Core.Contracts
{
    public interface IOrganizationsHandler : IUnityService
    {
        Task<ExecutionResponse<SearchResults<Organization>>> Get (OrganizationsSearchCriteria criteria, RequestContext requestContext);
        Task<ExecutionResponse<Organization>> Create (Organization request, RequestContext requestContext);
        Task<ExecutionResponse<Organization>> Edit(Organization organization, RequestContext requestContext);
        Task<ExecutionResponse<bool>> Delete(Organization organization, RequestContext requestContext);
        Task<ExecutionResponse<bool>> Delete(int id, RequestContext requestContext);
        Task<ExecutionResponse<bool>> Delete(string code, RequestContext requestContext);
        Task<ExecutionResponse<bool>> Activate(int id, RequestContext requestContext);
        Task<ExecutionResponse<bool>> Deactivate(int id, RequestContext requestContext);
        Task<ExecutionResponse<bool>> Activate(string code, RequestContext requestContext);
        Task<ExecutionResponse<bool>> Deactivate(string code, RequestContext requestContext);
        Task<ExecutionResponse<bool>> IsExists(OrganizationsSearchCriteria criteria, RequestContext requestContext);
        Task<ExecutionResponse<bool>> IsUnique(Organization organization, RequestContext requestContext);
    }
}
