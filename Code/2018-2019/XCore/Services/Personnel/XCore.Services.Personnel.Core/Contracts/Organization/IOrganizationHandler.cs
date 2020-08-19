using System.Threading.Tasks;
using XCore.Framework.Framework.Unity.Contracts;
using XCore.Framework.Infrastructure.Context.Execution.Support;
using XCore.Framework.Infrastructure.Entities.Repositories.Models;
using XCore.Services.Personnel.Models.Enums;
using XCore.Services.Personnel.Models.Organizations;

namespace XCore.Services.Personnel.Core.Contracts.Organizations
{
    public interface IOrganizationHandler : IUnityService
    {
        Task<ExecutionResponse<SearchResults<Organization>>> Get(OrganizationSearchCriteria criteria, RequestContext requestContext);
        Task<ExecutionResponse<Organization>> Create(Organization Organization, RequestContext requestContext);
        Task<ExecutionResponse<Organization>> Edit(Organization Organization, RequestContext requestContext);
        Task<ExecutionResponse<bool>> Delete(Organization Organization, RequestContext requestContext);
        Task<ExecutionResponse<bool>> Delete(int id, RequestContext requestContext);
        Task<ExecutionResponse<bool>> IsUnique(Organization Organization, RequestContext requestContext);
        Task<ExecutionResponse<bool>> IsExists(OrganizationSearchCriteria criteria, RequestContext requestContext);
    }
}
