using System.Threading.Tasks;
using XCore.Framework.Framework.Unity.Contracts;
using XCore.Framework.Infrastructure.Context.Execution.Support;
using XCore.Services.Lookups.Core.Models.Domain;

namespace XCore.Services.Lookups.Core.Contracts
{
    public interface ILookupsHandler : IUnityService
    {
        Task<ExecutionResponse<LookupCategory>> Create(LookupCategory lookupCategory, RequestContext requestContext);
        Task<ExecutionResponse<bool>> Edit(LookupCategory lookupCategory, RequestContext requestContext);
    }
}
