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
    public class PersonnelAccountClient : IPersonnelAccountClient
    {
        #region props.
        public bool Initialized { get; private set; }
        protected virtual IRestHandler<PersonnelClientConfig> RestHandler { get; set; }
        protected virtual IConfigProvider<PersonnelClientConfig> ConfigProvider { get; set; }

        #endregion
        #region cst.

        public PersonnelAccountClient(IRestHandler<PersonnelClientConfig> restHandler) : this(restHandler,null)
        {
        }
       
        public PersonnelAccountClient(IRestHandler<PersonnelClientConfig> restHandler, IConfigProvider<PersonnelClientConfig> configProvider)
        {
            this.ConfigProvider = configProvider;
            this.RestHandler = restHandler;
            this.Initialized = Initialize();
        }

        #endregion

        #region IAccountClient

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
