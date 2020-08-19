using System.Threading.Tasks;
using XCore.Framework.Framework.Services.Rest;
using XCore.Framework.Infrastructure.Context.Services.Models;
using XCore.Services.Personnel.Models.DTO.Accounts;
using XCore.Services.Personnel.Models.DTO.Essential.Accounts;
using XCore.Services.Personnel.Models.DTO.Support;

namespace XCore.Services.Personnel.SDK.Contracts
{
    public interface IOrganizationAccountClient
    {
        bool Initialized { get; }
        Task<RestResponse<ServiceExecutionResponseDTO<SearchResultsDTO<OrganizationAccountDTO>>>> Get(ServiceExecutionRequestDTO<OrganizationAccountSearchCriteriaDTO> request);
        Task<RestResponse<ServiceExecutionResponseDTO<OrganizationAccountDTO>>> Create(ServiceExecutionRequestDTO<OrganizationAccountEssentialDTO> request);
        Task<RestResponse<ServiceExecutionResponseDTO<bool>>> Delete(ServiceExecutionRequestDTO<OrganizationAccountDTO> request);
        Task<RestResponse<ServiceExecutionResponseDTO<OrganizationAccountDTO>>> Edit(ServiceExecutionRequestDTO<OrganizationAccountEssentialDTO> request);
    }
}
