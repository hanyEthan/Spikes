using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using XCore.Framework.Framework.Services.Rest;
using XCore.Framework.Infrastructure.Context.Execution.Support;
using XCore.Framework.Infrastructure.Context.Services.Models;
using XCore.Services.Notifications.SDK.Model;
using XCore.Services.Notifications.SDK.Models.DTOs;
using XCore.Services.Notifications.SDK.Models.Support;

namespace XCore.Services.Notifications.SDK.Contracts
{
   public interface IInternalNotificationClient
    {
        bool Initialized { get; }
        Task<RestResponse<ServiceExecutionResponseDTO<InternalNotificationDTO>>> Create(ServiceExecutionRequestDTO<InternalNotificationDTO> InternalNotification, RequestContext requestContext);
       Task<RestResponse<ServiceExecutionResponseDTO<SearchResultsDTO<InternalNotificationDTO>>>> Get(ServiceExecutionRequestDTO<InternalNotificationSearchCriteriaDTO> criteria, RequestContext requestContext);
        Task<RestResponse<ServiceExecutionResponseDTO<bool>>> MarkasRead(ServiceExecutionRequestDTO<List<int?>> id, RequestContext requestContext);
     //   Task<RestResponse<ExecutionResponse<bool>>> MarkasRead(List<string> code, RequestContext requestContext);
        Task<RestResponse<ServiceExecutionResponseDTO<bool>>> MarkasDismissed(ServiceExecutionRequestDTO<int> id, RequestContext requestContext);
       // Task<RestResponse<ExecutionResponse<bool>>> MarkasDismissed(string code, RequestContext requestContext);
        Task<RestResponse<ServiceExecutionResponseDTO<bool>>> MarkasDeleted(ServiceExecutionRequestDTO<int> id, RequestContext requestContext);
        //Task<RestResponse<ExecutionResponse<bool>>> MarkasDeleted(string code, RequestContext requestContext);
        Task<RestResponse<ServiceExecutionResponseDTO<bool>>> Delete(ServiceExecutionRequestDTO<int> id, RequestContext requestContext);
        //Task<RestResponse<ExecutionResponse<bool>>> Delete(string code, RequestContext requestContext);
      
    }
}
