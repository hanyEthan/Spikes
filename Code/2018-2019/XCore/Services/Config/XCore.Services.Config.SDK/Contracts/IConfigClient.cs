using System;
using System.Collections.Generic;
using System.Text;
using XCore.Framework.Framework.Services.Rest;
using XCore.Framework.Infrastructure.Context.Services.Models;
using XCore.Services.Config.SDK.Models.DTOs;
using XCore.Services.Config.SDK.Models.Support;

namespace XCore.Services.Config.SDK.Contracts
{
    public interface IConfigClient
    {
        #region AppDTO

        ServiceExecutionResponseDTO<SearchResultsDTO<AppDTO>> Get(ServiceExecutionRequestDTO<AppSearchCriteriaDTO> request);
        ServiceExecutionResponseDTO<AppDTO> Create(ServiceExecutionRequestDTO<AppDTO> request);
        ServiceExecutionResponseDTO<AppDTO> Edit(ServiceExecutionRequestDTO<AppDTO> request);
        ServiceExecutionResponseDTO<bool> Delete(ServiceExecutionRequestDTO<AppDTO> request);
        ServiceExecutionResponseDTO<bool> DeleteApp(ServiceExecutionRequestDTO<int> request);
        ServiceExecutionResponseDTO<bool> ActivateApp(ServiceExecutionRequestDTO<int> request);
        ServiceExecutionResponseDTO<bool> DeactivateApp(ServiceExecutionRequestDTO<int> request);

        #endregion
        #region ModuleDTOs

        ServiceExecutionResponseDTO<SearchResultsDTO<ModuleDTO>> Get(ServiceExecutionRequestDTO<ModuleSearchCriteriaDTO> request);
        ServiceExecutionResponseDTO<ModuleDTO> Create(ServiceExecutionRequestDTO<ModuleDTO> request);
        ServiceExecutionResponseDTO<ModuleDTO> Edit(ServiceExecutionRequestDTO<ModuleDTO> request);
        ServiceExecutionResponseDTO<bool> Delete(ServiceExecutionRequestDTO<ModuleDTO> request);
        ServiceExecutionResponseDTO<bool> DeleteModuleDTO(ServiceExecutionRequestDTO<int> request);
        ServiceExecutionResponseDTO<bool> ActivateModuleDTO(ServiceExecutionRequestDTO<int> request);
        ServiceExecutionResponseDTO<bool> DeactivateModuleDTO(ServiceExecutionRequestDTO<int> request);

        #endregion
        #region Config

        RestResponse<ServiceExecutionResponseDTO<ConfigItemDTO>> Get(ServiceExecutionRequestDTO<ConfigKeyDTO> request);
        RestResponse<ServiceExecutionResponseDTO<SearchResultsDTO<ConfigItemDTO>>> Get(ServiceExecutionRequestDTO<ConfigSearchCriteriaDTO> request);
        ServiceExecutionResponseDTO<ConfigItemDTO> Create(ServiceExecutionRequestDTO<ConfigItemDTO> request);
        ServiceExecutionResponseDTO<ConfigItemDTO> Edit(ServiceExecutionRequestDTO<ConfigItemDTO> request);
        ServiceExecutionResponseDTO<bool> Set(ServiceExecutionRequestDTO<ConfigSetRequest> request);
        ServiceExecutionResponseDTO<bool> Delete(ServiceExecutionRequestDTO<ConfigItemDTO> request);
        ServiceExecutionResponseDTO<bool> DeleteConfig(ServiceExecutionRequestDTO<int> request);
        ServiceExecutionResponseDTO<bool> ActivateConfig(ServiceExecutionRequestDTO<int> request);
        ServiceExecutionResponseDTO<bool> DeactivateConfig(ServiceExecutionRequestDTO<int> request);

        #endregion
    }
}
