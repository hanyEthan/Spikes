using System.Threading.Tasks;
using XCore.Framework.Framework.Services.Rest;
using XCore.Framework.Infrastructure.Context.Services.Models;
using XCore.Services.Hiring.SDK.Models.DTO;
using XCore.Services.Hiring.SDK.Models.Search;

namespace XCore.Services.Hiring.SDK.Contracts
{
    public interface IHiringProcessClient
    {
        bool Initialized { get; }
        Task<RestResponse<ServiceExecutionResponseDTO<HiringProcessDTO>>> Create(ServiceExecutionRequestDTO<HiringProcessDTO> request);
        Task<RestResponse<ServiceExecutionResponseDTO<SearchResultsDTO<HiringProcessDTO>>>> Get(ServiceExecutionRequestDTO<HiringProcessesSearchCriteriaDTO> request);
 

    }
}
