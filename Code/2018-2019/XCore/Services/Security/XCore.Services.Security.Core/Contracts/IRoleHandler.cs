using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using XCore.Framework.Framework.Unity.Contracts;
using XCore.Framework.Infrastructure.Context.Execution.Support;
using XCore.Framework.Infrastructure.Entities.Repositories.Models;
using XCore.Services.Security.Core.Models.Domain;
using XCore.Services.Security.Core.Models.Support;

namespace XCore.Services.Security.Core.Contracts
{
   public interface IRoleHandler : IUnityService
    {
        #region Role
        Task<ExecutionResponse<SearchResults<Role>>> Get(RoleSearchCriteria criteria, RequestContext requestContext);
        Task<ExecutionResponse<Role>> Create(Role Role, RequestContext requestContext);
        Task<ExecutionResponse<Role>> Edit(Role Role, RequestContext requestContext);
        Task<ExecutionResponse<bool>> DeleteRole(int id, RequestContext requestContext);
        Task<ExecutionResponse<bool>> ActivateRole(int id, RequestContext requestContext);
        Task<ExecutionResponse<bool>> DeactivateRole(int id, RequestContext requestContext);
        Task<ExecutionResponse<bool>> DeleteRole(string code, RequestContext requestContext);
        Task<ExecutionResponse<bool>> ActivateRole(string code, RequestContext requestContext);
        Task<ExecutionResponse<bool>> DeactivateRole(string code, RequestContext requestContext);
        Task<ExecutionResponse<bool>> IsUnique(Role Role, RequestContext requestContext);
        Task<ExecutionResponse<bool>> IsExists(RoleSearchCriteria criteria, RequestContext requestContext);
        #endregion
    }
}
