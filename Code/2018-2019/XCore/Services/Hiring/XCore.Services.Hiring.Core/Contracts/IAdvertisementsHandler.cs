using System.Threading.Tasks;
using XCore.Framework.Framework.Unity.Contracts;
using XCore.Framework.Infrastructure.Context.Execution.Support;
using XCore.Framework.Infrastructure.Entities.Repositories.Models;
using XCore.Services.Hiring.Core.Models.Domain;
using XCore.Services.Hiring.Core.Models.Search;

namespace XCore.Services.Hiring.Core.Contracts
{
    public interface IAdvertisementsHandler : IUnityService
    {
        Task<ExecutionResponse<SearchResults<Advertisement>>> Get (AdvertisementsSearchCriteria criteria, RequestContext requestContext);
        Task<ExecutionResponse<Advertisement>> Create(Advertisement Attachment, RequestContext requestContext);
        Task<ExecutionResponse<Advertisement>> Edit(Advertisement advertisement, RequestContext requestContext);
        Task<ExecutionResponse<bool>> Delete(Advertisement advertisement, RequestContext requestContext);
        Task<ExecutionResponse<bool>> Delete(int id, RequestContext requestContext);
        Task<ExecutionResponse<bool>> Delete(string code, RequestContext requestContext);
        Task<ExecutionResponse<bool>> Activate(int id, RequestContext requestContext);
        Task<ExecutionResponse<bool>> Activate(string code, RequestContext requestContext);
        Task<ExecutionResponse<bool>> Deactivate(int id, RequestContext requestContext);
        Task<ExecutionResponse<bool>> Deactivate(string code, RequestContext requestContext);
        Task<ExecutionResponse<bool>> IsExists(AdvertisementsSearchCriteria criteria, RequestContext requestContext);
        Task<ExecutionResponse<bool>> IsUnique(Advertisement advertisement, RequestContext requestContext);
    }
}
