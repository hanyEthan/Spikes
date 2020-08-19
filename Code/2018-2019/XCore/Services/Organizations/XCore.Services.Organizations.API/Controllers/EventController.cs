using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using XCore.Framework.Infrastructure.Context.Services.Handlers;
using XCore.Framework.Infrastructure.Context.Services.Models;
using XCore.Framework.Infrastructure.Entities.Mappers;
using XCore.Framework.Infrastructure.Entities.Repositories.Models;
using XCore.Framework.Utilities;
using XCore.Services.Organizations.API.Mappers;
using XCore.Services.Organizations.API.Models;
using XCore.Services.Organizations.API.Models.Event;
using XCore.Services.Organizations.Core.Contracts;
using XCore.Services.Organizations.Core.Models.Domain;
using XCore.Services.Organizations.Core.Models.Support;

namespace XCore.Services.Organizations.API.Controllers
{
    [ApiVersion("0.1")]
    [ApiController]
    [Route("Api/v{version:apiVersion}/[controller]")]
    public class EventController : ControllerBase
    {
        #region props.
        public bool? Initialized { get; protected set; }
        private readonly IEventHandler EventHandler;
        #endregion
        #region cst.

        public EventController(IEventHandler eventHandler)
        {
            EventHandler = eventHandler;
            this.Initialized = Initialize();
        }

        #endregion
        #region actions.
        #region Event : Create

        [HttpPost]
        [Route("Create")]
        public async Task<ServiceExecutionResponseDTO<EventDTO>> Create(ServiceExecutionRequestDTO<EventDTO> request)
        {
            string serviceName = "xcore.org.Event.create";

            try
            {
                #region request.

                // ...
                ServiceExecutionRequestDTO<EventDTO> requestDTO = request;
                ServiceExecutionRequest<Event> requestDMN;
                ServiceExecutionResponse<Event> responseDMN;
                ServiceExecutionResponseDTO<EventDTO> responseDTO;

                // ...
                responseDTO = ServiceExecutionContext.HandleRequestDTO<EventDTO, Event, EventDTO, Event>(requestDTO, EventMapper.Instance, out requestDMN, out bool isValidRequest);
                if (!isValidRequest) return responseDTO;

                #endregion
                #region BL.

                var domainResponse = await EventHandler.Create(requestDMN.Content, requestDMN.ToRequestContext());

                #endregion
                #region response.

                var res = ServiceExecutionContext.PrepareResponse<EventDTO, Event, EventDTO>(requestDTO, responseDTO, domainResponse.State, domainResponse.Message, domainResponse.Result, EventMapper.Instance, serviceName);
                return res;
                #endregion
            }
            #region Exception

            catch (Exception x)
            {
                XLogger.Error($"Exception : {x}");
                return ServiceExecutionContext.PrepareResponseError<EventDTO, EventDTO>(request, serviceName);
            }

            #endregion
        }

        #endregion
        #region Event : Edit

        [HttpPost]
        [Route("Edit")]
        public async Task<ServiceExecutionResponseDTO<EventDTO>> Edit(ServiceExecutionRequestDTO<EventDTO> request)
        {
            string serviceName = "xcore.org.Event.edit";

            try
            {
                #region request.

                // ...
                bool isValidRequest;
                ServiceExecutionRequestDTO<EventDTO> requestDTO = request;
                ServiceExecutionRequest<Event> requestDMN;
                ServiceExecutionResponse<Event> responseDMN;
                ServiceExecutionResponseDTO<EventDTO> responseDTO;

                // ...
                responseDTO = ServiceExecutionContext.HandleRequestDTO<EventDTO, Event, EventDTO, Event>(requestDTO, EventMapper.Instance


                    , out requestDMN, out isValidRequest);
                if (!isValidRequest) return responseDTO;

                #endregion
                #region BL.

                var domainResponse = await EventHandler.Edit(requestDMN.Content, requestDMN.ToRequestContext());

                #endregion
                #region response.

                return ServiceExecutionContext.PrepareResponse<EventDTO, Event, EventDTO>(requestDTO, responseDTO, domainResponse.State, domainResponse.Message, domainResponse.Result, EventMapper.Instance, serviceName);

                #endregion
            }
            #region Exception

            catch (Exception x)
            {
                XLogger.Error($"Exception : {x}");
                return ServiceExecutionContext.PrepareResponseError<EventDTO, EventDTO>(request, serviceName);
            }

            #endregion
        }

        #endregion
        #region Event : Get
        [HttpPost("Get")]
        public async Task<ServiceExecutionResponseDTO<SearchResultsDTO<EventDTO>>> Get(ServiceExecutionRequestDTO<EventSearchCriteriaDTO> request)
        {
            string serviceName = "xcore.org.Event.get";

            try
            {
                #region request.

                // ...
                ServiceExecutionRequestDTO<EventSearchCriteriaDTO> requestDTO = request;
                ServiceExecutionRequest<EventSearchCriteria> requestDMN;
                ServiceExecutionResponse<SearchResults<Event>> responseDMN = new ServiceExecutionResponse<SearchResults<Event>>();
                ServiceExecutionResponseDTO<SearchResultsDTO<EventDTO>> responseDTO;

                // ...
                responseDTO = ServiceExecutionContext.HandleRequestDTO<EventSearchCriteriaDTO, EventSearchCriteria, SearchResultsDTO<EventDTO>, SearchResults<Event>>(requestDTO, EventSearchCriteriaMapper.Instance, out requestDMN, out bool isValidRequest);
                if (!isValidRequest) return responseDTO;

                #endregion
                #region BL.

                var domainResponse = await EventHandler.Get(requestDMN.Content, requestDMN.ToRequestContext());

                #endregion
                #region response.

                return ServiceExecutionContext.PrepareResponse<EventSearchCriteriaDTO, SearchResults<Event>, SearchResultsDTO<EventDTO>>(requestDTO, responseDMN, domainResponse.State, domainResponse.Message, domainResponse.Result, EventSearchResultsMapper.Instance, serviceName);

                #endregion
            }
            #region Exception

            catch (Exception x) 
            {
                XLogger.Error($"Exception : {x}");
                return ServiceExecutionContext.PrepareResponseError<EventSearchCriteriaDTO, SearchResultsDTO<EventDTO>>(request, serviceName);
            }
            #endregion
        }

