using System.Threading.Tasks;
using XCore.Framework.Framework.Unity.Contracts;
using XCore.Framework.Infrastructure.Context.Execution.Support;
using XCore.Framework.Infrastructure.Entities.Repositories.Models;
using XCore.Services.Personnel.Models.Accounts;
using XCore.Services.Personnel.Models.Enums;

namespace XCore.Services.Personnel.Core.Contracts.Accounts
{
    public interface IOrganizationAccountHandler : IUnityService
    {
        Task<ExecutionResponse<SearchResults<OrganizationAccount>>> Get(OrganizationAccountSearchCriteria criteria, RequestContext requestContext);
        Task<ExecutionResponse<OrganizationAccount>> Create(OrganizationAccount Account, RequestContext requestContext);
        Task<ExecutionResponse<OrganizationAccount>> Edit(OrganizationAccount Account, RequestContext requestContext);
        Task<ExecutionResponse<bool>> Delete(OrganizationAccount Account, RequestContext requestContext);
        Task<ExecutionResponse<bool>> Delete(int id, RequestContext requestContext);
        Task<ExecutionResponse<bool>> IsUnique(OrganizationAccount Account, RequestContext requestContext);
        Task<ExecutionResponse<bool>> IsExists(OrganizationAccountSearchCriteria criteria, RequestContext requestContext);
    }
}
