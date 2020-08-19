using System;
using System.Threading.Tasks;
using XCore.Framework.Framework.ServiceBus.Contracts;
using XCore.Framework.Framework.Services.Rest;
using XCore.Framework.Infrastructure.Config.Contracts;
using XCore.Framework.Infrastructure.Context.Services.Models;
using XCore.Services.Organizations.SDK.Contracts;
using XCore.Services.Organizations.SDK.Models.DTOs;
using XCore.Services.Organizations.SDK.Models.Support;

namespace XCore.Services.Organizations.SDK.Client
{
    public class OrganizationClient : IOrganzationClient
    {
        #region props.

        public bool Initialized { get; private set; }

        protected virtual IRestHandler<OrganizationClientConfig> RestHandler { get; set; }
        protected virtual IConfigProvider<OrganizationClientConfig> ConfigProvider { get; set; }
        public IServiceBus ServiceBus { get; set; }

        #endregion
        #region cst.

        public OrganizationClient(IRestHandler<OrganizationClientConfig> restHandler) : this(restHandler, null, null)
        {
        }
        public OrganizationClient(IRestHandler<OrganizationClientConfig> restHandler, IConfigProvider<OrganizationClientConfig> configProvider) : this(restHandler, null, configProvider)
        {
        }

        public OrganizationClient(IRestHandler<OrganizationClientConfig> restHandler, IServiceBus serviceBus, IConfigProvider<OrganizationClientConfig> configProvider)
        {
            this.ServiceBus = serviceBus;
            this.ConfigProvider = configProvider;
            this.RestHandler = restHandler;

            this.Initialized = Initialize();
        }

        #endregion
        #region IOrganizationClient
        #region Organization

        public async Task<RestResponse<ServiceExecutionResponseDTO<OrganizationDTO>>> AddParentToOthers(ServiceExecutionRequestDTO<OrganizationDTO> request)
        {
            #region validate.

            if (!this.Initialized)
            {
                throw new Exception("Client is not properly Initialized. please check the client config configurations.");
            }

            #endregion
            return await this.RestHandler.CallAsync<ServiceExecutionRequestDTO<OrganizationDTO>, ServiceExecutionResponseDTO<OrganizationDTO>>(HttpMethod.POST, request, "/api/v0.1/Organization/AddParentToOthers");
        }

        public async Task<RestResponse<ServiceExecutionResponseDTO<OrganizationDTO>>> Create(ServiceExecutionRequestDTO<OrganizationDTO> request)
        {
            #region validate.

            if (!this.Initialized)
            {
                throw new Exception("Client is not properly Initialized. please check the client config configurations.");
            }

            #endregion
            return await this.RestHandler.CallAsync<ServiceExecutionRequestDTO<OrganizationDTO>, ServiceExecutionResponseDTO<OrganizationDTO>>(HttpMethod.POST, request, "/api/v0.1/Organization/Create");
        }

        public async Task<RestResponse<ServiceExecutionResponseDTO<OrganizationDTO>>> Edit(ServiceExecutionRequestDTO<OrganizationDTO> request)
        {
            #region validate.

            if (!this.Initialized)
            {
                throw new Exception("Client is not properly Initialized. please check the client config configurations.");
            }

            #endregion
            return await this.RestHandler.CallAsync<ServiceExecutionRequestDTO<OrganizationDTO>, ServiceExecutionResponseDTO<OrganizationDTO>>(HttpMethod.POST, request, "/api/v0.1/Organization/Edit");
        }
        public async Task<RestResponse<ServiceExecutionResponseDTO<SearchResultsDTO<OrganizationDTO>>>> Get(ServiceExecutionRequestDTO<OrganizationSearchCriteriaDTO> request)
        {
            #region validate.

            if (!this.Initialized)
            {
                throw new Exception("Client is not properly Initialized. please check the client config configurations.");
            }

            #endregion
            return await this.RestHandler.CallAsync<ServiceExecutionRequestDTO<OrganizationSearchCriteriaDTO>, ServiceExecutionResponseDTO<SearchResultsDTO<OrganizationDTO>>>(HttpMethod.POST, request, "/api/v0.1/organization/Get");
        }

