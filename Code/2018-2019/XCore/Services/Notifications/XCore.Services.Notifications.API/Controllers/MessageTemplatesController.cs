using Microsoft.AspNetCore.Mvc;
using System;
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
    public class MessageTemplatesController : ControllerBase
    {
        #region props.
        public bool? Initialized { get; protected set; }
        private readonly IMessageTemplatesHandler MessageTemplatesHandler;
        #endregion
        #region cst.
        public MessageTemplatesController(IMessageTemplatesHandler messageTemplatesHandler)
        {
            this.MessageTemplatesHandler = messageTemplatesHandler;
            this.Initialized = Initialize();
        }
        #endregion
        #region actions.
        #region MessageTemplate : Create

        [HttpPost]
        [Route("Create")]
        public async Task<ServiceExecutionResponseDTO<MessageTemplateDTO>> MessageTemplateCreate(ServiceExecutionRequestDTO<MessageTemplateDTO> request)
        {
            string serviceName = "xcore.notifications.message.template.create";

            try
            {
                #region request.

                // ...
                ServiceExecutionRequestDTO<MessageTemplateDTO> requestDTO = request;
                ServiceExecutionRequest<MessageTemplate> requestDMN;
                ServiceExecutionResponse<MessageTemplate> responseDMN;
                ServiceExecutionResponseDTO<MessageTemplateDTO> responseDTO;

                // ...
                responseDTO = ServiceExecutionContext.HandleRequestDTO<MessageTemplateDTO, MessageTemplate, MessageTemplateDTO, MessageTemplate>(requestDTO, MessageTemplateMapper.Instance, out requestDMN, out bool isValidRequest);
                if (!isValidRequest) return responseDTO;

                #endregion
                #region BL.

                var domainResponse = await MessageTemplatesHandler.Create(requestDMN.Content, requestDMN.ToRequestContext());

                #endregion
                #region response.

                var res = ServiceExecutionContext.PrepareResponse<MessageTemplateDTO, MessageTemplate, MessageTemplateDTO>(requestDTO, responseDTO, domainResponse.State, domainResponse.Message, domainResponse.Result, MessageTemplateMapper.Instance, serviceName);
                return res;
                #endregion
            }
            #region Exception

            catch (Exception x)
            {
                XLogger.Error($"Exception : {x}");
                return ServiceExecutionContext.PrepareResponseError<MessageTemplateDTO, MessageTemplateDTO>(request, serviceName);
            }

            #endregion
        }

        #endregion
        #region MessageTemplate : Edit

        [HttpPost]
        [Route("Edit")]
        public async Task<ServiceExecutionResponseDTO<MessageTemplateDTO>> MessageTemplateEdit(ServiceExecutionRequestDTO<MessageTemplateDTO> request)
        {
            string serviceName = "xcore.notifications.message.template.edit";

            try
            {
                #region request.

                // ...
                bool isValidRequest;
                ServiceExecutionRequestDTO<MessageTemplateDTO> requestDTO = request;
                ServiceExecutionRequest<MessageTemplate> requestDMN;
                ServiceExecutionResponse<MessageTemplate> responseDMN;
                ServiceExecutionResponseDTO<MessageTemplateDTO> responseDTO;

                // ...
                responseDTO = ServiceExecutionContext.HandleRequestDTO<MessageTemplateDTO, MessageTemplate, MessageTemplateDTO, MessageTemplate>(requestDTO, MessageTemplateMapper.Instance, out requestDMN, out isValidRequest);
                if (!isValidRequest) return responseDTO;

                #endregion
                #region BL.

                var domainResponse = await MessageTemplatesHandler.Edit(requestDMN.Content, requestDMN.ToRequestContext());

                #endregion
                #region response.

                return ServiceExecutionContext.PrepareResponse<MessageTemplateDTO, MessageTemplate, MessageTemplateDTO>(requestDTO, responseDTO, domainResponse.State, domainResponse.Message, domainResponse.Result, MessageTemplateMapper.Instance, serviceName);

                #endregion
            }
            #region Exception

            catch (Exception x)
            {
                XLogger.Error($"Exception : {x}");
                return ServiceExecutionContext.PrepareResponseError<MessageTemplateDTO, MessageTemplateDTO>(request, serviceName);
            }

            #endregion
        }

        #endregion
        #region MessageTemplate : Delete        

        [HttpPost]
        [Route("Delete")]
        public async Task<ServiceExecutionResponseDTO<bool>> MessageTemplateDelete(ServiceExecutionRequestDTO<int> request)
        {
            string serviceName = "xcore.notifications.message.template.Delete";

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

                var domainResponse = await MessageTemplatesHandler.Delete(requestDMN.Content, requestDMN.ToRequestContext());

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
        #region MessageTemplate : Get


        [HttpPost("Get")]
        public async Task<ServiceExecutionResponseDTO<SearchResultsDTO<MessageTemplateDTO>>> AppGet(ServiceExecutionRequestDTO<MessageTemplateSearchCriteriaDTO> request)
        {
            string serviceName = "xcore.notifications.message.template.get";

            try
            {
                #region request.

                // ...
                ServiceExecutionRequestDTO<MessageTemplateSearchCriteriaDTO> requestDTO = request;
                ServiceExecutionRequest<MessageTemplateSearchCriteria> requestDMN;
                ServiceExecutionResponse<SearchResults<MessageTemplate>> responseDMN;
                ServiceExecutionResponseDTO<SearchResultsDTO<MessageTemplateDTO>> responseDTO;

                // ...
                responseDTO = ServiceExecutionContext.HandleRequestDTO<MessageTemplateSearchCriteriaDTO, MessageTemplateSearchCriteria, SearchResultsDTO<MessageTemplateDTO>, SearchResults<MessageTemplate>>(requestDTO, MessageTemplateSearchCriteriaMapper.Instance, out requestDMN, out bool isValidRequest);
                if (!isValidRequest) return responseDTO;

                #endregion
                #region BL.

                var domainResponse = await MessageTemplatesHandler.Get(requestDMN.Content, requestDMN.ToRequestContext());

                #endregion
                #region response.

                return ServiceExecutionContext.PrepareResponse<MessageTemplateSearchCriteriaDTO, SearchResults<MessageTemplate>, SearchResultsDTO<MessageTemplateDTO>>(requestDTO, responseDTO, domainResponse.State, domainResponse.Message, domainResponse.Result, MessageTemplateSearchResultsMapper.Instance, serviceName);

                #endregion
            }
            #region Exception

            catch (Exception x)
            {
                XLogger.Error($"Exception : {x}");
                return ServiceExecutionContext.PrepareResponseError<MessageTemplateSearchCriteriaDTO, SearchResultsDTO<MessageTemplateDTO>>(request, serviceName);
            }
            #endregion
        }

        #endregion
        #region MessageTemplate : Activate
        [HttpPost]
        [Route("ActivateMessageTemplate")]
        public async Task<ServiceExecutionResponseDTO<bool>> ActivateApp(ServiceExecutionRequestDTO<int> request)
        {
            string serviceName = "xcore.notifications.message.template.activate";

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

                var domainResponse = await MessageTemplatesHandler.Activate(requestDMN.Content, requestDMN.ToRequestContext());

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
        #region App : DeActivate

        [HttpPost]
        [Route("DeActivateMessageTemplate")]
        public async Task<ServiceExecutionResponseDTO<bool>> DeActivateApp(ServiceExecutionRequestDTO<int> request)
        {
            string serviceName = "xcore.Notification.message.template.deactivate";

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

                var domainResponse = await MessageTemplatesHandler.Deactivate(requestDMN.Content, requestDMN.ToRequestContext());

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
        #region MessageTemplate : Resolve


        [HttpPost("Resolve")]
        public async Task<ServiceExecutionResponseDTO<ResolveResponseDTO>> AppResolve(ServiceExecutionRequestDTO<ResolveRequestDTO> request)
        {
            string serviceName = "xcore.notifications.message.template.Resolve";

            try
            {
                #region request.

                // ...
                ServiceExecutionRequestDTO<ResolveRequestDTO> requestDTO = request;
                ServiceExecutionRequest<ResolveRequest> requestDMN;
                ServiceExecutionResponse<ResolveResponse> responseDMN;
                ServiceExecutionResponseDTO<ResolveResponseDTO> responseDTO;

                // ...
                responseDTO = ServiceExecutionContext.HandleRequestDTO<ResolveRequestDTO, ResolveRequest, ResolveResponseDTO, ResolveResponse>(requestDTO, ResolveRequestMapper.Instance, out requestDMN, out bool isValidRequest);
                if (!isValidRequest) return responseDTO;

                #endregion
                #region BL.

                var domainResponse = await MessageTemplatesHandler.Resolve(requestDMN.Content, requestDMN.ToRequestContext());

                #endregion
                #region response.

                return ServiceExecutionContext.PrepareResponse<ResolveRequestDTO, ResolveResponse, ResolveResponseDTO>(requestDTO, responseDTO, domainResponse.State, domainResponse.Message, domainResponse.Result, ResolveResponseMapper.Instance, serviceName);

                #endregion
            }
            #region Exception

            catch (Exception x)
            {
                XLogger.Error($"Exception : {x}");
                return ServiceExecutionContext.PrepareResponseError<ResolveRequestDTO, ResolveResponseDTO>(request, serviceName);
            }
            #endregion
        }

        #endregion
        #endregion
        #region helpers.
        private bool Initialize()
        {
            bool isValid = true;

            isValid = isValid && (MessageTemplatesHandler?.Initialized ?? false);

            return isValid;
        }
        #endregion
    }
}
