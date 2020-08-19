using System.Threading.Tasks;
using XCore.Framework.Framework.Services.Rest;
using XCore.Framework.Infrastructure.Context.Services.Models;
using XCore.Services.Personnel.Models.DTO.Essential.Settings;
using XCore.Services.Personnel.Models.DTO.Settings;
using XCore.Services.Personnel.Models.DTO.Support;

namespace XCore.Services.Personnel.SDK.Contracts
{
    public interface ISettingClient
    {
        bool Initialized { get; }
        Task<RestResponse<ServiceExecutionResponseDTO<SearchResultsDTO<SettingDTO>>>> Get(ServiceExecutionRequestDTO<SettingSearchCriteriaDTO> request);
        Task<RestResponse<ServiceExecutionResponseDTO<SettingDTO>>> Create(ServiceExecutionRequestDTO<SettingEssentialDTO> request);
        Task<RestResponse<ServiceExecutionResponseDTO<bool>>> Delete(ServiceExecutionRequestDTO<SettingDTO> request);
        Task<RestResponse<ServiceExecutionResponseDTO<SettingDTO>>> Edit(ServiceExecutionRequestDTO<SettingEssentialDTO> request);
    }
}