        public async Task<RestResponse<ServiceExecutionResponseDTO<bool>>> DeleteOrganization(ServiceExecutionRequestDTO<string> request)
        {
            #region validate.

            if (!this.Initialized)
            {
                throw new Exception("Client is not properly Initialized. please check the client config configurations.");
            }

            #endregion
            return await this.RestHandler.CallAsync<ServiceExecutionRequestDTO<string>, ServiceExecutionResponseDTO<bool>>(HttpMethod.POST, request, "/api/v0.1/Organization/Delete");
        }
        public async Task<RestResponse<ServiceExecutionResponseDTO<bool>>> ActivateOrganization(ServiceExecutionRequestDTO<string> request)
        {
            #region validate.

            if (!this.Initialized)
            {
                throw new Exception("Client is not properly Initialized. please check the client config configurations.");
            }

            #endregion
            return await this.RestHandler.CallAsync<ServiceExecutionRequestDTO<string>, ServiceExecutionResponseDTO<bool>>(HttpMethod.POST, request, "/api/v0.1/Organization/Activate");
        }
        public async Task<RestResponse<ServiceExecutionResponseDTO<bool>>> DeactivateOrganization(ServiceExecutionRequestDTO<string> request)
        {
            #region validate.

            if (!this.Initialized)
            {
                throw new Exception("Client is not properly Initialized. please check the client config configurations.");
            }

            #endregion
            return await this.RestHandler.CallAsync<ServiceExecutionRequestDTO<string>, ServiceExecutionResponseDTO<bool>>(HttpMethod.POST, request, "/api/v0.1/Organization/Deactivate");
        }
        #endregion
        #region Department
      
        public async Task<RestResponse<ServiceExecutionResponseDTO<DepartmentDTO>>> CreateDepartment(ServiceExecutionRequestDTO<DepartmentDTO> request)
        {
            #region validate.

            if (!this.Initialized)
            {
                throw new Exception("Client is not properly Initialized. please check the client config configurations.");
            }

            #endregion
            return await this.RestHandler.CallAsync<ServiceExecutionRequestDTO<DepartmentDTO>, ServiceExecutionResponseDTO<DepartmentDTO>>(HttpMethod.POST, request, "/api/v0.1/Department/CreateDepartment");
        }
        public async Task<RestResponse<ServiceExecutionResponseDTO<DepartmentDTO>>> EditDepartment(ServiceExecutionRequestDTO<DepartmentDTO> request)
        {
            #region validate.

            if (!this.Initialized)
            {
                throw new Exception("Client is not properly Initialized. please check the client config configurations.");
            }

            #endregion
            return await this.RestHandler.CallAsync<ServiceExecutionRequestDTO<DepartmentDTO>, ServiceExecutionResponseDTO<DepartmentDTO>>(HttpMethod.POST, request, "/api/v0.1/Department/EditDepartment");
        }
        public async Task<RestResponse<ServiceExecutionResponseDTO<SearchResultsDTO<DepartmentDTO>>>> GetDepartment(ServiceExecutionRequestDTO<DepartmentSearchCriteriaDTO> request)
        {
            #region validate.

            if (!this.Initialized)
            {
                throw new Exception("Client is not properly Initialized. please check the client config configurations.");
            }

            #endregion
            return await this.RestHandler.CallAsync<ServiceExecutionRequestDTO<DepartmentSearchCriteriaDTO>, ServiceExecutionResponseDTO<SearchResultsDTO<DepartmentDTO>>>(HttpMethod.POST, request, "/api/v0.1/Department/Get");
        }
        public async Task<RestResponse<ServiceExecutionResponseDTO<bool>>> RemoveDepartmentParent(ServiceExecutionRequestDTO<string> request)
        {
            #region validate.

            if (!this.Initialized)
            {
                throw new Exception("Client is not properly Initialized. please check the client config configurations.");
            }

            #endregion
            return await this.RestHandler.CallAsync<ServiceExecutionRequestDTO<string>, ServiceExecutionResponseDTO<bool>>(HttpMethod.POST, request, "/api/v0.1/Department/RemoveDepartmentParent");
        }
        public async Task<RestResponse<ServiceExecutionResponseDTO<bool>>> ActivateDepartment(ServiceExecutionRequestDTO<string> request)
        {
            #region validate.

            if (!this.Initialized)
            {
                throw new Exception("Client is not properly Initialized. please check the client config configurations.");
            }

            #endregion
            return await this.RestHandler.CallAsync<ServiceExecutionRequestDTO<string>, ServiceExecutionResponseDTO<bool>>(HttpMethod.POST, request, "/api/v0.1/Department/Activate");
        }
        public async Task<RestResponse<ServiceExecutionResponseDTO<bool>>> DeactivateDepartment(ServiceExecutionRequestDTO<string> request)
        {
            #region validate.

            if (!this.Initialized)
            {
                throw new Exception("Client is not properly Initialized. please check the client config configurations.");
            }

            #endregion
            return await this.RestHandler.CallAsync<ServiceExecutionRequestDTO<string>, ServiceExecutionResponseDTO<bool>>(HttpMethod.POST, request, "/api/v0.1/Department/Deactivate");
        }

