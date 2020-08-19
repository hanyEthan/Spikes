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
    public interface IClaimHandler : IUnityService
    {
        #region Claim
        Task<ExecutionResponse<SearchResults<Claim>>> Get(ClaimSearchCriteria criteria, RequestContext requestContext);
        Task<ExecutionResponse<Claim>> Create(Claim Claim, RequestContext requestContext);
        Task<ExecutionResponse<Claim>> Edit(Claim Claim, RequestContext requestContext);
        Task<ExecutionResponse<bool>> DeleteClaim(int id, RequestContext requestContext);
        Task<ExecutionResponse<bool>> DeleteClaim(string code, RequestContext requestContext);
        Task<ExecutionResponse<bool>> IsUnique(Claim Claim, RequestContext requestContext);
        Task<ExecutionResponse<bool>> IsExists(ClaimSearchCriteria criteria, RequestContext requestContext);
        #endregion
    }
}
