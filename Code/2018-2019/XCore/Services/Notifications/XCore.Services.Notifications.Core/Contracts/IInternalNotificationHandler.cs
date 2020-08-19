using System.Collections.Generic;
using System.Threading.Tasks;
using XCore.Framework.Framework.Unity.Contracts;
using XCore.Framework.Infrastructure.Context.Execution.Support;
using XCore.Framework.Infrastructure.Entities.Repositories.Models;
using XCore.Services.Notifications.Core.Models.Domain;
using XCore.Services.Notifications.Core.Models.Support;

namespace XCore.Services.Notifications.Core.Contracts
{
    public interface IInternalNotificationHandler : IUnityService
    {
        Task<ExecutionResponse<InternalNotification>> Create(InternalNotification InternalNotification, RequestContext requestContext);
        Task<ExecutionResponse<SearchResults<InternalNotification>>> Get(InternalNotificationSearchCriteria criteria, RequestContext requestContext);
        Task<ExecutionResponse<bool>> MarkasRead(List<int?> id, RequestContext requestContext);
        Task<ExecutionResponse<bool>> MarkasRead(List<string> code, RequestContext requestContext);
        Task<ExecutionResponse<bool>> MarkasDismissed(int id, RequestContext requestContext);
        Task<ExecutionResponse<bool>> MarkasDismissed(string code, RequestContext requestContext);
        Task<ExecutionResponse<bool>> MarkasDeleted(int id, RequestContext requestContext);
        Task<ExecutionResponse<bool>> MarkasDeleted(string code, RequestContext requestContext);
        Task<ExecutionResponse<bool>> Delete(int id, RequestContext requestContext);
        Task<ExecutionResponse<bool>> Delete(string code, RequestContext requestContext);
        Task<ExecutionResponse<bool>> IsUnique(InternalNotification MessageTemplate, RequestContext requestContext);
        Task<ExecutionResponse<bool>> IsExists(InternalNotificationSearchCriteria criteria, RequestContext requestContext);
    }
}
