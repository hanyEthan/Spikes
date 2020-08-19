using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;
using XCore.Framework.Infrastructure.Context.Services.Handlers;
using XCore.Framework.Infrastructure.Context.Services.Models;
using XCore.Framework.Infrastructure.Entities.Mappers;
using XCore.Framework.Infrastructure.Entities.Repositories.Models;
using XCore.Framework.Utilities;
using XCore.Services.Security.API.Mappers;
using XCore.Services.Security.API.Model;
using XCore.Services.Security.Core.Contracts;
using XCore.Services.Security.Core.Models.Domain;
using XCore.Services.Security.Core.Models.Support;

namespace XCore.Services.Security.API.Controllers
{
    [ApiVersion("0.1")]
    [ApiController]
    [Route("Api/v{version:apiVersion}/[controller]")]
    public class ActorController : ControllerBase
    {
        #region props.

        public bool? Initialized { get; protected set; }
        private readonly IActorHandler ActorHandler;

        #endregion
        #region cst.

        public ActorController(IActorHandler actorHandler)
        {
            this.ActorHandler = actorHandler;
            this.Initialized = Initialize();
        }

        #endregion

        #region actions.

        #region Actor : Create

        [HttpPost]
        [Route("Create")]
        public async Task<ServiceExecutionResponseDTO<ActorDTO>> ActorCreate(ServiceExecutionRequestDTO<ActorDTO> request)
        {
            string serviceName = "xcore.security.Actors.Register";

            try
            {
                #region check.

                Check();

                #endregion
                #region request.

                // ...
                ServiceExecutionRequestDTO<ActorDTO> requestDTO = request;
                ServiceExecutionRequest<Actor> requestDMN;
                ServiceExecutionResponse<Actor> responseDMN;
                ServiceExecutionResponseDTO<ActorDTO> responseDTO;

                // ...
                responseDTO = ServiceExecutionContext.HandleRequestDTO<ActorDTO, Actor, ActorDTO, Actor>(requestDTO, ActorMapper.Instance, out requestDMN, out bool isValidRequest);
                if (!isValidRequest) return responseDTO;

                #endregion
                #region BL.

                var domainResponse = await ActorHandler.Create(requestDMN.Content, requestDMN.ToRequestContext());

                #endregion
                #region response.

                var res = ServiceExecutionContext.PrepareResponse<ActorDTO, Actor, ActorDTO>(requestDTO, responseDTO, domainResponse.State, domainResponse.Message, domainResponse.Result, ActorMapper.Instance, serviceName);
                return res;
                #endregion
            }
            #region Exception

            catch (Exception x)
            {
                XLogger.Error($"Exception : {x}");
                return ServiceExecutionContext.PrepareResponseError<ActorDTO, ActorDTO>(request, serviceName);
            }

            #endregion
        }

        #endregion
        #region Actor : Edit

        [HttpPost]
        [Route("Edit")]
        public async Task<ServiceExecutionResponseDTO<ActorDTO>> ActorEdit(ServiceExecutionRequestDTO<ActorDTO> request)
        {
            string serviceName = "xcore.security.Actors.edit";

            try
            {
                #region check.

                Check();

                #endregion
                #region request.

                // ...
                bool isValidRequest;
                ServiceExecutionRequestDTO<ActorDTO> requestDTO = request;
                ServiceExecutionRequest<Actor> requestDMN;
                ServiceExecutionResponse<Actor> responseDMN;
                ServiceExecutionResponseDTO<ActorDTO> responseDTO;

                // ...
                responseDTO = ServiceExecutionContext.HandleRequestDTO<ActorDTO, Actor, ActorDTO, Actor>(requestDTO, ActorMapper.Instance


                    , out requestDMN, out isValidRequest);
                if (!isValidRequest) return responseDTO;

                #endregion
                #region BL.

                var domainResponse = await ActorHandler.Edit(requestDMN.Content, requestDMN.ToRequestContext());

                #endregion
                #region response.

                return ServiceExecutionContext.PrepareResponse<ActorDTO, Actor, ActorDTO>(requestDTO, responseDTO, domainResponse.State, domainResponse.Message, domainResponse.Result, ActorMapper.Instance, serviceName);

                #endregion
            }
            #region Exception

            catch (Exception x)
            {
                XLogger.Error($"Exception : {x}");
                return ServiceExecutionContext.PrepareResponseError<ActorDTO, ActorDTO>(request, serviceName);
            }

            #endregion
        }

