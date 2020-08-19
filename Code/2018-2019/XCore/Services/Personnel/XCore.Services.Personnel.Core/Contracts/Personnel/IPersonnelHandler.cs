using System.Threading.Tasks;
using XCore.Framework.Framework.Unity.Contracts;
using XCore.Framework.Infrastructure.Context.Execution.Support;
using XCore.Framework.Infrastructure.Entities.Repositories.Models;
using XCore.Services.Personnel.Models.Enums;
using XCore.Services.Personnel.Models.Personnels;

namespace XCore.Services.Personnel.Core.Contracts.Personnels
{
    public interface IPersonnelHandler : IUnityService
    {
        Task<ExecutionResponse<SearchResults<Person>>> Get(PersonSearchCriteria criteria, RequestContext requestContext);
        Task<ExecutionResponse<Person>> Create(Person Personnel, RequestContext requestContext);
        Task<ExecutionResponse<Person>> Edit(Person Personnel, RequestContext requestContext);
        Task<ExecutionResponse<bool>> Delete(Person Personnel, RequestContext requestContext);
        Task<ExecutionResponse<bool>> Delete(int id, RequestContext requestContext);
        Task<ExecutionResponse<bool>> IsUnique(Person Personnel, RequestContext requestContext);
        Task<ExecutionResponse<bool>> IsExists(PersonSearchCriteria criteria, RequestContext requestContext);
        Task<ExecutionResponse<bool>> Activate(int id, RequestContext requestContext);
        Task<ExecutionResponse<bool>> Deactivate(int id, RequestContext requestContext);
    }
}
