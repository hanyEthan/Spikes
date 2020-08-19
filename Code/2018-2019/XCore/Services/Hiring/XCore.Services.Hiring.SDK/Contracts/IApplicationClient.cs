using System.Threading.Tasks;
using XCore.Framework.Framework.Services.Rest;
using XCore.Framework.Infrastructure.Context.Services.Models;
using XCore.Services.Hiring.SDK.Models.DTO;
using XCore.Services.Hiring.SDK.Models.Search;

namespace XCore.Services.Hiring.SDK.Contracts
{
    public interface IApplicationClient
    {
        bool Initialized { get; }
        Task<RestResponse<ServiceExecutionResponseDTO<ApplicationDTO>>> Create(ServiceExecutionRequestDTO<ApplicationDTO> request);
        Task<RestResponse<ServiceExecutionResponseDTO<SearchResultsDTO<ApplicationDTO>>>> Get(ServiceExecutionRequestDTO<ApplicationsSearchCriteriaDTO> request);
 

    }
}
