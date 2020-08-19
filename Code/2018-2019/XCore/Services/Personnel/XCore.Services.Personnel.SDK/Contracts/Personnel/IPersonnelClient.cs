using System.Threading.Tasks;
using XCore.Framework.Framework.Services.Rest;
using XCore.Framework.Infrastructure.Context.Services.Models;
using XCore.Services.Personnel.Models.DTO.Accounts;
using XCore.Services.Personnel.Models.DTO.Departments;
using XCore.Services.Personnel.Models.DTO.Essential.Accounts;
using XCore.Services.Personnel.Models.DTO.Essential.Departments;
using XCore.Services.Personnel.Models.DTO.Essential.Organizations;
using XCore.Services.Personnel.Models.DTO.Essential.Personnels;
using XCore.Services.Personnel.Models.DTO.Essential.Settings;
using XCore.Services.Personnel.Models.DTO.Organizations;
using XCore.Services.Personnel.Models.DTO.Personnels;
using XCore.Services.Personnel.Models.DTO.Settings;
using XCore.Services.Personnel.Models.DTO.Support;

namespace XCore.Services.Personnel.SDK.Contracts
{
    public interface IPersonnelClient
    {
        bool Initialized { get; }
        #region Person
        Task<RestResponse<ServiceExecutionResponseDTO<SearchResultsDTO<PersonnelDTO>>>> Get(ServiceExecutionRequestDTO<PersonnelSearchCriteriaDTO> request);
        Task<RestResponse<ServiceExecutionResponseDTO<PersonnelDTO>>> Create(ServiceExecutionRequestDTO<PersonnelEssentialDTO> request);
        Task<RestResponse<ServiceExecutionResponseDTO<bool>>> Delete(ServiceExecutionRequestDTO<PersonnelDTO> request);
        Task<RestResponse<ServiceExecutionResponseDTO<PersonnelDTO>>> Edit(ServiceExecutionRequestDTO<PersonnelEssentialDTO> request);
        Task<RestResponse<ServiceExecutionResponseDTO<bool>>> ActivatePerson(ServiceExecutionRequestDTO<int> request);
        Task<RestResponse<ServiceExecutionResponseDTO<bool>>> DeactivatePerson(ServiceExecutionRequestDTO<int> request);
        #endregion
        #region OrganizationAccount
        Task<RestResponse<ServiceExecutionResponseDTO<SearchResultsDTO<OrganizationAccountDTO>>>> Get(ServiceExecutionRequestDTO<OrganizationAccountSearchCriteriaDTO> request);
        Task<RestResponse<ServiceExecutionResponseDTO<OrganizationAccountDTO>>> Create(ServiceExecutionRequestDTO<OrganizationAccountEssentialDTO> request);
        Task<RestResponse<ServiceExecutionResponseDTO<bool>>> Delete(ServiceExecutionRequestDTO<OrganizationAccountDTO> request);
        Task<RestResponse<ServiceExecutionResponseDTO<OrganizationAccountDTO>>> Edit(ServiceExecutionRequestDTO<OrganizationAccountEssentialDTO> request);
        #endregion
        #region PersonnelAccount
        Task<RestResponse<ServiceExecutionResponseDTO<SearchResultsDTO<PersonnelAccountDTO>>>> Get(ServiceExecutionRequestDTO<PersonnelAccountSearchCriteriaDTO> request);
        Task<RestResponse<ServiceExecutionResponseDTO<PersonnelAccountDTO>>> Create(ServiceExecutionRequestDTO<PersonnelAccountEssentialDTO> request);
        Task<RestResponse<ServiceExecutionResponseDTO<bool>>> Delete(ServiceExecutionRequestDTO<PersonnelAccountDTO> request);
        Task<RestResponse<ServiceExecutionResponseDTO<PersonnelAccountDTO>>> Edit(ServiceExecutionRequestDTO<PersonnelAccountEssentialDTO> request);
        #endregion
        #region Department
        Task<RestResponse<ServiceExecutionResponseDTO<SearchResultsDTO<DepartmentDTO>>>> Get(ServiceExecutionRequestDTO<DepartmentSearchCriteriaDTO> request);
        Task<RestResponse<ServiceExecutionResponseDTO<DepartmentDTO>>> Create(ServiceExecutionRequestDTO<DepartmentEssentialDTO> request);
        Task<RestResponse<ServiceExecutionResponseDTO<bool>>> Delete(ServiceExecutionRequestDTO<DepartmentDTO> request);
        Task<RestResponse<ServiceExecutionResponseDTO<DepartmentDTO>>> Edit(ServiceExecutionRequestDTO<DepartmentEssentialDTO> request);
        #endregion
        #region Organization
        Task<RestResponse<ServiceExecutionResponseDTO<SearchResultsDTO<OrganizationDTO>>>> Get(ServiceExecutionRequestDTO<OrganizationSearchCriteriaDTO> request);
        Task<RestResponse<ServiceExecutionResponseDTO<OrganizationDTO>>> Create(ServiceExecutionRequestDTO<OrganizationEssentialDTO> request);
        Task<RestResponse<ServiceExecutionResponseDTO<bool>>> Delete(ServiceExecutionRequestDTO<OrganizationDTO> request);
        Task<RestResponse<ServiceExecutionResponseDTO<OrganizationDTO>>> Edit(ServiceExecutionRequestDTO<OrganizationEssentialDTO> request);
        #endregion
        #region Setting
        Task<RestResponse<ServiceExecutionResponseDTO<SearchResultsDTO<SettingDTO>>>> Get(ServiceExecutionRequestDTO<SettingSearchCriteriaDTO> request);
        Task<RestResponse<ServiceExecutionResponseDTO<SettingDTO>>> Create(ServiceExecutionRequestDTO<SettingEssentialDTO> request);
        Task<RestResponse<ServiceExecutionResponseDTO<bool>>> Delete(ServiceExecutionRequestDTO<SettingDTO> request);
        Task<RestResponse<ServiceExecutionResponseDTO<SettingDTO>>> Edit(ServiceExecutionRequestDTO<SettingEssentialDTO> request);
        #endregion
    }
}
