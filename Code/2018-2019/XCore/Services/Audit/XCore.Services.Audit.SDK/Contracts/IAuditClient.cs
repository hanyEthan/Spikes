using System.Threading.Tasks;
using XCore.Framework.Framework.ServiceBus.Contracts;
using XCore.Framework.Framework.Services.Rest;
using XCore.Framework.Infrastructure.Context.Services.Models;
using XCore.Services.Audit.Models.Contracts;
using XCore.Services.Audit.SDK.Models;

namespace XCore.Services.Audit.SDK.Contracts
{
    public interface IAuditClient
    {
        bool Initialized { get; }

        Task<RestResponse<ServiceExecutionResponseDTO<bool>>> Create(ServiceExecutionRequestDTO<AuditTrailDTO> request);
        Task<RestResponse<ServiceExecutionResponseDTO<SearchResultsDTO<AuditTrailDTO>>>> Get(ServiceExecutionRequestDTO<AuditSearchCriteriaDTO> request);
        Task CreateAsync(ServiceExecutionRequestDTO<IAuditMessage> request);
    }
}
