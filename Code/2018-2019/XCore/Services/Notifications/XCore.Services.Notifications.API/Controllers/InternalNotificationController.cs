using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using XCore.Framework.Infrastructure.Context.Services.Handlers;
using XCore.Framework.Infrastructure.Context.Services.Models;
using XCore.Framework.Infrastructure.Entities.Mappers;
using XCore.Framework.Infrastructure.Entities.Repositories.Models;
using XCore.Framework.Utilities;
using XCore.Services.Notifications.API.Mappers;
using XCore.Services.Notifications.API.Model;
using XCore.Services.Notifications.Core.Contracts;
using XCore.Services.Notifications.Core.Models.Domain;
using XCore.Services.Notifications.Core.Models.Support;

namespace XCore.Services.Notifications.API.Controllers
{
    [ApiVersion("0.1")]
    [ApiController]
    [Route("Api/v{version:apiVersion}/[controller]")]
    public class InternalNotificationController : ControllerBase
    {
        #region props.
        public bool? Initialized { get; protected set; }
        private readonly IInternalNotificationHandler InternalNotification;
        #endregion
        #region cst.
        public InternalNotificationController(IInternalNotificationHandler InternalNotification)
        {
            this.InternalNotification = InternalNotification;
            this.Initialized = Initialize();
        }
        #endregion
        #region actions.
        #region InternalNotification : Get


        [HttpPost("Get")]
        public async Task<ServiceExecutionResponseDTO<SearchResultsDTO<InternalNotificationDTO>>> InternalNotificationGet(ServiceExecutionRequestDTO<InternalNotificationSearchCriteriaDTO> request)
        {
            string serviceName = "xcore.notifications.internal.notification.get";

            try
            {
                #region request.

                // ...
                ServiceExecutionRequestDTO<InternalNotificationSearchCriteriaDTO> requestDTO = request;
                ServiceExecutionRequest<InternalNotificationSearchCriteria> requestDMN;
                ServiceExecutionResponse<SearchResults<InternalNotification>> responseDMN;
                ServiceExecutionResponseDTO<SearchResultsDTO<InternalNotificationDTO>> responseDTO;

                // ...
                responseDTO = ServiceExecutionContext.HandleRequestDTO<InternalNotificationSearchCriteriaDTO, InternalNotificationSearchCriteria, SearchResultsDTO<InternalNotificationDTO>, SearchResults<InternalNotification>>(requestDTO, InternalNotificationSearchCriteriaMapper.Instance, out requestDMN, out bool isValidRequest);
                if (!isValidRequest) return responseDTO;

                #endregion
                #region BL.

                var domainResponse = await InternalNotification.Get(requestDMN.Content, requestDMN.ToRequestContext());

                #endregion
                #region response.

                return ServiceExecutionContext.PrepareResponse<InternalNotificationSearchCriteriaDTO, SearchResults<InternalNotification>, SearchResultsDTO<InternalNotificationDTO>>(requestDTO, responseDTO, domainResponse.State, domainResponse.Message, domainResponse.Result, InternalNotificationSearchResultsMapper.Instance, serviceName);

                #endregion
            }
            #region Exception

            catch (Exception x)
            {
                XLogger.Error($"Exception : {x}");
                return ServiceExecutionContext.PrepareResponseError<InternalNotificationSearchCriteriaDTO, SearchResultsDTO<InternalNotificationDTO>>(request, serviceName);
            }
            #endregion
        }

        #endregion
        #region InternalNotification : Create

