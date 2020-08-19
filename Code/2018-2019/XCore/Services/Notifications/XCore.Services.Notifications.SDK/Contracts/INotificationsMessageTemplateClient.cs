using System.Threading.Tasks;
using XCore.Framework.Framework.Services.Rest;
using XCore.Framework.Infrastructure.Context.Services.Models;
using XCore.Services.Notifications.SDK.Model;

namespace XCore.Services.Notifications.SDK.Contracts
{
    public interface INotificationsMessageTemplateClient
    {
        bool Initialized { get; }

        #region actions.
        Task<RestResponse<ServiceExecutionResponseDTO<MessageTemplateDTO>>> Create(ServiceExecutionRequestDTO<MessageTemplateDTO> request);
        Task<RestResponse<ServiceExecutionResponseDTO<MessageTemplateDTO>>> Edit(ServiceExecutionRequestDTO<MessageTemplateDTO> request);
        Task<RestResponse<ServiceExecutionResponseDTO<bool>>> Delete(ServiceExecutionRequestDTO<int> request);
        Task<RestResponse<ServiceExecutionResponseDTO<SearchResultsDTO<MessageTemplateDTO>>>> Get(ServiceExecutionRequestDTO<MessageTemplateSearchCriteriaDTO> request);
        Task<RestResponse<ServiceExecutionResponseDTO<bool>>> Activate(ServiceExecutionRequestDTO<int> request);
        Task<RestResponse<ServiceExecutionResponseDTO<bool>>> DeActivate(ServiceExecutionRequestDTO<int> request);
        Task<RestResponse<ServiceExecutionResponseDTO<ResolveResponseDTO>>> Resolve(ServiceExecutionRequestDTO<ResolveRequestDTO> request);

        #endregion
    }
}
