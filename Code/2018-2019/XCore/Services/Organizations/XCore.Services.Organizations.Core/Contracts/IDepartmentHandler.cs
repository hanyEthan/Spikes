using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using XCore.Framework.Framework.Unity.Contracts;
using XCore.Framework.Infrastructure.Context.Execution.Support;
using XCore.Framework.Infrastructure.Entities.Repositories.Models;
using XCore.Services.Organizations.Core.Models.Domain;
using XCore.Services.Organizations.Core.Models.Support;

namespace XCore.Services.Organizations.Core.Contracts
{
    public interface IDepartmentHandler : IUnityService
    {
        Task<ExecutionResponse<SearchResults<Department>>> Get(DepartmentSearchCriteria criteria, RequestContext requestContext);
        Task<ExecutionResponse<Department>> Create(Department Department, RequestContext requestContext);
        Task<ExecutionResponse<Department>> Edit(Department Department, RequestContext requestContext);
        Task<ExecutionResponse<bool>> Activate(string code, RequestContext requestContext);
        Task<ExecutionResponse<bool>> Delete(int id, RequestContext requestContext);
        Task<ExecutionResponse<bool>> Delete(Department department, RequestContext requestContext);
        Task<ExecutionResponse<bool>> Delete(string DepartmentCode, RequestContext requestContext);
        Task<ExecutionResponse<bool>> Deactivate(string code, RequestContext requestContext);
        Task<ExecutionResponse<bool>> IsUnique(Department Department, RequestContext requestContext);
        Task<ExecutionResponse<bool>> IsExists(DepartmentSearchCriteria criteria, RequestContext requestContext);
    }
}
