using System;
using System.Threading.Tasks;
using XCore.Framework.Framework.Services.Rest;
using XCore.Framework.Infrastructure.Config.Contracts;
using XCore.Framework.Infrastructure.Context.Services.Models;
using XCore.Services.Configurations.SDK.Contracts;
using XCore.Services.Configurations.SDK.Models;
using XCore.Services.Configurations.SDK.Models.DTOs;
using XCore.Services.Configurations.SDK.Models.Support;

namespace XCore.Services.Configurations.SDK.Client
{
    public class ConfigClient : IConfigClient
    {
        #region props.

        public bool Initialized { get; protected set; }

        protected virtual IRestHandler<ConfigClientData> RestHandler { get; set; }

        #endregion
        #region cst.

        public ConfigClient(IRestHandler<ConfigClientData> restHandler)
        {
            this.RestHandler = restHandler;
            this.Initialized = Initialize();
        }

        #endregion
        #region IConfigClient

        #region Apps

        public async Task<RestResponse<ServiceExecutionResponseDTO<SearchResultsDTO<AppDTO>>>> Get(ServiceExecutionRequestDTO<AppSearchCriteriaDTO> request)
        {
            return  await this.RestHandler.CallAsync<ServiceExecutionRequestDTO<AppSearchCriteriaDTO>, ServiceExecutionResponseDTO<SearchResultsDTO<AppDTO>>>(HttpMethod.POST, request, "/api/v0.1/Config/Get");
        }
        public async Task<RestResponse<ServiceExecutionResponseDTO<AppDTO>>> Create(ServiceExecutionRequestDTO<AppDTO> request)
        {
            return await this.RestHandler.CallAsync<ServiceExecutionRequestDTO<AppDTO>, ServiceExecutionResponseDTO<AppDTO>>(HttpMethod.POST, request, "/api/v0.1/Config/Create");
        }
        public async Task<RestResponse<ServiceExecutionResponseDTO<AppDTO>>> Edit(ServiceExecutionRequestDTO<AppDTO> request)
        {
            return await this.RestHandler.CallAsync<ServiceExecutionRequestDTO<AppDTO>, ServiceExecutionResponseDTO<AppDTO>>(HttpMethod.POST, request, "/api/v0.1/Config/Edit");
        }
        public async Task<RestResponse<ServiceExecutionResponseDTO<bool>>> DeleteApp(ServiceExecutionRequestDTO<string> request)
        {
            return await this.RestHandler.CallAsync<ServiceExecutionRequestDTO<string>, ServiceExecutionResponseDTO<bool>>(HttpMethod.POST, request, "/api/v0.1/Config/Delete");
        }
        public async Task<RestResponse<ServiceExecutionResponseDTO<bool>>> ActivateApp(ServiceExecutionRequestDTO<string> request)
        {
            return await this.RestHandler.CallAsync<ServiceExecutionRequestDTO<string>, ServiceExecutionResponseDTO<bool>>(HttpMethod.POST, request, "/api/v0.1/Config/Activate");
        }
        public async Task<RestResponse<ServiceExecutionResponseDTO<bool>>> DeactivateApp(ServiceExecutionRequestDTO<string> request)
        {
            return await this.RestHandler.CallAsync<ServiceExecutionRequestDTO<string>, ServiceExecutionResponseDTO<bool>>(HttpMethod.POST, request, "/api/v0.1/Config/Deactivate");
        }

        #endregion
        #region Modules

