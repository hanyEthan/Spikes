using System;
using System.Threading.Tasks;
using XCore.Framework.Framework.Services.Rest;
using XCore.Framework.Infrastructure.Config.Contracts;
using XCore.Framework.Infrastructure.Context.Services.Models;
using XCore.Services.Personnel.Models.DTO.Settings;
using XCore.Services.Personnel.SDK.Models;
using XCore.Services.Personnel.SDK.Contracts;
using XCore.Services.Personnel.Models.DTO.Support;
using XCore.Services.Personnel.Models.DTO.Essential.Settings;

namespace XCore.Services.Personnel.SDK.Client
{
    public class SettingClient : ISettingClient
    {
        #region props.
        public bool Initialized { get; private set; }
        protected virtual IRestHandler<PersonnelClientConfig> RestHandler { get; set; }
        protected virtual IConfigProvider<PersonnelClientConfig> ConfigProvider { get; set; }

        #endregion
        #region cst.

        public SettingClient(IRestHandler<PersonnelClientConfig> restHandler) : this(restHandler,null)
        {
        }
       
        public SettingClient(IRestHandler<PersonnelClientConfig> restHandler, IConfigProvider<PersonnelClientConfig> configProvider)
        {
            this.ConfigProvider = configProvider;
            this.RestHandler = restHandler;
            this.Initialized = Initialize();
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
