using System.Threading.Tasks;
using XCore.Framework.Framework.Unity.Contracts;
using XCore.Framework.Infrastructure.Context.Execution.Support;
using XCore.Framework.Infrastructure.Entities.Repositories.Models;
using XCore.Services.Personnel.Models.Enums;
using XCore.Services.Personnel.Models.Settings;

namespace XCore.Services.Personnel.Core.Contracts.Settings
{
    public interface ISettingHandler : IUnityService
    {
        Task<ExecutionResponse<SearchResults<Setting>>> Get(SettingSearchCriteria criteria, RequestContext requestContext);
        Task<ExecutionResponse<Setting>> Create(Setting Setting, RequestContext requestContext);
        Task<ExecutionResponse<Setting>> Edit(Setting Setting, RequestContext requestContext);
        Task<ExecutionResponse<bool>> Delete(Setting Setting, RequestContext requestContext);
        Task<ExecutionResponse<bool>> Delete(int id, RequestContext requestContext);
        Task<ExecutionResponse<bool>> IsUnique(Setting Setting, RequestContext requestContext);
        Task<ExecutionResponse<bool>> IsExists(SettingSearchCriteria criteria, RequestContext requestContext);
    }
}
