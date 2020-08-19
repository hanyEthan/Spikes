using System.Threading.Tasks;
using XCore.Framework.Framework.Unity.Contracts;
using XCore.Framework.Infrastructure.Context.Execution.Support;
using XCore.Framework.Infrastructure.Entities.Repositories.Models;
using XCore.Services.Hiring.Core.Models.Domain;
using XCore.Services.Hiring.Core.Models.Search;

namespace XCore.Services.Hiring.Core.Contracts
{
    public interface IApplicationHistoryHandler : IUnityService
    {
        Task<ExecutionResponse<SearchResults<ApplicationHistory>>> Get (ApplicationHistorySearchCriteria criteria, RequestContext requestContext);
        Task<ExecutionResponse<ApplicationHistory>> Create (ApplicationHistory request, RequestContext requestContext);                
    }
}