        #endregion
        #region Settings
        public async Task<RestResponse<ServiceExecutionResponseDTO<SettingsDTO>>> CreateSettings(ServiceExecutionRequestDTO<SettingsDTO> request)
        {
            #region validate.

            if (!this.Initialized)
            {
                throw new Exception("Client is not properly Initialized. please check the client config configurations.");
            }

            #endregion
            return await this.RestHandler.CallAsync<ServiceExecutionRequestDTO<SettingsDTO>, ServiceExecutionResponseDTO<SettingsDTO>>(HttpMethod.POST, request, "/api/v0.1/Settings/CreateSettings");
        }
        public async Task<RestResponse<ServiceExecutionResponseDTO<SettingsDTO>>> EditSettings(ServiceExecutionRequestDTO<SettingsDTO> request)
        {
            #region validate.

            if (!this.Initialized)
            {
                throw new Exception("Client is not properly Initialized. please check the client config configurations.");
            }

            #endregion
            return await this.RestHandler.CallAsync<ServiceExecutionRequestDTO<SettingsDTO>, ServiceExecutionResponseDTO<SettingsDTO>>(HttpMethod.POST, request, "/api/v0.1/Settings/EditSettings");
        }
        public async Task<RestResponse<ServiceExecutionResponseDTO<SearchResultsDTO<SettingsDTO>>>> GetSettings(ServiceExecutionRequestDTO<SettingsSearchCriteriaDTO> request)
        {
            #region validate.

            if (!this.Initialized)
            {
                throw new Exception("Client is not properly Initialized. please check the client config configurations.");
            }

            #endregion
            return await this.RestHandler.CallAsync<ServiceExecutionRequestDTO<SettingsSearchCriteriaDTO>, ServiceExecutionResponseDTO<SearchResultsDTO<SettingsDTO>>>(HttpMethod.POST, request, "/api/v0.1/Settings/GetSettings");
        }
        public async Task<RestResponse<ServiceExecutionResponseDTO<bool>>> ActivateSettings(ServiceExecutionRequestDTO<string> request)
        {
            #region validate.

            if (!this.Initialized)
            {
                throw new Exception("Client is not properly Initialized. please check the client config configurations.");
            }

            #endregion
            return await this.RestHandler.CallAsync<ServiceExecutionRequestDTO<string>, ServiceExecutionResponseDTO<bool>>(HttpMethod.POST, request, "/api/v0.1/Settings/ActivateSettings");
        }
        public async Task<RestResponse<ServiceExecutionResponseDTO<bool>>> DeactivateSettings(ServiceExecutionRequestDTO<string> request)
        {
            #region validate.

            if (!this.Initialized)
            {
                throw new Exception("Client is not properly Initialized. please check the client config configurations.");
            }

            #endregion
            return await this.RestHandler.CallAsync<ServiceExecutionRequestDTO<string>, ServiceExecutionResponseDTO<bool>>(HttpMethod.POST, request, "/api/v0.1/Settings/DeactivateSettings");
        }

