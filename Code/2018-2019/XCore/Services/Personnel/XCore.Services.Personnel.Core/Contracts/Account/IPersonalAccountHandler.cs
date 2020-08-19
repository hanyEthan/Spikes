using System.Threading.Tasks;
using XCore.Framework.Framework.Unity.Contracts;
using XCore.Framework.Infrastructure.Context.Execution.Support;
using XCore.Framework.Infrastructure.Entities.Repositories.Models;
using XCore.Services.Personnel.Models.Accounts;
using XCore.Services.Personnel.Models.Enums;

namespace XCore.Services.Personnel.Core.Contracts.Accounts
{
    public interface IPersonnelAccountHandler : IUnityService
    {
        Task<ExecutionResponse<SearchResults<PersonnelAccount>>> Get(PersonnelAccountSearchCriteria criteria, RequestContext requestContext);
        Task<ExecutionResponse<PersonnelAccount>> Create(PersonnelAccount Account, RequestContext requestContext);
        Task<ExecutionResponse<PersonnelAccount>> Edit(PersonnelAccount Account, RequestContext requestContext);
        Task<ExecutionResponse<bool>> Delete(PersonnelAccount Account, RequestContext requestContext);
        Task<ExecutionResponse<bool>> Delete(int id, RequestContext requestContext);
        Task<ExecutionResponse<bool>> IsUnique(PersonnelAccount Account, RequestContext requestContext);
        Task<ExecutionResponse<bool>> IsExists(PersonnelAccountSearchCriteria criteria, RequestContext requestContext);
    }
}
