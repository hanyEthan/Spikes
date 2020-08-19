using System;
using System.Threading.Tasks;
using XCore.Framework.Framework.Services.Rest;
using XCore.Framework.Infrastructure.Config.Contracts;
using XCore.Framework.Infrastructure.Context.Services.Models;
using XCore.Services.Personnel.Models.DTO.Accounts;
using XCore.Services.Personnel.SDK.Models;
using XCore.Services.Personnel.SDK.Contracts;
using XCore.Services.Personnel.Models.DTO.Support;
using XCore.Services.Personnel.Models.DTO.Essential.Accounts;

namespace XCore.Services.Personnel.SDK.Client
{
    public class OrganizationAccountClient : IOrganizationAccountClient
    {
        #region props.
        public bool Initialized { get; private set; }
        protected virtual IRestHandler<PersonnelClientConfig> RestHandler { get; set; }
        protected virtual IConfigProvider<PersonnelClientConfig> ConfigProvider { get; set; }

        #endregion
        #region cst.

        public OrganizationAccountClient(IRestHandler<PersonnelClientConfig> restHandler) : this(restHandler,null)
        {
        }
       
        public OrganizationAccountClient(IRestHandler<PersonnelClientConfig> restHandler, IConfigProvider<PersonnelClientConfig> configProvider)
        {
            this.ConfigProvider = configProvider;
            this.RestHandler = restHandler;
            this.Initialized = Initialize();
        }

        #endregion

        #region IAccountClient

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