        #endregion
        #region OrganizationDelegation
        public async Task<RestResponse<ServiceExecutionResponseDTO<OrganizationDelegationDTO>>> CreateOrganizationDelegation(ServiceExecutionRequestDTO<OrganizationDelegationDTO> request)
        {
            #region validate.

            if (!this.Initialized)
            {
                throw new Exception("Client is not properly Initialized. please check the client config configurations.");
            }

            #endregion
            return await this.RestHandler.CallAsync<ServiceExecutionRequestDTO<OrganizationDelegationDTO>, ServiceExecutionResponseDTO<OrganizationDelegationDTO>>(HttpMethod.POST, request, "/api/v0.1/OrganizationDelegation/CreateOrganizationDelegation");
        }
        public async Task<RestResponse<ServiceExecutionResponseDTO<OrganizationDelegationDTO>>> EditOrganizationDelegation(ServiceExecutionRequestDTO<OrganizationDelegationDTO> request)
        {
            #region validate.

            if (!this.Initialized)
            {
                throw new Exception("Client is not properly Initialized. please check the client config configurations.");
            }

            #endregion
            return await this.RestHandler.CallAsync<ServiceExecutionRequestDTO<OrganizationDelegationDTO>, ServiceExecutionResponseDTO<OrganizationDelegationDTO>>(HttpMethod.POST, request, "/api/v0.1/OrganizationDelegation/EditOrganizationDelegation");
        }
        public async Task<RestResponse<ServiceExecutionResponseDTO<SearchResultsDTO<OrganizationDelegationDTO>>>> GetOrganizationDelegation(ServiceExecutionRequestDTO<OrganizationDelegationSearchCriteriaDTO> request)
        {
            #region validate.

            if (!this.Initialized)
            {
                throw new Exception("Client is not properly Initialized. please check the client config configurations.");
            }

            #endregion
            return await this.RestHandler.CallAsync<ServiceExecutionRequestDTO<OrganizationDelegationSearchCriteriaDTO>, ServiceExecutionResponseDTO<SearchResultsDTO<OrganizationDelegationDTO>>>(HttpMethod.POST, request, "/api/v0.1/OrganizationDelegation/GetOrganizationDelegation");
        }
        public async Task<RestResponse<ServiceExecutionResponseDTO<bool>>> ActivateOrganizationDelegation(ServiceExecutionRequestDTO<string> request)
        {
            #region validate.

            if (!this.Initialized)
            {
                throw new Exception("Client is not properly Initialized. please check the client config configurations.");
            }

            #endregion
            return await this.RestHandler.CallAsync<ServiceExecutionRequestDTO<string>, ServiceExecutionResponseDTO<bool>>(HttpMethod.POST, request, "/api/v0.1/OrganizationDelegation/ActivateOrganizationDelegation");
        }
        public async Task<RestResponse<ServiceExecutionResponseDTO<bool>>> DeactivateOrganizationDelegation(ServiceExecutionRequestDTO<string> request)
        {
            #region validate.

            if (!this.Initialized)
            {
                throw new Exception("Client is not properly Initialized. please check the client config configurations.");
            }

            #endregion
            return await this.RestHandler.CallAsync<ServiceExecutionRequestDTO<string>, ServiceExecutionResponseDTO<bool>>(HttpMethod.POST, request, "/api/v0.1/OrganizationDelegation/DeactivateOrganizationDelegation");
        }

