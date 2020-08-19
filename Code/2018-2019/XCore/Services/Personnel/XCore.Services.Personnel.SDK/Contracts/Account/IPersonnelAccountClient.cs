using System.Threading.Tasks;
using XCore.Framework.Framework.Services.Rest;
using XCore.Framework.Infrastructure.Context.Services.Models;
using XCore.Services.Personnel.Models.DTO.Accounts;
using XCore.Services.Personnel.Models.DTO.Essential.Accounts;
using XCore.Services.Personnel.Models.DTO.Support;

namespace XCore.Services.Personnel.SDK.Contracts
{
    public interface IPersonnelAccountClient
    {
        bool Initialized { get; }
        Task<RestResponse<ServiceExecutionResponseDTO<SearchResultsDTO<PersonnelAccountDTO>>>> Get(ServiceExecutionRequestDTO<PersonnelAccountSearchCriteriaDTO> request);
        Task<RestResponse<ServiceExecutionResponseDTO<PersonnelAccountDTO>>> Create(ServiceExecutionRequestDTO<PersonnelAccountEssentialDTO> request);
        Task<RestResponse<ServiceExecutionResponseDTO<bool>>> Delete(ServiceExecutionRequestDTO<PersonnelAccountDTO> request);
        Task<RestResponse<ServiceExecutionResponseDTO<PersonnelAccountDTO>>> Edit(ServiceExecutionRequestDTO<PersonnelAccountEssentialDTO> request);
    }
}
