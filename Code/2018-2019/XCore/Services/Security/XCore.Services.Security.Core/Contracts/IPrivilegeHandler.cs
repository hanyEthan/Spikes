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
    public interface IPrivilegeHandler : IUnityService
    {
        #region Privilege
        Task<ExecutionResponse<SearchResults<Privilege>>> Get(PrivilegeSearchCriteria criteria, RequestContext requestContext);
        Task<ExecutionResponse<bool>> IsExists(PrivilegeSearchCriteria criteria, RequestContext requestContext);

        #endregion
    }
}
