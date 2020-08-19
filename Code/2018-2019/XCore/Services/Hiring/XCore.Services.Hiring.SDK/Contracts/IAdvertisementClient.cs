using System.Threading.Tasks;
using XCore.Framework.Framework.Services.Rest;
using XCore.Framework.Infrastructure.Context.Services.Models;
using XCore.Services.Hiring.SDK.Models.DTO;

namespace XCore.Services.Hiring.SDK.Contracts
{
    public interface IAdvertisementClient
    {
        bool Initialized { get; }
        Task<RestResponse<ServiceExecutionResponseDTO<AdvertisementDTO>>> Create(ServiceExecutionRequestDTO<AdvertisementDTO> request);
        Task<RestResponse<ServiceExecutionResponseDTO<SearchResultsDTO<AdvertisementDTO>>>> Get(ServiceExecutionRequestDTO<AdvertisementsSearchCriteriaDTO> request);
 

    }
}