        #endregion
        #region Actor : Delete

        [HttpPost]
        [Route("Delete")]
        public async Task<ServiceExecutionResponseDTO<bool>> ActorDelete(ServiceExecutionRequestDTO<int> request)
        {
            string serviceName = "xcore.security.Actors.unregister";

            try
            {
                #region check.

                Check();

                #endregion
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

                var domainResponse = await ActorHandler.DeleteActor(requestDMN.Content, requestDMN.ToRequestContext());

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
        #region Actor : Get


        [HttpPost("Get")]
        public async Task<ServiceExecutionResponseDTO<SearchResultsDTO<ActorDTO>>> ActorGet(ServiceExecutionRequestDTO<ActorSearchCriteriaDTO> request)
        {
            string serviceName = "xcore.security.Actors.get";

            try
            {
                #region request.

                // ...
                ServiceExecutionRequestDTO<ActorSearchCriteriaDTO> requestDTO = request;
                ServiceExecutionRequest<ActorSearchCriteria> requestDMN;
                ServiceExecutionResponse<SearchResults<Actor>> responseDMN = new ServiceExecutionResponse<SearchResults<Actor>>();
                ServiceExecutionResponseDTO<SearchResultsDTO<ActorDTO>> responseDTO;

                // ...
                responseDTO = ServiceExecutionContext.HandleRequestDTO<ActorSearchCriteriaDTO, ActorSearchCriteria, SearchResultsDTO<ActorDTO>, SearchResults<Actor>>(requestDTO, ActorSearchCriteriaMapper.Instance, out requestDMN, out bool isValidRequest);
                if (!isValidRequest) return responseDTO;

                #endregion
                #region BL.

                var domainResponse = await ActorHandler.Get(requestDMN.Content, requestDMN.ToRequestContext());

                #endregion
                #region response.

                return ServiceExecutionContext.PrepareResponse<ActorSearchCriteriaDTO, SearchResults<Actor>, SearchResultsDTO<ActorDTO>>(requestDTO, responseDMN, domainResponse.State, domainResponse.Message, domainResponse.Result, ActorSearchResultsMapper.Instance, serviceName);

                #endregion
            }
            #region Exception

            catch (Exception x)
            {
                XLogger.Error($"Exception : {x}");
                return ServiceExecutionContext.PrepareResponseError<ActorSearchCriteriaDTO, SearchResultsDTO<ActorDTO>>(request, serviceName);
            }
            #endregion
        }

        #endregion
        #region Actor : Activate
        [HttpPost]
        [Route("ActivateActor")]
        public async Task<ServiceExecutionResponseDTO<bool>> ActivateActor(ServiceExecutionRequestDTO<int> request)
        {
            string serviceName = "xcore.config.Actors.activate";

            try
            {
                #region check.

                Check();

                #endregion
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

                var domainResponse = await ActorHandler.ActivateActor(requestDMN.Content, requestDMN.ToRequestContext());

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
        #region Actor : DeActivate

        [HttpPost]
        [Route("DeActivateActor")]
        public async Task<ServiceExecutionResponseDTO<bool>> DeActivateActor(ServiceExecutionRequestDTO<int> request)
        {
            string serviceName = "xcore.config.Actors.deactivate";

            try
            {
                #region check.

                Check();

                #endregion
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

                var domainResponse = await ActorHandler.DeactivateActor(requestDMN.Content, requestDMN.ToRequestContext());

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

            isValid = isValid && (ActorHandler?.Initialized ?? false);

            return isValid;
        }
        private void Check()
        {
            if (!this.Initialized.GetValueOrDefault())
            {
                throw new Exception("not initialized correctly.");
            }
        }

        #endregion
    }
}