        #endregion
        #region Role
        public async Task<RestResponse<ServiceExecutionResponseDTO<RoleDTO>>> CreateRole(ServiceExecutionRequestDTO<RoleDTO> request)
        {
            #region validate.

            if (!this.Initialized)
            {
                throw new Exception("Client is not properly Initialized. please check the client config configurations.");
            }

            #endregion
            return await this.RestHandler.CallAsync<ServiceExecutionRequestDTO<RoleDTO>, ServiceExecutionResponseDTO<RoleDTO>>(HttpMethod.POST, request, "/api/v0.1/role/Create");
        }
        public async Task<RestResponse<ServiceExecutionResponseDTO<RoleDTO>>> EditRole(ServiceExecutionRequestDTO<RoleDTO> request)
        {
            #region validate.

            if (!this.Initialized)
            {
                throw new Exception("Client is not properly Initialized. please check the client config configurations.");
            }

            #endregion
            return await this.RestHandler.CallAsync<ServiceExecutionRequestDTO<RoleDTO>, ServiceExecutionResponseDTO<RoleDTO>>(HttpMethod.POST, request, "/api/v0.1/role/Edit");
        }
        public async Task<RestResponse<ServiceExecutionResponseDTO<SearchResultsDTO<RoleDTO>>>> GetRole(ServiceExecutionRequestDTO<RoleSearchCriteriaDTO> request)
        {
            #region validate.

            if (!this.Initialized)
            {
                throw new Exception("Client is not properly Initialized. please check the client config configurations.");
            }

            #endregion
            return await this.RestHandler.CallAsync<ServiceExecutionRequestDTO<RoleSearchCriteriaDTO>, ServiceExecutionResponseDTO<SearchResultsDTO<RoleDTO>>>(HttpMethod.POST, request, "/api/v0.1/role/Get");
        }
        public async Task<RestResponse<ServiceExecutionResponseDTO<bool>>> DeleteRole(ServiceExecutionRequestDTO<string> request)
        {
            #region validate.

            if (!this.Initialized)
            {
                throw new Exception("Client is not properly Initialized. please check the client config configurations.");
            }

            #endregion
            return await this.RestHandler.CallAsync<ServiceExecutionRequestDTO<string>, ServiceExecutionResponseDTO<bool>>(HttpMethod.POST, request, "/api/v0.1/Organization/Delete");
        }
        public async Task<RestResponse<ServiceExecutionResponseDTO<bool>>> ActivateRole(ServiceExecutionRequestDTO<string> request)
        {
            #region validate.

            if (!this.Initialized)
            {
                throw new Exception("Client is not properly Initialized. please check the client config configurations.");
            }

            #endregion
            return await this.RestHandler.CallAsync<ServiceExecutionRequestDTO<string>, ServiceExecutionResponseDTO<bool>>(HttpMethod.POST, request, "/api/v0.1/role/Activate");
        }

        public async Task<RestResponse<ServiceExecutionResponseDTO<bool>>> DeactivateRole(ServiceExecutionRequestDTO<string> request)
        {
            if (!this.Initialized)
            {
                throw new Exception("Client is not properly Initialized. please check the client config configurations.");
            }


            return await this.RestHandler.CallAsync<ServiceExecutionRequestDTO<string>, ServiceExecutionResponseDTO<bool>>(HttpMethod.POST, request, "/api/v0.1/role/Deactivate");
        }

        #endregion#region Role
        #region Event
        public async Task<RestResponse<ServiceExecutionResponseDTO<EventDTO>>> CreateEvent(ServiceExecutionRequestDTO<EventDTO> request)
        {
            if (!this.Initialized)
            {
                throw new Exception("Client is not properly Initialized. please check the client config configurations.");
            }


            return await this.RestHandler.CallAsync<ServiceExecutionRequestDTO<EventDTO>, ServiceExecutionResponseDTO<EventDTO>>(HttpMethod.POST, request, "/api/v0.1/event/Create");
        }

        public async Task<RestResponse<ServiceExecutionResponseDTO<EventDTO>>> EditEvent(ServiceExecutionRequestDTO<EventDTO> request)
        {
            if (!this.Initialized)
            {
                throw new Exception("Client is not properly Initialized. please check the client config configurations.");
            }


            return await this.RestHandler.CallAsync<ServiceExecutionRequestDTO<EventDTO>, ServiceExecutionResponseDTO<EventDTO>>(HttpMethod.POST, request, "/api/v0.1/event/edit");
        }

        public async Task<RestResponse<ServiceExecutionResponseDTO<SearchResultsDTO<EventDTO>>>> GetEvent(ServiceExecutionRequestDTO<EventSearchCriteriaDTO> request)
        {
            if (!this.Initialized)
            {
                throw new Exception("Client is not properly Initialized. please check the client config configurations.");
            }

            return await this.RestHandler.CallAsync<ServiceExecutionRequestDTO<EventSearchCriteriaDTO>, ServiceExecutionResponseDTO<SearchResultsDTO<EventDTO>>>(HttpMethod.POST, request, "/api/v0.1/event/Get");
        }

