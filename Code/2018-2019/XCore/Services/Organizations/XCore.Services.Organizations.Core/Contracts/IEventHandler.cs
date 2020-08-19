using System.Threading.Tasks;
using XCore.Framework.Framework.Unity.Contracts;
using XCore.Framework.Infrastructure.Context.Execution.Support;
using XCore.Framework.Infrastructure.Entities.Repositories.Models;
using XCore.Services.Organizations.Core.Models.Domain;
using XCore.Services.Organizations.Core.Models.Support;

namespace XCore.Services.Organizations.Core.Contracts
{
    public interface IEventHandler : IUnityService
    {
        Task<ExecutionResponse<Event>> Create(Event Event, RequestContext requestContext);
        Task<ExecutionResponse<Event>> Edit(Event Event, RequestContext requestContext);
        Task<ExecutionResponse<bool>> Delete(Event Event, RequestContext requestContext);
        Task<ExecutionResponse<bool>> Delete(int id, RequestContext requestContext);
        Task<ExecutionResponse<bool>> Delete(string code, RequestContext requestContext);
        Task<ExecutionResponse<bool>> Activate(int id, RequestContext requestContext);
        Task<ExecutionResponse<bool>> Activate(string code, RequestContext requestContext);
        Task<ExecutionResponse<bool>> Deactivate(int id, RequestContext requestContext);
        Task<ExecutionResponse<bool>> Deactivate(string code, RequestContext requestContext);
        Task<ExecutionResponse<SearchResults<Event>>> Get(EventSearchCriteria criteria, RequestContext requestContext);
        Task<ExecutionResponse<bool>> IsUnique(Event Event, RequestContext requestContext);
        Task<ExecutionResponse<bool>> IsExists(EventSearchCriteria criteria, RequestContext requestContext);
    }
}
