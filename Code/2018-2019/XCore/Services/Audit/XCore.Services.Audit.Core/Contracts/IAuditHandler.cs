using System.Threading.Tasks;
using XCore.Framework.Framework.Unity.Contracts;
using XCore.Framework.Infrastructure.Context.Execution.Support;
using XCore.Framework.Infrastructure.Entities.Repositories.Models;
using XCore.Services.Audit.Core.Models;

namespace XCore.Services.Audit.Core.Contracts
{
    public interface IAuditHandler : IUnityService
    {
        Task<ExecutionResponse<SearchResults<AuditTrail>>> Get(AuditSearchCriteria criteria, RequestContext requestContext);
        Task<ExecutionResponse<AuditTrail>> Create(AuditTrail AuditTrail, RequestContext requestContext);
    }
}