        public async Task<RestResponse<ServiceExecutionResponseDTO<bool>>> ActivateEvent(ServiceExecutionRequestDTO<string> request)
        {
            if (!this.Initialized)
            {
                throw new Exception("Client is not properly Initialized. please check the client config configurations.");
            }
            return await this.RestHandler.CallAsync<ServiceExecutionRequestDTO<string>, ServiceExecutionResponseDTO<bool>>(HttpMethod.POST, request, "/api/v0.1/event/Activate");

        }

        public async Task<RestResponse<ServiceExecutionResponseDTO<bool>>> DeleteEvent(ServiceExecutionRequestDTO<string> request)
        {
            if (!this.Initialized)
            {
                throw new Exception("Client is not properly Initialized. please check the client config configurations.");
            }
            return await this.RestHandler.CallAsync<ServiceExecutionRequestDTO<string>, ServiceExecutionResponseDTO<bool>>(HttpMethod.POST, request, "/api/v0.1/event/delete");
        }

        public async Task<RestResponse<ServiceExecutionResponseDTO<bool>>> DeactivateEvent(ServiceExecutionRequestDTO<string> request)
        {
            if (!this.Initialized)
            {
                throw new Exception("Client is not properly Initialized. please check the client config configurations.");
            }
            return await this.RestHandler.CallAsync<ServiceExecutionRequestDTO<string>, ServiceExecutionResponseDTO<bool>>(HttpMethod.POST, request, "/api/v0.1/event/Deactivate");

        }



        #endregion
        #region venue

        public async Task<RestResponse<ServiceExecutionResponseDTO<VenueDTO>>> CreateVenue(ServiceExecutionRequestDTO<VenueDTO> request)
        {
            #region validate.

            if (!this.Initialized)
            {
                throw new Exception("Client is not properly Initialized. please check the client config configurations.");
            }

            #endregion
            return await this.RestHandler.CallAsync<ServiceExecutionRequestDTO<VenueDTO>, ServiceExecutionResponseDTO<VenueDTO>>(HttpMethod.POST, request, "/api/v0.1/venue/Create");
        }

        public async Task<RestResponse<ServiceExecutionResponseDTO<VenueDTO>>> EditVenue(ServiceExecutionRequestDTO<VenueDTO> request)
        {
            if (!this.Initialized)
            {
                throw new Exception("Client is not properly Initialized. please check the client config configurations.");
            }


            return await this.RestHandler.CallAsync<ServiceExecutionRequestDTO<VenueDTO>, ServiceExecutionResponseDTO<VenueDTO>>(HttpMethod.POST, request, "/api/v0.1/venue/edit");
        }

        public async Task<RestResponse<ServiceExecutionResponseDTO<SearchResultsDTO<VenueDTO>>>> GetVenue(ServiceExecutionRequestDTO<VenueSearchCriteriaDTO> request)
        {
            if (!this.Initialized)
            {
                throw new Exception("Client is not properly Initialized. please check the client config configurations.");
            }

           
            return await this.RestHandler.CallAsync<ServiceExecutionRequestDTO<VenueSearchCriteriaDTO>, ServiceExecutionResponseDTO<SearchResultsDTO<VenueDTO>>>(HttpMethod.POST, request, "/api/v0.1/venue/get");
        }

        public async Task<RestResponse<ServiceExecutionResponseDTO<bool>>> ActivateVenue(ServiceExecutionRequestDTO<string> request)
        {
            if (!this.Initialized)
            {
                throw new Exception("Client is not properly Initialized. please check the client config configurations.");
            }
            return await this.RestHandler.CallAsync<ServiceExecutionRequestDTO<string>, ServiceExecutionResponseDTO<bool>>(HttpMethod.POST, request, "/api/v0.1/venue/Activate");
        }

        public async Task<RestResponse<ServiceExecutionResponseDTO<bool>>> DeleteVenue(ServiceExecutionRequestDTO<string> request)
        {

            if (!this.Initialized)
            {
                throw new Exception("Client is not properly Initialized. please check the client config configurations.");
            }
            return await this.RestHandler.CallAsync<ServiceExecutionRequestDTO<string>, ServiceExecutionResponseDTO<bool>>(HttpMethod.POST, request, "/api/v0.1/venue/Delete");
        }

