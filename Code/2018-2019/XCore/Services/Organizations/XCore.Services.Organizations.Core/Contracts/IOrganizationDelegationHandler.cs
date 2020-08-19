using System.Collections.Generic;
using System.Threading.Tasks;
using XCore.Framework.Framework.Unity.Contracts;
using XCore.Framework.Infrastructure.Context.Execution.Support;
using XCore.Framework.Infrastructure.Entities.Repositories.Models;
using XCore.Services.Organizations.Core.Models.Domain;
using XCore.Services.Organizations.Core.Models.Support;

namespace XCore.Services.Organizations.Core.Contracts
{
    public interface IOrganizationDelegationHandler : IUnityService
    {
        #region OrganizationDelegation
        Task<ExecutionResponse<OrganizationDelegation>> Create(OrganizationDelegation DelegatesOrganization, RequestContext requestContext);
        Task<ExecutionResponse<OrganizationDelegation>> Edit(OrganizationDelegation DelegatesOrganization, RequestContext requestContext);
        Task<ExecutionResponse<bool>> Activate(string code, RequestContext requestContext);
        Task<ExecutionResponse<bool>> Activate(int id, RequestContext requestContext);
        Task<ExecutionResponse<bool>> Deactivate(int id, RequestContext requestContext);
        Task<ExecutionResponse<bool>> Deactivate(string code, RequestContext requestContext);
        Task<ExecutionResponse<bool>> Delete(int id, RequestContext requestContext);
        Task<ExecutionResponse<bool>> Delete(string code, RequestContext requestContext);
        Task<ExecutionResponse<bool>> Delete(OrganizationDelegation OrganizationDelegation, RequestContext requestContext);
        Task<ExecutionResponse<bool>> IsUnique(OrganizationDelegation organizationDelegation, RequestContext requestContext);
        Task<ExecutionResponse<bool>> IsExists(OrganizationDelegationSearchCriteria criteria, RequestContext requestContext);
        Task<ExecutionResponse<SearchResults<OrganizationDelegation>>> Get(OrganizationDelegationSearchCriteria criteria, RequestContext requestContext);



        #endregion
    }
}
