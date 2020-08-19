using System;
using System.Threading.Tasks;
using XCore.Framework.Framework.Services.Rest;
using XCore.Framework.Infrastructure.Config.Contracts;
using XCore.Framework.Infrastructure.Context.Services.Models;
using XCore.Services.Personnel.Models.DTO.Personnels;
using XCore.Services.Personnel.SDK.Contracts;
using XCore.Services.Personnel.SDK.Models;
using XCore.Services.Personnel.Models.DTO.Support;
using XCore.Services.Personnel.Models.DTO.Essential.Personnels;
using XCore.Framework.Framework.ServiceBus.Contracts;
using XCore.Services.Personnel.Models.DTO.Accounts;
using XCore.Services.Personnel.Models.DTO.Essential.Accounts;
using XCore.Services.Personnel.Models.DTO.Departments;
using XCore.Services.Personnel.Models.DTO.Essential.Departments;
using XCore.Services.Personnel.Models.DTO.Organizations;
using XCore.Services.Personnel.Models.DTO.Essential.Organizations;
using XCore.Services.Personnel.Models.DTO.Settings;
using XCore.Services.Personnel.Models.DTO.Essential.Settings;

namespace XCore.Services.Personnel.SDK.Client
{
    public class PersonnelClient : IPersonnelClient
    {
        #region props.
        public bool Initialized { get; private set; }
        protected virtual IRestHandler<PersonnelClientConfig> RestHandler { get; set; }
        protected virtual IConfigProvider<PersonnelClientConfig> ConfigProvider { get; set; }

        #endregion
        #region cst.

        public PersonnelClient(IRestHandler<PersonnelClientConfig> restHandler) : this(restHandler,null)
        {
        }
       
        public PersonnelClient(IRestHandler<PersonnelClientConfig> restHandler, IConfigProvider<PersonnelClientConfig> configProvider)
        {
            this.ConfigProvider = configProvider;
            this.RestHandler = restHandler;
            this.Initialized = Initialize();
        }

        #endregion

        #region IPersonnelClient

        public async Task<RestResponse<ServiceExecutionResponseDTO<PersonnelDTO>>> Create(ServiceExecutionRequestDTO<PersonnelEssentialDTO> request)
        {
            #region validate.

            if (!this.Initialized)
            {
                throw new Exception("Client is not properly Initialized. please check the client config configurations.");
            }

            #endregion
            return await this.RestHandler.CallAsync<ServiceExecutionRequestDTO<PersonnelEssentialDTO>, ServiceExecutionResponseDTO<PersonnelDTO>>(HttpMethod.POST, request, "/api/v0.1/Personnel/create/");
        }
        public async Task<RestResponse<ServiceExecutionResponseDTO<SearchResultsDTO<PersonnelDTO>>>> Get(ServiceExecutionRequestDTO<PersonnelSearchCriteriaDTO> request)
        {
            #region validate.

            if (!this.Initialized)
            {
                throw new Exception("Client is not properly Initialized. please check the client config configurations.");
            }

            #endregion
            return await this.RestHandler.CallAsync<ServiceExecutionRequestDTO<PersonnelSearchCriteriaDTO>, ServiceExecutionResponseDTO<SearchResultsDTO<PersonnelDTO>>>(HttpMethod.POST, request, "/api/v0.1/Personnel/get/");
        }
        public async Task<RestResponse<ServiceExecutionResponseDTO<bool>>> Delete(ServiceExecutionRequestDTO<PersonnelDTO> request)
        {
            #region validate.

            if (!this.Initialized)
            {
                throw new Exception("Client is not properly Initialized. please check the client config configurations.");
            }

            #endregion
            return await this.RestHandler.CallAsync<ServiceExecutionRequestDTO<PersonnelDTO>, ServiceExecutionResponseDTO<bool>>(HttpMethod.POST, request, "/api/v0.1/Personnel/delete/");
        }
        public async Task<RestResponse<ServiceExecutionResponseDTO<PersonnelDTO>>> Edit(ServiceExecutionRequestDTO<PersonnelEssentialDTO> request)
        {
            #region validate.

            if (!this.Initialized)
            {
                throw new Exception("Client is not properly Initialized. please check the client config configurations.");
            }

            #endregion
            return await this.RestHandler.CallAsync<ServiceExecutionRequestDTO<PersonnelEssentialDTO>, ServiceExecutionResponseDTO<PersonnelDTO>>(HttpMethod.POST, request, "/api/v0.1/Personnel/Edit");
        }
        public async Task<RestResponse<ServiceExecutionResponseDTO<bool>>> ActivatePerson(ServiceExecutionRequestDTO<int> request)
        {
            #region validate.

            if (!this.Initialized)
            {
                throw new Exception("Client is not properly Initialized. please check the client config configurations.");
            }

            #endregion
            return await this.RestHandler.CallAsync<ServiceExecutionRequestDTO<int>, ServiceExecutionResponseDTO<bool>>(HttpMethod.POST, request, "/api/v0.1/Personnel/Activate");
        }
        public async Task<RestResponse<ServiceExecutionResponseDTO<bool>>> DeactivatePerson(ServiceExecutionRequestDTO<int> request)
        {
            #region validate.

            if (!this.Initialized)
            {
                throw new Exception("Client is not properly Initialized. please check the client config configurations.");
            }

            #endregion
            return await this.RestHandler.CallAsync<ServiceExecutionRequestDTO<int>, ServiceExecutionResponseDTO<bool>>(HttpMethod.POST, request, "/api/v0.1/Personnel/DeActivate");
        }

