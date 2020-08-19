using System.Threading.Tasks;
using XCore.Framework.Framework.Services.Rest;
using XCore.Framework.Infrastructure.Context.Services.Models;
using XCore.Services.Organizations.SDK.Models.DTOs;
using XCore.Services.Organizations.SDK.Models.Support;

namespace XCore.Services.Organizations.SDK.Contracts
{
    public interface IOrganzationClient
    {
        bool Initialized { get; }
        #region Organization
        
        Task<RestResponse<ServiceExecutionResponseDTO<OrganizationDTO>>> AddParentToOthers(ServiceExecutionRequestDTO<OrganizationDTO> request);
        Task<RestResponse<ServiceExecutionResponseDTO<OrganizationDTO>>> Create(ServiceExecutionRequestDTO<OrganizationDTO> request);
        Task<RestResponse<ServiceExecutionResponseDTO<OrganizationDTO>>> Edit(ServiceExecutionRequestDTO<OrganizationDTO> request);
        Task<RestResponse<ServiceExecutionResponseDTO<SearchResultsDTO<OrganizationDTO>>>> Get(ServiceExecutionRequestDTO<OrganizationSearchCriteriaDTO> request);
        Task<RestResponse<ServiceExecutionResponseDTO<bool>>> DeleteOrganization(ServiceExecutionRequestDTO<string> request);
        Task<RestResponse<ServiceExecutionResponseDTO<bool>>> ActivateOrganization(ServiceExecutionRequestDTO<string> request);
        Task<RestResponse<ServiceExecutionResponseDTO<bool>>> DeactivateOrganization(ServiceExecutionRequestDTO<string> request);
        #endregion
        #region Department
        
        Task<RestResponse<ServiceExecutionResponseDTO<bool>>> RemoveDepartmentParent(ServiceExecutionRequestDTO<string> request);
        Task<RestResponse<ServiceExecutionResponseDTO<DepartmentDTO>>> CreateDepartment(ServiceExecutionRequestDTO<DepartmentDTO> request);
        Task<RestResponse<ServiceExecutionResponseDTO<DepartmentDTO>>> EditDepartment(ServiceExecutionRequestDTO<DepartmentDTO> request);
        Task<RestResponse<ServiceExecutionResponseDTO<SearchResultsDTO<DepartmentDTO>>>> GetDepartment(ServiceExecutionRequestDTO<DepartmentSearchCriteriaDTO> request);
        Task<RestResponse<ServiceExecutionResponseDTO<bool>>> ActivateDepartment(ServiceExecutionRequestDTO<string> request);
        Task<RestResponse<ServiceExecutionResponseDTO<bool>>> DeactivateDepartment(ServiceExecutionRequestDTO<string> request);
        #endregion
        #region Role
        Task<RestResponse<ServiceExecutionResponseDTO<RoleDTO>>> CreateRole(ServiceExecutionRequestDTO<RoleDTO> request);
        Task<RestResponse<ServiceExecutionResponseDTO<RoleDTO>>> EditRole(ServiceExecutionRequestDTO<RoleDTO> request);
        Task<RestResponse<ServiceExecutionResponseDTO<SearchResultsDTO<RoleDTO>>>> GetRole(ServiceExecutionRequestDTO<RoleSearchCriteriaDTO> request);
        Task<RestResponse<ServiceExecutionResponseDTO<bool>>> ActivateRole(ServiceExecutionRequestDTO<string> request);
        Task<RestResponse<ServiceExecutionResponseDTO<bool>>> DeleteRole(ServiceExecutionRequestDTO<string> request);
        Task<RestResponse<ServiceExecutionResponseDTO<bool>>> DeactivateRole(ServiceExecutionRequestDTO<string> request);
        #endregion
        #region Event
        Task<RestResponse<ServiceExecutionResponseDTO<EventDTO>>> CreateEvent(ServiceExecutionRequestDTO<EventDTO> request);
        Task<RestResponse<ServiceExecutionResponseDTO<EventDTO>>> EditEvent(ServiceExecutionRequestDTO<EventDTO> request);
        Task<RestResponse<ServiceExecutionResponseDTO<SearchResultsDTO<EventDTO>>>> GetEvent(ServiceExecutionRequestDTO<EventSearchCriteriaDTO> request);
        Task<RestResponse<ServiceExecutionResponseDTO<bool>>> ActivateEvent(ServiceExecutionRequestDTO<string> request);
        Task<RestResponse<ServiceExecutionResponseDTO<bool>>> DeleteEvent(ServiceExecutionRequestDTO<string> request);
        Task<RestResponse<ServiceExecutionResponseDTO<bool>>> DeactivateEvent(ServiceExecutionRequestDTO<string> request);
        #endregion
        #region Venue
        Task<RestResponse<ServiceExecutionResponseDTO<VenueDTO>>> CreateVenue(ServiceExecutionRequestDTO<VenueDTO> request);
        Task<RestResponse<ServiceExecutionResponseDTO<VenueDTO>>> EditVenue(ServiceExecutionRequestDTO<VenueDTO> request);
        Task<RestResponse<ServiceExecutionResponseDTO<SearchResultsDTO<VenueDTO>>>> GetVenue(ServiceExecutionRequestDTO<VenueSearchCriteriaDTO> request);
        Task<RestResponse<ServiceExecutionResponseDTO<bool>>> ActivateVenue(ServiceExecutionRequestDTO<string> request);
        Task<RestResponse<ServiceExecutionResponseDTO<bool>>> DeleteVenue(ServiceExecutionRequestDTO<string> request);
        Task<RestResponse<ServiceExecutionResponseDTO<bool>>> DeactivateVenue(ServiceExecutionRequestDTO<string> request);
        #endregion
        #region City
        Task<RestResponse<ServiceExecutionResponseDTO<CityDTO>>> CreateCity(ServiceExecutionRequestDTO<CityDTO> request);
        Task<RestResponse<ServiceExecutionResponseDTO<CityDTO>>> EditCity(ServiceExecutionRequestDTO<CityDTO> request);
        Task<RestResponse<ServiceExecutionResponseDTO<SearchResultsDTO<CityDTO>>>> GetCity(ServiceExecutionRequestDTO<CitySearchCriteriaDTO> request);
        Task<RestResponse<ServiceExecutionResponseDTO<bool>>> ActivateCity(ServiceExecutionRequestDTO<string> request);
        Task<RestResponse<ServiceExecutionResponseDTO<bool>>> DeleteCity(ServiceExecutionRequestDTO<string> request);
        Task<RestResponse<ServiceExecutionResponseDTO<bool>>> DeactivateCity(ServiceExecutionRequestDTO<string> request);
        #endregion
        #region Settings