        #endregion
        #region Event : Activate
        [HttpPost]
        [Route("Activate")]
        public async Task<ServiceExecutionResponseDTO<bool>> Activate(ServiceExecutionRequestDTO<string> request)
        {
            string serviceName = "xcore.org.Event.activate";

            try
            {
                #region request.

                // ...
                bool isValidRequest;
                ServiceExecutionRequestDTO<string> requestDTO = request;
                ServiceExecutionRequest<string> requestDMN;
                ServiceExecutionResponse<bool> responseDMN = new ServiceExecutionResponse<bool>();
                ServiceExecutionResponseDTO<bool> responseDTO;

                // ...
                responseDTO = ServiceExecutionContext.HandleRequestDTO<string, string, bool, bool>(requestDTO, NativeMapper<string>.Instance, out requestDMN, out isValidRequest);
                if (!isValidRequest) return responseDTO;

                #endregion
                #region BL.

                var domainResponse = await EventHandler.Activate(requestDMN.Content, requestDMN.ToRequestContext());

                #endregion
                #region response.

                return ServiceExecutionContext.PrepareResponse<string, bool, bool>(requestDTO, responseDMN, domainResponse.State, domainResponse.Message, domainResponse.Result, NativeMapper<bool>.Instance, serviceName);

                #endregion
            }
            #region Exception

            catch (Exception x)
            {
                XLogger.Error($"Exception : {x}");
                return ServiceExecutionContext.PrepareResponseError<string, bool>(request, serviceName);
            }

            #endregion
        }

        #endregion
        #region Event : DeActivate

        [HttpPost]
        [Route("DeActivate")]
        public async Task<ServiceExecutionResponseDTO<bool>> DeActivate(ServiceExecutionRequestDTO<string> request)
        {
            string serviceName = "xcore.org.Event.deactivate";

            try
            {
                #region request.

                // ...
                bool isValidRequest;
                ServiceExecutionRequestDTO<string> requestDTO = request;
                ServiceExecutionRequest<string> requestDMN;
                ServiceExecutionResponse<bool> responseDMN = new ServiceExecutionResponse<bool>();
                ServiceExecutionResponseDTO<bool> responseDTO;

                // ...
                responseDTO = ServiceExecutionContext.HandleRequestDTO<string, string, bool, bool>(requestDTO, NativeMapper<string>.Instance, out requestDMN, out isValidRequest);
                if (!isValidRequest) return responseDTO;

                #endregion
                #region BL.

                var domainResponse = await EventHandler.Deactivate(requestDMN.Content, requestDMN.ToRequestContext());

                #endregion
                #region response.

                return ServiceExecutionContext.PrepareResponse<string, bool, bool>(requestDTO, responseDMN, domainResponse.State, domainResponse.Message, domainResponse.Result, NativeMapper<bool>.Instance, serviceName);

                #endregion
            }
            #region Exception

            catch (Exception x)
            {
                XLogger.Error($"Exception : {x}");
                return ServiceExecutionContext.PrepareResponseError<string, bool>(request, serviceName);
            }

            #endregion
        }

        #endregion
        #region Event : Delete
        [HttpPost]
        [Route("Delete")]
        public async Task<ServiceExecutionResponseDTO<bool>> Delete(ServiceExecutionRequestDTO<string> request)
        {
            string serviceName = "xcore.org.Event.delete";

            try
            {
                #region request.

                // ...
                bool isValidRequest;
                ServiceExecutionRequestDTO<string> requestDTO = request;
                ServiceExecutionRequest<string> requestDMN;
                ServiceExecutionResponse<bool> responseDMN = new ServiceExecutionResponse<bool>();
                ServiceExecutionResponseDTO<bool> responseDTO;

                // ...
                responseDTO = ServiceExecutionContext.HandleRequestDTO<string, string, bool, bool>(requestDTO, NativeMapper<string>.Instance, out requestDMN, out isValidRequest);
                if (!isValidRequest) return responseDTO;

                #endregion
                #region BL.

                var domainResponse = await EventHandler.Delete(requestDMN.Content, requestDMN.ToRequestContext());

                #endregion
                #region response.

                return ServiceExecutionContext.PrepareResponse<string, bool, bool>(requestDTO, responseDMN, domainResponse.State, domainResponse.Message, domainResponse.Result, NativeMapper<bool>.Instance, serviceName);

                #endregion
            }
            #region Exception

            catch (Exception x)
            {
                XLogger.Error($"Exception : {x}");
                return ServiceExecutionContext.PrepareResponseError<string, bool>(request, serviceName);
            }

            #endregion

        }

        #endregion




        #endregion
        #region helpers.

        private bool Initialize()
        {
            bool isValid = true;

            isValid = isValid && (EventHandler?.Initialized ?? false);

            return isValid;
        }

        #endregion













    }
}