        #endregion
        #region IOrganizationAccountClient

        public async Task<RestResponse<ServiceExecutionResponseDTO<OrganizationAccountDTO>>> Create(ServiceExecutionRequestDTO<OrganizationAccountEssentialDTO> request)
        {
            #region validate.

            if (!this.Initialized)
            {
                throw new Exception("Client is not properly Initialized. please check the client config configurations.");
            }

            #endregion
            return await this.RestHandler.CallAsync<ServiceExecutionRequestDTO<OrganizationAccountEssentialDTO>, ServiceExecutionResponseDTO<OrganizationAccountDTO>>(HttpMethod.POST, request, "/api/v0.1/Personnel/OrganizationAccount/create/");
        }
        public async Task<RestResponse<ServiceExecutionResponseDTO<SearchResultsDTO<OrganizationAccountDTO>>>> Get(ServiceExecutionRequestDTO<OrganizationAccountSearchCriteriaDTO> request)
        {
            #region validate.

            if (!this.Initialized)
            {
                throw new Exception("Client is not properly Initialized. please check the client config configurations.");
            }

            #endregion
            return await this.RestHandler.CallAsync<ServiceExecutionRequestDTO<OrganizationAccountSearchCriteriaDTO>, ServiceExecutionResponseDTO<SearchResultsDTO<OrganizationAccountDTO>>>(HttpMethod.POST, request, "/api/v0.1/Personnel/OrganizationAccount/get/");
        }
        public async Task<RestResponse<ServiceExecutionResponseDTO<bool>>> Delete(ServiceExecutionRequestDTO<OrganizationAccountDTO> request)
        {
            #region validate.

            if (!this.Initialized)
            {
                throw new Exception("Client is not properly Initialized. please check the client config configurations.");
            }

            #endregion
            return await this.RestHandler.CallAsync<ServiceExecutionRequestDTO<OrganizationAccountDTO>, ServiceExecutionResponseDTO<bool>>(HttpMethod.POST, request, "/api/v0.1/Personnel/OrganizationAccount/delete/");
        }
        public async Task<RestResponse<ServiceExecutionResponseDTO<OrganizationAccountDTO>>> Edit(ServiceExecutionRequestDTO<OrganizationAccountEssentialDTO> request)
        {
            #region validate.

            if (!this.Initialized)
            {
                throw new Exception("Client is not properly Initialized. please check the client config configurations.");
            }

            #endregion
            return await this.RestHandler.CallAsync<ServiceExecutionRequestDTO<OrganizationAccountEssentialDTO>, ServiceExecutionResponseDTO<OrganizationAccountDTO>>(HttpMethod.POST, request, "/api/v0.1/Personnel/OrganizationAccount/Edit");
        }
        #endregion
        #region IPersonnelAccountClient

