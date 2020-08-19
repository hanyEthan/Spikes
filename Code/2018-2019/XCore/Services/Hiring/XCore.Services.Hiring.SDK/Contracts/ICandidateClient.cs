using System.Threading.Tasks;
using XCore.Framework.Framework.Services.Rest;
using XCore.Framework.Infrastructure.Context.Services.Models;
using XCore.Services.Hiring.SDK.Models.DTO;
using XCore.Services.Hiring.SDK.Models.Search;

namespace XCore.Services.Hiring.SDK.Contracts
{
    public interface ICandidateClient
    {
        bool Initialized { get; }
        Task<RestResponse<ServiceExecutionResponseDTO<CandidateDTO>>> Create(ServiceExecutionRequestDTO<CandidateDTO> request);
        Task<RestResponse<ServiceExecutionResponseDTO<SearchResultsDTO<CandidateDTO>>>> Get(ServiceExecutionRequestDTO<CandidatesSearchCriteriaDTO> request);
 

    }
}
