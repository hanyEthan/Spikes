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
   public interface ITargetHandler : IUnityService
    {
        #region Target
        Task<ExecutionResponse<SearchResults<Target>>> Get(TargetSearchCriteria criteria, RequestContext requestContext);
        Task<ExecutionResponse<bool>> IsExists(TargetSearchCriteria criteria, RequestContext requestContext);
        #endregion
    }
}
