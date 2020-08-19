using System.Threading.Tasks;
using XCore.Framework.Framework.Unity.Contracts;
using XCore.Framework.Infrastructure.Context.Execution.Support;
using XCore.Framework.Infrastructure.Entities.Repositories.Models;
using XCore.Services.Personnel.Models.Departments;
using XCore.Services.Personnel.Models.Enums;

namespace XCore.Services.Personnel.Core.Contracts.Departments
{
    public interface IDepartmentHandler : IUnityService
    {
        Task<ExecutionResponse<SearchResults<Department>>> Get(DepartmentSearchCriteria criteria, RequestContext requestContext);
        Task<ExecutionResponse<Department>> Create(Department Department, RequestContext requestContext);
        Task<ExecutionResponse<Department>> Edit(Department Department, RequestContext requestContext);
        Task<ExecutionResponse<bool>> Delete(Department Department, RequestContext requestContext);
        Task<ExecutionResponse<bool>> Delete(int id, RequestContext requestContext);
        Task<ExecutionResponse<bool>> IsUnique(Department Department, RequestContext requestContext);
        Task<ExecutionResponse<bool>> IsExists(DepartmentSearchCriteria criteria, RequestContext requestContext);
    }
}
