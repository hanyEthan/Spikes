using System.Collections.Generic;
using System.Threading.Tasks;
using XCore.Framework.Framework.Services.Rest;
using XCore.Framework.Infrastructure.Context.Services.Models;
using XCore.Services.Docs.SDK.Models;

namespace XCore.Services.Docs.SDK.Contracts
{
    public interface IDocumentClient
    {
        bool Initialized { get; }

        Task<RestResponse<ServiceExecutionResponseDTO<DocumentDTO>>> Create(ServiceExecutionRequestDTO<DocumentDTO> request);
        Task<RestResponse<ServiceExecutionResponseDTO<List<DocumentDTO>>>> Create(ServiceExecutionRequestDTO<List<DocumentDTO>> request);
        Task<RestResponse<ServiceExecutionResponseDTO<SearchResultsDTO<DocumentDTO>>>> Get(ServiceExecutionRequestDTO<DocumentSearchCriteriaDTO> request);
        Task<RestResponse<ServiceExecutionResponseDTO<DocumentDTO>>> Edit(ServiceExecutionRequestDTO<DocumentDTO> request);
        Task<RestResponse<ServiceExecutionResponseDTO<bool>>> Delete(ServiceExecutionRequestDTO<int> request);
        Task<RestResponse<ServiceExecutionResponseDTO<bool>>> Delete(ServiceExecutionRequestDTO<List<int>> request);

    }
}
