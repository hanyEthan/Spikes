using System;
using XCore.Framework.Framework.Services.Rest;
using XCore.Framework.Infrastructure.Config.Contracts;
using XCore.Framework.Infrastructure.Context.Services.Models;
using XCore.Services.Config.SDK.Contracts;
using XCore.Services.Config.SDK.Models;
using XCore.Services.Config.SDK.Models.DTOs;
using XCore.Services.Config.SDK.Models.Support;

namespace XCore.Services.Config.SDK.Client
{
    public class ConfigClient : IConfigClient
    {
        #region props.

        protected virtual IConfigProvider<ConfigClientData> ConfigProvider { get; set; }
        protected virtual IRestHandler<ConfigClientData> RestHandler { get; set; }

        #endregion
        #region cst.

        public ConfigClient(IRestHandler<ConfigClientData> restHandler) : this(restHandler, null)
        {
        }
        public ConfigClient(IRestHandler<ConfigClientData> restHandler, IConfigProvider<ConfigClientData> configProvider)
        {
            this.RestHandler = restHandler;
        }

        #endregion
        #region IConfigClient

        #region AppDTO

        public ServiceExecutionResponseDTO<SearchResultsDTO<AppDTO>> Get(ServiceExecutionRequestDTO<AppSearchCriteriaDTO> request) { throw new NotImplementedException(); }
        public ServiceExecutionResponseDTO<AppDTO> Create(ServiceExecutionRequestDTO<AppDTO> request) { throw new NotImplementedException(); }
        public ServiceExecutionResponseDTO<AppDTO> Edit(ServiceExecutionRequestDTO<AppDTO> request) { throw new NotImplementedException(); }
        public ServiceExecutionResponseDTO<bool> Delete(ServiceExecutionRequestDTO<AppDTO> request) { throw new NotImplementedException(); }
        public ServiceExecutionResponseDTO<bool> DeleteApp(ServiceExecutionRequestDTO<int> request) { throw new NotImplementedException(); }
        public ServiceExecutionResponseDTO<bool> ActivateApp(ServiceExecutionRequestDTO<int> request) { throw new NotImplementedException(); }
        public ServiceExecutionResponseDTO<bool> DeactivateApp(ServiceExecutionRequestDTO<int> request) { throw new NotImplementedException(); }

        #endregion
        #region ModuleDTOs

        public ServiceExecutionResponseDTO<SearchResultsDTO<ModuleDTO>> Get(ServiceExecutionRequestDTO<ModuleSearchCriteriaDTO> request) { throw new NotImplementedException(); }
        public ServiceExecutionResponseDTO<ModuleDTO> Create(ServiceExecutionRequestDTO<ModuleDTO> request) { throw new NotImplementedException(); }
        public ServiceExecutionResponseDTO<ModuleDTO> Edit(ServiceExecutionRequestDTO<ModuleDTO> request) { throw new NotImplementedException(); }
        public ServiceExecutionResponseDTO<bool> Delete(ServiceExecutionRequestDTO<ModuleDTO> request) { throw new NotImplementedException(); }
        public ServiceExecutionResponseDTO<bool> DeleteModuleDTO(ServiceExecutionRequestDTO<int> request) { throw new NotImplementedException(); }
        public ServiceExecutionResponseDTO<bool> ActivateModuleDTO(ServiceExecutionRequestDTO<int> request) { throw new NotImplementedException(); }
        public ServiceExecutionResponseDTO<bool> DeactivateModuleDTO(ServiceExecutionRequestDTO<int> request) { throw new NotImplementedException(); }

        #endregion
        #region Config

        public RestResponse<ServiceExecutionResponseDTO<ConfigItemDTO>> Get(ServiceExecutionRequestDTO<ConfigKeyDTO> request)
        {
            return this.RestHandler.Call<ServiceExecutionRequestDTO<ConfigKeyDTO>, ServiceExecutionResponseDTO<ConfigItemDTO>>(HttpMethod.POST, request, "/config/get/");
        }
        public RestResponse<ServiceExecutionResponseDTO<SearchResultsDTO<ConfigItemDTO>>> Get(ServiceExecutionRequestDTO<ConfigSearchCriteriaDTO> request) 
        {
            return this.RestHandler.Call<ServiceExecutionRequestDTO<ConfigSearchCriteriaDTO>, ServiceExecutionResponseDTO<SearchResultsDTO<ConfigItemDTO>>>(HttpMethod.POST, request, "/config/get/");

        }
        public ServiceExecutionResponseDTO<ConfigItemDTO> Create(ServiceExecutionRequestDTO<ConfigItemDTO> request) { throw new NotImplementedException(); }
        public ServiceExecutionResponseDTO<ConfigItemDTO> Edit(ServiceExecutionRequestDTO<ConfigItemDTO> request) { throw new NotImplementedException(); }
        public ServiceExecutionResponseDTO<bool> Set(ServiceExecutionRequestDTO<ConfigSetRequest> request) { throw new NotImplementedException(); }
        public ServiceExecutionResponseDTO<bool> Delete(ServiceExecutionRequestDTO<ConfigItemDTO> request) { throw new NotImplementedException(); }
        public ServiceExecutionResponseDTO<bool> DeleteConfig(ServiceExecutionRequestDTO<int> request) { throw new NotImplementedException(); }
        public ServiceExecutionResponseDTO<bool> ActivateConfig(ServiceExecutionRequestDTO<int> request) { throw new NotImplementedException(); }
        public ServiceExecutionResponseDTO<bool> DeactivateConfig(ServiceExecutionRequestDTO<int> request) { throw new NotImplementedException(); }

        #endregion

        #endregion
    }
}
