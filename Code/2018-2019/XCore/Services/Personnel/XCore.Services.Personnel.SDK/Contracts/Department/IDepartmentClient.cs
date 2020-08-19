using System.Threading.Tasks;
using XCore.Framework.Framework.Services.Rest;
using XCore.Framework.Infrastructure.Context.Services.Models;
using XCore.Services.Personnel.Models.DTO.Departments;
using XCore.Services.Personnel.Models.DTO.Essential.Departments;
using XCore.Services.Personnel.Models.DTO.Support;

namespace XCore.Services.Personnel.SDK.Contracts
{
    public interface IDepartmentClient
    {
        bool Initialized { get; }
        Task<RestResponse<ServiceExecutionResponseDTO<SearchResultsDTO<DepartmentDTO>>>> Get(ServiceExecutionRequestDTO<DepartmentSearchCriteriaDTO> request);
        Task<RestResponse<ServiceExecutionResponseDTO<DepartmentDTO>>> Create(ServiceExecutionRequestDTO<DepartmentEssentialDTO> request);
        Task<RestResponse<ServiceExecutionResponseDTO<bool>>> Delete(ServiceExecutionRequestDTO<DepartmentDTO> request);
        Task<RestResponse<ServiceExecutionResponseDTO<DepartmentDTO>>> Edit(ServiceExecutionRequestDTO<DepartmentEssentialDTO> request);
    }
}
