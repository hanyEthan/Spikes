using System.Threading.Tasks;
using XCore.Framework.Framework.Services.Rest;
using XCore.Framework.Infrastructure.Context.Services.Models;
using XCore.Services.Personnel.Models.DTO.Essential.Organizations;
using XCore.Services.Personnel.Models.DTO.Organizations;
using XCore.Services.Personnel.Models.DTO.Support;

namespace XCore.Services.Personnel.SDK.Contracts
{
    public interface IOrganizationClient
    {
        bool Initialized { get; }
        Task<RestResponse<ServiceExecutionResponseDTO<SearchResultsDTO<OrganizationDTO>>>> Get(ServiceExecutionRequestDTO<OrganizationSearchCriteriaDTO> request);
        Task<RestResponse<ServiceExecutionResponseDTO<OrganizationDTO>>> Create(ServiceExecutionRequestDTO<OrganizationEssentialDTO> request);
        Task<RestResponse<ServiceExecutionResponseDTO<bool>>> Delete(ServiceExecutionRequestDTO<OrganizationDTO> request);
        Task<RestResponse<ServiceExecutionResponseDTO<OrganizationDTO>>> Edit(ServiceExecutionRequestDTO<OrganizationEssentialDTO> request);
    }
}
