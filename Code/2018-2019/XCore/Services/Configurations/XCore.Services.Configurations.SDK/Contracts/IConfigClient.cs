using System.Threading.Tasks;
using XCore.Framework.Framework.Services.Rest;
using XCore.Framework.Infrastructure.Context.Services.Models;

using XCore.Services.Configurations.SDK.Models.DTOs;
using XCore.Services.Configurations.SDK.Models.Support;

namespace XCore.Services.Configurations.SDK.Contracts
{
    public interface IConfigClient
    {
        bool Initialized { get; }

        #region Apps

        Task<RestResponse<ServiceExecutionResponseDTO<SearchResultsDTO<AppDTO>>>> Get(ServiceExecutionRequestDTO<AppSearchCriteriaDTO> request);
        Task<RestResponse<ServiceExecutionResponseDTO<AppDTO>>> Create(ServiceExecutionRequestDTO<AppDTO> request);
        Task<RestResponse<ServiceExecutionResponseDTO<AppDTO>>> Edit(ServiceExecutionRequestDTO<AppDTO> request);
        Task<RestResponse<ServiceExecutionResponseDTO<bool>>> DeleteApp(ServiceExecutionRequestDTO<string> request);
        Task<RestResponse<ServiceExecutionResponseDTO<bool>>> ActivateApp(ServiceExecutionRequestDTO<string> request);
        Task<RestResponse<ServiceExecutionResponseDTO<bool>>> DeactivateApp(ServiceExecutionRequestDTO<string> request);

        #endregion
        #region Modules

        Task<RestResponse<ServiceExecutionResponseDTO<SearchResultsDTO<ModuleDTO>>>> Get(ServiceExecutionRequestDTO<ModuleSearchCriteriaDTO> request);
        Task<RestResponse<ServiceExecutionResponseDTO<ModuleDTO>>> Create(ServiceExecutionRequestDTO<ModuleDTO> request);
        Task<RestResponse<ServiceExecutionResponseDTO<ModuleDTO>>> Edit(ServiceExecutionRequestDTO<ModuleDTO> request);
        Task<RestResponse<ServiceExecutionResponseDTO<bool>>> DeleteModule(ServiceExecutionRequestDTO<string> request);
        Task<RestResponse<ServiceExecutionResponseDTO<bool>>> ActivateModule(ServiceExecutionRequestDTO<string> request);
        Task<RestResponse<ServiceExecutionResponseDTO<bool>>> DeactivateModule(ServiceExecutionRequestDTO<string> request);

        #endregion
        #region Configs

        Task<RestResponse<ServiceExecutionResponseDTO<ConfigItemDTO>>> Get(ServiceExecutionRequestDTO<ConfigKeyDTO> request);
        Task<RestResponse<ServiceExecutionResponseDTO<ConfigItemDTO>>> Create(ServiceExecutionRequestDTO<ConfigItemDTO> request);
        Task<RestResponse<ServiceExecutionResponseDTO<ConfigItemDTO>>> Edit(ServiceExecutionRequestDTO<ConfigItemDTO> request);
        Task<RestResponse<ServiceExecutionResponseDTO<bool>>> Set(ServiceExecutionRequestDTO<ConfigSetRequest> request);
        Task<RestResponse<ServiceExecutionResponseDTO<bool>>> DeleteConfig(ServiceExecutionRequestDTO<string> request);
        Task<RestResponse<ServiceExecutionResponseDTO<bool>>> ActivateConfig(ServiceExecutionRequestDTO<string> request);
        Task<RestResponse<ServiceExecutionResponseDTO<bool>>> DeactivateConfig(ServiceExecutionRequestDTO<string> request);

        #endregion
    }
}