        public async Task<RestResponse<ServiceExecutionResponseDTO<bool>>> DeactivateVenue(ServiceExecutionRequestDTO<string> request)
        {
            if (!this.Initialized)
            {
                throw new Exception("Client is not properly Initialized. please check the client config configurations.");
            }
            return await this.RestHandler.CallAsync<ServiceExecutionRequestDTO<string>, ServiceExecutionResponseDTO<bool>>(HttpMethod.POST, request, "/api/v0.1/venue/Deactivate");
        }
        #endregion
        #region City
        public async Task<RestResponse<ServiceExecutionResponseDTO<CityDTO>>> CreateCity(ServiceExecutionRequestDTO<CityDTO> request)
        {
            #region validate.

            if (!this.Initialized)
            {
                throw new Exception("Client is not properly Initialized. please check the client config configurations.");
            }

            #endregion
            return await this.RestHandler.CallAsync<ServiceExecutionRequestDTO<CityDTO>, ServiceExecutionResponseDTO<CityDTO>>(HttpMethod.POST, request, "/api/v0.1/City/Create");
        }

        public async Task<RestResponse<ServiceExecutionResponseDTO<CityDTO>>> EditCity(ServiceExecutionRequestDTO<CityDTO> request)
        {
            #region validate.

            if (!this.Initialized)
            {
                throw new Exception("Client is not properly Initialized. please check the client config configurations.");
            }

            #endregion
            return await this.RestHandler.CallAsync<ServiceExecutionRequestDTO<CityDTO>, ServiceExecutionResponseDTO<CityDTO>>(HttpMethod.POST, request, "/api/v0.1/City/edit");
        }

        public async  Task<RestResponse<ServiceExecutionResponseDTO<SearchResultsDTO<CityDTO>>>> GetCity(ServiceExecutionRequestDTO<CitySearchCriteriaDTO> request)
        {
            #region validate.

            if (!this.Initialized)
            {
                throw new Exception("Client is not properly Initialized. please check the client config configurations.");
            }

            #endregion
            return await this.RestHandler.CallAsync<ServiceExecutionRequestDTO<CitySearchCriteriaDTO>, ServiceExecutionResponseDTO<SearchResultsDTO<CityDTO>>>(HttpMethod.POST, request, "/api/v0.1/City/get");
        }

        public async Task<RestResponse<ServiceExecutionResponseDTO<bool>>> ActivateCity(ServiceExecutionRequestDTO<string> request)
        {
            #region validate.

            if (!this.Initialized)
            {
                throw new Exception("Client is not properly Initialized. please check the client config configurations.");
            }
            #endregion
            return await this.RestHandler.CallAsync<ServiceExecutionRequestDTO<string>, ServiceExecutionResponseDTO<bool>>(HttpMethod.POST, request, "/api/v0.1/city/Activate");
        }

        public async  Task<RestResponse<ServiceExecutionResponseDTO<bool>>> DeleteCity(ServiceExecutionRequestDTO<string> request)
        {
            #region validate.

            if (!this.Initialized)
            {
                throw new Exception("Client is not properly Initialized. please check the client config configurations.");
            }
            #endregion
            return await this.RestHandler.CallAsync<ServiceExecutionRequestDTO<string>, ServiceExecutionResponseDTO<bool>>(HttpMethod.POST, request, "/api/v0.1/city/Delete");
        }

        public  async Task<RestResponse<ServiceExecutionResponseDTO<bool>>> DeactivateCity(ServiceExecutionRequestDTO<string> request)
        {
            #region validate.

            if (!this.Initialized)
            {
                throw new Exception("Client is not properly Initialized. please check the client config configurations.");
            }
            #endregion
            return await this.RestHandler.CallAsync<ServiceExecutionRequestDTO<string>, ServiceExecutionResponseDTO<bool>>(HttpMethod.POST, request, "/api/v0.1/city/Deactivate");
        } 
        #endregion

        #region helpers.

        private bool Initialize()
        {
            bool isValid = true;

            isValid = isValid && this.ServiceBus != null;
            isValid = isValid && this.ServiceBus.Initialized;

            isValid = isValid && this.ConfigProvider != null;
            isValid = isValid && this.ConfigProvider.Initialized;

            isValid = isValid && this.RestHandler != null;
            isValid = isValid && this.RestHandler.Initialized;

            return isValid;
        }

      





        #endregion
        #endregion





    }
}
