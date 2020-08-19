using System.Threading.Tasks;
using XCore.Framework.Framework.Unity.Contracts;
using XCore.Framework.Infrastructure.Context.Execution.Support;
using XCore.Framework.Infrastructure.Entities.Repositories.Models;
using XCore.Services.Notifications.Core.Models.Domain;
using XCore.Services.Notifications.Core.Models.Support;

namespace XCore.Services.Notifications.Core.Contracts
{
    public interface IMessageTemplatesHandler : IUnityService
    {
        Task<ExecutionResponse<SearchResults<MessageTemplate>>> Get(MessageTemplateSearchCriteria criteria, RequestContext requestContext);
        Task<ExecutionResponse<MessageTemplate>> Create(MessageTemplate MessageTemplate, RequestContext requestContext);
        Task<ExecutionResponse<MessageTemplate>> Edit(MessageTemplate MessageTemplate, RequestContext requestContext);
        Task<ExecutionResponse<bool>> Delete(int id, RequestContext requestContext);
        Task<ExecutionResponse<bool>> Delete(string code, RequestContext requestContext);
        Task<ExecutionResponse<bool>> Activate(int id, RequestContext requestContext);
        Task<ExecutionResponse<bool>> Activate(string code, RequestContext requestContext);
        Task<ExecutionResponse<bool>> Deactivate(int id, RequestContext requestContext);
        Task<ExecutionResponse<bool>> Deactivate(string code, RequestContext requestContext);
        Task<ExecutionResponse<bool>> IsUnique(MessageTemplate MessageTemplate, RequestContext requestContext);
        Task<ExecutionResponse<bool>> IsExists(MessageTemplateSearchCriteria criteria, RequestContext requestContext);
        Task<ExecutionResponse<ResolveResponse>> Resolve(ResolveRequest request, RequestContext requestContext);
    }
}