        public async Task<RestResponse<ServiceExecutionResponseDTO<SearchResultsDTO<ModuleDTO>>>> Get(ServiceExecutionRequestDTO<ModuleSearchCriteriaDTO> request)
        {
            return await this.RestHandler.CallAsync<ServiceExecutionRequestDTO<ModuleSearchCriteriaDTO>, ServiceExecutionResponseDTO<SearchResultsDTO<ModuleDTO>>>(HttpMethod.POST, request, "/api/v0.1/Module/Get");
        }
        public async Task<RestResponse<ServiceExecutionResponseDTO<ModuleDTO>>> Create(ServiceExecutionRequestDTO<ModuleDTO> request)
        {
            return await this.RestHandler.CallAsync<ServiceExecutionRequestDTO<ModuleDTO>, ServiceExecutionResponseDTO<ModuleDTO>>(HttpMethod.POST, request, "/api/v0.1/Module/Create");
        }
        public async Task<RestResponse<ServiceExecutionResponseDTO<ModuleDTO>>> Edit(ServiceExecutionRequestDTO<ModuleDTO> request)
        {
            return await this.RestHandler.CallAsync<ServiceExecutionRequestDTO<ModuleDTO>, ServiceExecutionResponseDTO<ModuleDTO>>(HttpMethod.POST, request, "/api/v0.1/Module/Edit");
        }
        public async Task<RestResponse<ServiceExecutionResponseDTO<bool>>> DeleteModule(ServiceExecutionRequestDTO<string> request)
        {
            return await this.RestHandler.CallAsync<ServiceExecutionRequestDTO<string>, ServiceExecutionResponseDTO<bool>>(HttpMethod.POST, request, "/api/v0.1/Module/Delete");
        }
        public async Task<RestResponse<ServiceExecutionResponseDTO<bool>>> ActivateModule(ServiceExecutionRequestDTO<string> request)
        {
            return await this.RestHandler.CallAsync<ServiceExecutionRequestDTO<string>, ServiceExecutionResponseDTO<bool>>(HttpMethod.POST, request, "/api/v0.1/Module/Activate");
        }
        public async Task<RestResponse<ServiceExecutionResponseDTO<bool>>> DeactivateModule(ServiceExecutionRequestDTO<string> request)
        {
            return await this.RestHandler.CallAsync<ServiceExecutionRequestDTO<string>, ServiceExecutionResponseDTO<bool>>(HttpMethod.POST, request, "/api/v0.1/Module/Deactivate");
        }

        #endregion
        #region Configs

        public async Task<RestResponse<ServiceExecutionResponseDTO<ConfigItemDTO>>> Get(ServiceExecutionRequestDTO<ConfigKeyDTO> request)
        {
            return await this.RestHandler.CallAsync<ServiceExecutionRequestDTO<ConfigKeyDTO>, ServiceExecutionResponseDTO<ConfigItemDTO>>(HttpMethod.POST, request, "Api/v0.1/Config/Get");
        }
        public async Task<RestResponse<ServiceExecutionResponseDTO<ConfigItemDTO>>> Create(ServiceExecutionRequestDTO<ConfigItemDTO> request)
        {
            return await this.RestHandler.CallAsync<ServiceExecutionRequestDTO<ConfigItemDTO>, ServiceExecutionResponseDTO<ConfigItemDTO>>(HttpMethod.POST, request, "/api/v0.1/Config/Create");
        }
        public async Task<RestResponse<ServiceExecutionResponseDTO<ConfigItemDTO>>> Edit(ServiceExecutionRequestDTO<ConfigItemDTO> request)
        {
            return await this.RestHandler.CallAsync<ServiceExecutionRequestDTO<ConfigItemDTO>, ServiceExecutionResponseDTO<ConfigItemDTO>>(HttpMethod.POST, request, "/api/v0.1/Config/Edit");
        }
        public async Task<RestResponse<ServiceExecutionResponseDTO<bool>>> Set(ServiceExecutionRequestDTO<ConfigSetRequest> request)
        {
            return await this.RestHandler.CallAsync<ServiceExecutionRequestDTO<ConfigSetRequest>, ServiceExecutionResponseDTO<bool>>(HttpMethod.POST, request, "/api/v0.1/Config/Set");
        }
        public async Task<RestResponse<ServiceExecutionResponseDTO<bool>>> DeleteConfig(ServiceExecutionRequestDTO<string> request)
        {
            return await this.RestHandler.CallAsync<ServiceExecutionRequestDTO<string>, ServiceExecutionResponseDTO<bool>>(HttpMethod.POST, request, "/api/v0.1/Config/Delete");
        }
        public async Task<RestResponse<ServiceExecutionResponseDTO<bool>>> ActivateConfig(ServiceExecutionRequestDTO<string> request)
        {
            return await this.RestHandler.CallAsync<ServiceExecutionRequestDTO<string>, ServiceExecutionResponseDTO<bool>>(HttpMethod.POST, request, "/api/v0.1/Config/Activate");
        }
        public async Task<RestResponse<ServiceExecutionResponseDTO<bool>>> DeactivateConfig(ServiceExecutionRequestDTO<string> request)
        {
            return await this.RestHandler.CallAsync<ServiceExecutionRequestDTO<string>, ServiceExecutionResponseDTO<bool>>(HttpMethod.POST, request, "/api/v0.1/Config/Deactivate");
        }

        #endregion

        #endregion

        #region helpers.

        private bool Initialize()
        {
            bool isValid = true;

            isValid = isValid && this.RestHandler != null;
            isValid = isValid && this.RestHandler.Initialized;

            return isValid;
        }

        #endregion
    }
}