        public async Task<RestResponse<ServiceExecutionResponseDTO<PersonnelAccountDTO>>> Create(ServiceExecutionRequestDTO<PersonnelAccountEssentialDTO> request)
        {
            #region validate.

            if (!this.Initialized)
            {
                throw new Exception("Client is not properly Initialized. please check the client config configurations.");
            }

            #endregion
            return await this.RestHandler.CallAsync<ServiceExecutionRequestDTO<PersonnelAccountEssentialDTO>, ServiceExecutionResponseDTO<PersonnelAccountDTO>>(HttpMethod.POST, request, "/api/v0.1/Personnel/PersonnelAccount/create/");
        }
        public async Task<RestResponse<ServiceExecutionResponseDTO<SearchResultsDTO<PersonnelAccountDTO>>>> Get(ServiceExecutionRequestDTO<PersonnelAccountSearchCriteriaDTO> request)
        {
            #region validate.

            if (!this.Initialized)
            {
                throw new Exception("Client is not properly Initialized. please check the client config configurations.");
            }

            #endregion
            return await this.RestHandler.CallAsync<ServiceExecutionRequestDTO<PersonnelAccountSearchCriteriaDTO>, ServiceExecutionResponseDTO<SearchResultsDTO<PersonnelAccountDTO>>>(HttpMethod.POST, request, "/api/v0.1/Personnel/PersonnelAccount/get/");
        }
        public async Task<RestResponse<ServiceExecutionResponseDTO<bool>>> Delete(ServiceExecutionRequestDTO<PersonnelAccountDTO> request)
        {
            #region validate.

            if (!this.Initialized)
            {
                throw new Exception("Client is not properly Initialized. please check the client config configurations.");
            }

            #endregion
            return await this.RestHandler.CallAsync<ServiceExecutionRequestDTO<PersonnelAccountDTO>, ServiceExecutionResponseDTO<bool>>(HttpMethod.POST, request, "/api/v0.1/Personnel/PersonnelAccount/delete/");
        }
        public async Task<RestResponse<ServiceExecutionResponseDTO<PersonnelAccountDTO>>> Edit(ServiceExecutionRequestDTO<PersonnelAccountEssentialDTO> request)
        {
            #region validate.

            if (!this.Initialized)
            {
                throw new Exception("Client is not properly Initialized. please check the client config configurations.");
            }

            #endregion
            return await this.RestHandler.CallAsync<ServiceExecutionRequestDTO<PersonnelAccountEssentialDTO>, ServiceExecutionResponseDTO<PersonnelAccountDTO>>(HttpMethod.POST, request, "/api/v0.1/Personnel/PersonnelAccount/Edit");
        }
        #endregion
        #region IDepartmentClient

        public async Task<RestResponse<ServiceExecutionResponseDTO<DepartmentDTO>>> Create(ServiceExecutionRequestDTO<DepartmentEssentialDTO> request)
        {
            #region validate.

            if (!this.Initialized)
            {
                throw new Exception("Client is not properly Initialized. please check the client config configurations.");
            }

            #endregion
            return await this.RestHandler.CallAsync<ServiceExecutionRequestDTO<DepartmentEssentialDTO>, ServiceExecutionResponseDTO<DepartmentDTO>>(HttpMethod.POST, request, "/api/v0.1/Personnel/Department/create/");
        }
        public async Task<RestResponse<ServiceExecutionResponseDTO<SearchResultsDTO<DepartmentDTO>>>> Get(ServiceExecutionRequestDTO<DepartmentSearchCriteriaDTO> request)
        {
            #region validate.

            if (!this.Initialized)
            {
                throw new Exception("Client is not properly Initialized. please check the client config configurations.");
            }

            #endregion
            return await this.RestHandler.CallAsync<ServiceExecutionRequestDTO<DepartmentSearchCriteriaDTO>, ServiceExecutionResponseDTO<SearchResultsDTO<DepartmentDTO>>>(HttpMethod.POST, request, "/api/v0.1/Personnel/Department/get/");
        }
        public async Task<RestResponse<ServiceExecutionResponseDTO<bool>>> Delete(ServiceExecutionRequestDTO<DepartmentDTO> request)
        {
            #region validate.

            if (!this.Initialized)
            {
                throw new Exception("Client is not properly Initialized. please check the client config configurations.");
            }

            #endregion
            return await this.RestHandler.CallAsync<ServiceExecutionRequestDTO<DepartmentDTO>, ServiceExecutionResponseDTO<bool>>(HttpMethod.POST, request, "/api/v0.1/Personnel/Department/delete/");
        }
        public async Task<RestResponse<ServiceExecutionResponseDTO<DepartmentDTO>>> Edit(ServiceExecutionRequestDTO<DepartmentEssentialDTO> request)
        {
            #region validate.

            if (!this.Initialized)
            {
                throw new Exception("Client is not properly Initialized. please check the client config configurations.");
            }

            #endregion
            return await this.RestHandler.CallAsync<ServiceExecutionRequestDTO<DepartmentEssentialDTO>, ServiceExecutionResponseDTO<DepartmentDTO>>(HttpMethod.POST, request, "/api/v0.1/Personnel/Department/Edit");
        }
        #endregion
        #region IOrganizationClient

