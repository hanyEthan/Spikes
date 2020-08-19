using System.Collections.Generic;
using System.Threading.Tasks;
using XCore.Framework.Framework.Services.Rest;
using XCore.Framework.Infrastructure.Context.Services.Models;
using XCore.Services.Attachments.SDK.Models;

namespace XCore.Services.Attachments.SDK.Contracts
{
    public interface IAttachmentClient
    {
        bool Initialized { get; }
        Task<RestResponse<ServiceExecutionResponseDTO<SearchResultsDTO<AttachmentDTO>>>> Get(ServiceExecutionRequestDTO<AttachmentSearchCriteriaDTO> request);
        Task<RestResponse<ServiceExecutionResponseDTO<AttachmentDTO>>> Create(ServiceExecutionRequestDTO<AttachmentDTO> request);
        Task<RestResponse<ServiceExecutionResponseDTO<bool>>> CreateConfirm(ServiceExecutionRequestDTO<List<AttachmentDTO>> request);
        Task<RestResponse<ServiceExecutionResponseDTO<bool>>> Delete(ServiceExecutionRequestDTO<string> request);
        Task<RestResponse<ServiceExecutionResponseDTO<bool>>> DeleteListConfirm(ServiceExecutionRequestDTO<List<string>> request);
        Task<RestResponse<ServiceExecutionResponseDTO<bool>>> DeleteSoft(ServiceExecutionRequestDTO<string> request);
        Task<RestResponse<ServiceExecutionResponseDTO<bool>>> ConfirmStatus(ServiceExecutionRequestDTO<AttachmentConfirmationAction> request);


    }
}
