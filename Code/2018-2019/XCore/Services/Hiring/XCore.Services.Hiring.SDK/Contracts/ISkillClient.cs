using System.Threading.Tasks;
using XCore.Framework.Framework.Services.Rest;
using XCore.Framework.Infrastructure.Context.Services.Models;
using XCore.Services.Hiring.SDK.Models.DTO;
using XCore.Services.Hiring.SDK.Models.Search;

namespace XCore.Services.Hiring.SDK.Contracts
{
    public interface ISkillClient
    {
        bool Initialized { get; }
        Task<RestResponse<ServiceExecutionResponseDTO<SkillDTO>>> Create(ServiceExecutionRequestDTO<SkillDTO> request);
        Task<RestResponse<ServiceExecutionResponseDTO<SearchResultsDTO<SkillDTO>>>> Get(ServiceExecutionRequestDTO<SkillsSearchCriteriaDTO> request);
 

    }
}