        [HttpPost]
        [Route("Create")]
        public async Task<ServiceExecutionResponseDTO<InternalNotificationDTO>> InternalNotificationCreate(ServiceExecutionRequestDTO<InternalNotificationDTO> request)
        {
            string serviceName = "xcore.notifications.internal.notification.create";

            try
            {
                #region request.

                // ...
                ServiceExecutionRequestDTO<InternalNotificationDTO> requestDTO = request;
                ServiceExecutionRequest<InternalNotification> requestDMN;
                ServiceExecutionResponse<InternalNotification> responseDMN;
                ServiceExecutionResponseDTO<InternalNotificationDTO> responseDTO;

                // ...
                responseDTO = ServiceExecutionContext.HandleRequestDTO<InternalNotificationDTO, InternalNotification, InternalNotificationDTO, InternalNotification>(requestDTO, InternalNotificationMapper.Instance, out requestDMN, out bool isValidRequest);
                if (!isValidRequest) return responseDTO;

                #endregion
                #region BL.

                var domainResponse = await InternalNotification.Create(requestDMN.Content, requestDMN.ToRequestContext());

                #endregion
                #region response.

                var res = ServiceExecutionContext.PrepareResponse<InternalNotificationDTO, InternalNotification, InternalNotificationDTO>(requestDTO, responseDTO, domainResponse.State, domainResponse.Message, domainResponse.Result, InternalNotificationMapper.Instance, serviceName);
                return res;
                #endregion
            }
            #region Exception

            catch (Exception x)
            {
                XLogger.Error($"Exception : {x}");
                return ServiceExecutionContext.PrepareResponseError<InternalNotificationDTO, InternalNotificationDTO>(request, serviceName);
            }

            #endregion
        }

        #endregion
        #region InternalNotification : Delete        

        [HttpPost]
        [Route("Delete")]
        public async Task<ServiceExecutionResponseDTO<bool>> InternalNotificationDelete(ServiceExecutionRequestDTO<int> request)
        {
            string serviceName = "xcore.notifications.internal.notification.Delete";

            try
            {
                #region request.

                // ...
                bool isValidRequest;
                ServiceExecutionRequestDTO<int> requestDTO = request;
                ServiceExecutionRequest<int> requestDMN;
                ServiceExecutionResponse<bool> responseDMN = new ServiceExecutionResponse<bool>();
                ServiceExecutionResponseDTO<bool> responseDTO;

                // ...
                responseDTO = ServiceExecutionContext.HandleRequestDTO<int, int, bool, bool>(requestDTO, NativeMapper<int>.Instance, out requestDMN, out isValidRequest);
                if (!isValidRequest) return responseDTO;

                #endregion
                #region BL.

                var domainResponse = await InternalNotification.Delete(requestDMN.Content, requestDMN.ToRequestContext());

                #endregion
                #region response.

                return ServiceExecutionContext.PrepareResponse<int, bool, bool>(requestDTO, responseDMN, domainResponse.State, domainResponse.Message, domainResponse.Result, NativeMapper<bool>.Instance, serviceName);

                #endregion
            }
            #region Exception

            catch (Exception x)
            {
                XLogger.Error($"Exception : {x}");
                return ServiceExecutionContext.PrepareResponseError<int, bool>(request, serviceName);
            }

            #endregion
        }

        #endregion
        #region InternalNotification : MarkasRead
        [HttpPost]
        [Route("MarkasReadInternalNotification")]
        public async Task<ServiceExecutionResponseDTO<bool>> MarkasRead(ServiceExecutionRequestDTO<List<int?>> request)
        {
            string serviceName = "xcore.notifications.internal.notification.MarkasRead";

            try
            {
                #region request.

                // ...
                bool isValidRequest;
                ServiceExecutionRequestDTO<List<int?>> requestDTO = request;
                ServiceExecutionRequest<List<int?>> requestDMN;
                ServiceExecutionResponse<bool> responseDMN = new ServiceExecutionResponse<bool>();
                ServiceExecutionResponseDTO<bool> responseDTO;

                // ...
                responseDTO = ServiceExecutionContext.HandleRequestDTO <List<int?>, List<int?>, bool, bool>(requestDTO, NativeMapper<List<int?>>.Instance, out requestDMN, out isValidRequest);
                if (!isValidRequest) return responseDTO;

                #endregion
                #region BL.

                var domainResponse = await InternalNotification.MarkasRead(requestDMN.Content, requestDMN.ToRequestContext());

                #endregion
                #region response.

                return ServiceExecutionContext.PrepareResponse<List<int?>, bool, bool>(requestDTO, responseDMN, domainResponse.State, domainResponse.Message, domainResponse.Result, NativeMapper<bool>.Instance, serviceName);

                #endregion
            }
            #region Exception

            catch (Exception x)
            {
                XLogger.Error($"Exception : {x}");
                return ServiceExecutionContext.PrepareResponseError<List<int?>, bool>(request, serviceName);
            }

            #endregion
        }