        public async Task<RestResponse<ServiceExecutionResponseDTO<OrganizationDTO>>> Create(ServiceExecutionRequestDTO<OrganizationEssentialDTO> request)
        {
            #region validate.

            if (!this.Initialized)
            {
                throw new Exception("Client is not properly Initialized. please check the client config configurations.");
            }

            #endregion
            return await this.RestHandler.CallAsync<ServiceExecutionRequestDTO<OrganizationEssentialDTO>, ServiceExecutionResponseDTO<OrganizationDTO>>(HttpMethod.POST, request, "/api/v0.1/Personnel/Organization/create/");
        }
        public async Task<RestResponse<ServiceExecutionResponseDTO<SearchResultsDTO<OrganizationDTO>>>> Get(ServiceExecutionRequestDTO<OrganizationSearchCriteriaDTO> request)
        {
            #region validate.

            if (!this.Initialized)
            {
                throw new Exception("Client is not properly Initialized. please check the client config configurations.");
            }

            #endregion
            return await this.RestHandler.CallAsync<ServiceExecutionRequestDTO<OrganizationSearchCriteriaDTO>, ServiceExecutionResponseDTO<SearchResultsDTO<OrganizationDTO>>>(HttpMethod.POST, request, "/api/v0.1/Personnel/Organization/get/");
        }
        public async Task<RestResponse<ServiceExecutionResponseDTO<bool>>> Delete(ServiceExecutionRequestDTO<OrganizationDTO> request)
        {
            #region validate.

            if (!this.Initialized)
            {
                throw new Exception("Client is not properly Initialized. please check the client config configurations.");
            }

            #endregion
            return await this.RestHandler.CallAsync<ServiceExecutionRequestDTO<OrganizationDTO>, ServiceExecutionResponseDTO<bool>>(HttpMethod.POST, request, "/api/v0.1/Personnel/Organization/delete/");
        }
        public async Task<RestResponse<ServiceExecutionResponseDTO<OrganizationDTO>>> Edit(ServiceExecutionRequestDTO<OrganizationEssentialDTO> request)
        {
            #region validate.

            if (!this.Initialized)
            {
                throw new Exception("Client is not properly Initialized. please check the client config configurations.");
            }

            #endregion
            return await this.RestHandler.CallAsync<ServiceExecutionRequestDTO<OrganizationEssentialDTO>, ServiceExecutionResponseDTO<OrganizationDTO>>(HttpMethod.POST, request, "/api/v0.1/Personnel/Organization/Edit");
        }
        #endregion
        #region ISettingClient

        public async Task<RestResponse<ServiceExecutionResponseDTO<SettingDTO>>> Create(ServiceExecutionRequestDTO<SettingEssentialDTO> request)
        {
            #region validate.

            if (!this.Initialized)
            {
                throw new Exception("Client is not properly Initialized. please check the client config configurations.");
            }

            #endregion
            return await this.RestHandler.CallAsync<ServiceExecutionRequestDTO<SettingEssentialDTO>, ServiceExecutionResponseDTO<SettingDTO>>(HttpMethod.POST, request, "/api/v0.1/Personnel/Setting/create/");
        }
        public async Task<RestResponse<ServiceExecutionResponseDTO<SearchResultsDTO<SettingDTO>>>> Get(ServiceExecutionRequestDTO<SettingSearchCriteriaDTO> request)
        {
            #region validate.

            if (!this.Initialized)
            {
                throw new Exception("Client is not properly Initialized. please check the client config configurations.");
            }

            #endregion
            return await this.RestHandler.CallAsync<ServiceExecutionRequestDTO<SettingSearchCriteriaDTO>, ServiceExecutionResponseDTO<SearchResultsDTO<SettingDTO>>>(HttpMethod.POST, request, "/api/v0.1/Personnel/Setting/get/");
        }
        public async Task<RestResponse<ServiceExecutionResponseDTO<bool>>> Delete(ServiceExecutionRequestDTO<SettingDTO> request)
        {
            #region validate.

            if (!this.Initialized)
            {
                throw new Exception("Client is not properly Initialized. please check the client config configurations.");
            }

            #endregion
            return await this.RestHandler.CallAsync<ServiceExecutionRequestDTO<SettingDTO>, ServiceExecutionResponseDTO<bool>>(HttpMethod.POST, request, "/api/v0.1/Personnel/Setting/delete/");
        }
        public async Task<RestResponse<ServiceExecutionResponseDTO<SettingDTO>>> Edit(ServiceExecutionRequestDTO<SettingEssentialDTO> request)
        {
            #region validate.

            if (!this.Initialized)
            {
                throw new Exception("Client is not properly Initialized. please check the client config configurations.");
            }

            #endregion
            return await this.RestHandler.CallAsync<ServiceExecutionRequestDTO<SettingEssentialDTO>, ServiceExecutionResponseDTO<SettingDTO>>(HttpMethod.POST, request, "/api/v0.1/Personnel/Setting/Edit");
        }
        #endregion
        #region helpers.
        private bool Initialize()
        {
            bool isValid = true;

            isValid = isValid && this.ConfigProvider != null;
            isValid = isValid && this.ConfigProvider.Initialized;

            isValid = isValid && this.RestHandler != null;
            isValid = isValid && this.RestHandler.Initialized;

            return isValid;
        }
        #endregion
    }
}