        Task<RestResponse<ServiceExecutionResponseDTO<SettingsDTO>>> CreateSettings(ServiceExecutionRequestDTO<SettingsDTO> request);
        Task<RestResponse<ServiceExecutionResponseDTO<SettingsDTO>>> EditSettings(ServiceExecutionRequestDTO<SettingsDTO> request);
        Task<RestResponse<ServiceExecutionResponseDTO<SearchResultsDTO<SettingsDTO>>>> GetSettings(ServiceExecutionRequestDTO<SettingsSearchCriteriaDTO> request);
       // Task<RestResponse<ServiceExecutionResponseDTO<bool>>> DeleteSettings(ServiceExecutionRequestDTO<string> request);
        Task<RestResponse<ServiceExecutionResponseDTO<bool>>> ActivateSettings(ServiceExecutionRequestDTO<string> request);
        Task<RestResponse<ServiceExecutionResponseDTO<bool>>> DeactivateSettings(ServiceExecutionRequestDTO<string> request);
        #endregion
        #region OrganizationDelegation
         Task<RestResponse<ServiceExecutionResponseDTO<OrganizationDelegationDTO>>> CreateOrganizationDelegation(ServiceExecutionRequestDTO<OrganizationDelegationDTO> request);
         Task<RestResponse<ServiceExecutionResponseDTO<OrganizationDelegationDTO>>> EditOrganizationDelegation(ServiceExecutionRequestDTO<OrganizationDelegationDTO> request);
         Task<RestResponse<ServiceExecutionResponseDTO<bool>>> ActivateOrganizationDelegation(ServiceExecutionRequestDTO<string> request);
         Task<RestResponse<ServiceExecutionResponseDTO<bool>>> DeactivateOrganizationDelegation(ServiceExecutionRequestDTO<string> request);
         Task<RestResponse<ServiceExecutionResponseDTO<SearchResultsDTO<OrganizationDelegationDTO>>>> GetOrganizationDelegation(ServiceExecutionRequestDTO<OrganizationDelegationSearchCriteriaDTO> request);

        #endregion
    }
}