        #endregion
        #region InternalNotification : MarkasDismissed
        [HttpPost]
        [Route("MarkasDismissedInternalNotification")]
        public async Task<ServiceExecutionResponseDTO<bool>> MarkasDismissed(ServiceExecutionRequestDTO<int> request)
        {
            string serviceName = "xcore.notifications.internal.notification.MarkasDismissed";

            try
            {
                #region request.

                // ...
                bool isValidRequest;
                ServiceExecutionRequestDTO<int> requestDTO = request;
                ServiceExecutionRequest<int> requestDMN;
                ServiceExecutionResponse<bool> responseDMN = new ServiceExecutionResponse<bool>();
                ServiceExecutionResponseDTO<bool> responseDTO;

                // ...
                responseDTO = ServiceExecutionContext.HandleRequestDTO<int, int, bool, bool>(requestDTO, NativeMapper<int>.Instance, out requestDMN, out isValidRequest);
                if (!isValidRequest) return responseDTO;

                #endregion
                #region BL.

                var domainResponse = await InternalNotification.MarkasDismissed(requestDMN.Content, requestDMN.ToRequestContext());

                #endregion
                #region response.

                return ServiceExecutionContext.PrepareResponse<int, bool, bool>(requestDTO, responseDMN, domainResponse.State, domainResponse.Message, domainResponse.Result, NativeMapper<bool>.Instance, serviceName);

                #endregion
            }
            #region Exception

            catch (Exception x)
            {
                XLogger.Error($"Exception : {x}");
                return ServiceExecutionContext.PrepareResponseError<int, bool>(request, serviceName);
            }

            #endregion
        }

        #endregion
        #region InternalNotification : MarkasDeleted
        [HttpPost]
        [Route("MarkasDeletedInternalNotification")]
        public async Task<ServiceExecutionResponseDTO<bool>> MarkasDeleted(ServiceExecutionRequestDTO<int> request)
        {
            string serviceName = "xcore.notifications.internal.notification.MarkasDeleted";

            try
            {
                #region request.

                // ...
                bool isValidRequest;
                ServiceExecutionRequestDTO<int> requestDTO = request;
                ServiceExecutionRequest<int> requestDMN;
                ServiceExecutionResponse<bool> responseDMN = new ServiceExecutionResponse<bool>();
                ServiceExecutionResponseDTO<bool> responseDTO;

                // ...
                responseDTO = ServiceExecutionContext.HandleRequestDTO<int, int, bool, bool>(requestDTO, NativeMapper<int>.Instance, out requestDMN, out isValidRequest);
                if (!isValidRequest) return responseDTO;

                #endregion
                #region BL.

                var domainResponse = await InternalNotification.MarkasDeleted(requestDMN.Content, requestDMN.ToRequestContext());

                #endregion
                #region response.

                return ServiceExecutionContext.PrepareResponse<int, bool, bool>(requestDTO, responseDMN, domainResponse.State, domainResponse.Message, domainResponse.Result, NativeMapper<bool>.Instance, serviceName);

                #endregion
            }
            #region Exception

            catch (Exception x)
            {
                XLogger.Error($"Exception : {x}");
                return ServiceExecutionContext.PrepareResponseError<int, bool>(request, serviceName);
            }

            #endregion
        }

        #endregion

        #endregion
        #region helpers.
        private bool Initialize()
        {
            bool isValid = true;

            isValid = isValid && (InternalNotification?.Initialized ?? false);

            return isValid;
        }
        #endregion
    }
}
