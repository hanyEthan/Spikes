using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using XCore.Framework.Infrastructure.Context.Execution.Support;
using XCore.Framework.Infrastructure.Context.Services.Handlers;
using XCore.Framework.Infrastructure.Context.Services.Models;
using XCore.Framework.Infrastructure.Entities.Mappers;
using XCore.Framework.Infrastructure.Entities.Repositories.Models;
using XCore.Framework.Utilities;
using XCore.Services.Configurations.Core.Contracts;
using XCore.Services.Configurations.Core.Models.Domain;
using XCore.Services.Configurations.Core.Models.Support;
using XCore.Services.Configurations.Mappers;
using XCore.Services.Configurations.Models;

namespace XCore.Services.Configurations.API.Controllers
{
    [ApiVersion("0.1")]
    [ApiController]
    [Route("Api/v{version:apiVersion}/[controller]")]
    public class AppController : ControllerBase
    {
        #region props.

        public bool? Initialized { get; protected set; }
        private readonly IConfigHandler ConfigHandler;

        #endregion
        #region cst.

        public AppController(IConfigHandler configHandler)
        {
            ConfigHandler = configHandler;
            this.Initialized = Initialize();
        }

        #endregion
        #region actions.

        #region App : Create

        [HttpPost]
        [Route("Create")]
        public async Task<ServiceExecutionResponseDTO<AppDTO>> AppCreate(ServiceExecutionRequestDTO<AppDTO> request)
        {
            string serviceName = "xcore.config.apps.create";

            try
            {
                #region request.

                // ...
                ServiceExecutionRequestDTO<AppDTO> requestDTO = request;
                ServiceExecutionRequest<App> requestDMN;
                ServiceExecutionResponse<App> responseDMN;
                ServiceExecutionResponseDTO<AppDTO> responseDTO;

                // ...
                responseDTO = ServiceExecutionContext.HandleRequestDTO<AppDTO, App, AppDTO, App>(requestDTO, AppMapper.Instance, out requestDMN, out bool isValidRequest, httpRequest: base.Request);
                if (!isValidRequest) return responseDTO;

                #endregion
                #region BL.

                var domainResponse = await ConfigHandler.Create(requestDMN.Content, requestDMN.ToRequestContext());

                #endregion
                #region response.

                var res = ServiceExecutionContext.PrepareResponse<AppDTO, App, AppDTO>(requestDTO, responseDTO, domainResponse.State, domainResponse.Message, domainResponse.Result, AppMapper.Instance, serviceName);
                return res;
                #endregion
            }
            #region Exception

            catch (Exception x)
            {
                XLogger.Error($"Exception : {x}");
                return ServiceExecutionContext.PrepareResponseError<AppDTO, AppDTO>(request, serviceName);
            }

            #endregion
        }

        #endregion
        #region App : Edit

        [HttpPost]
        [Route("Edit")]
        public async Task<ServiceExecutionResponseDTO<AppDTO>> AppEdit(ServiceExecutionRequestDTO<AppDTO> request)
        {
            string serviceName = "xcore.config.apps.edit";

            try
            {
                #region check.

                // Check();

                #endregion
                #region request.

                // ...
                bool isValidRequest;
                ServiceExecutionRequestDTO<AppDTO> requestDTO = request;
                ServiceExecutionRequest<App> requestDMN;
                ServiceExecutionResponse<App> responseDMN;
                ServiceExecutionResponseDTO<AppDTO> responseDTO;

                // ...
                responseDTO = ServiceExecutionContext.HandleRequestDTO<AppDTO, App, AppDTO, App>(requestDTO, AppMapper.Instance , out requestDMN, out isValidRequest, httpRequest: base.Request);
                if (!isValidRequest) return responseDTO;

                #endregion
                #region BL.

                var domainResponse = await ConfigHandler.Edit(requestDMN.Content, requestDMN.ToRequestContext());

                #endregion
                #region response.

                return ServiceExecutionContext.PrepareResponse<AppDTO, App, AppDTO>(requestDTO, responseDTO, domainResponse.State, domainResponse.Message, domainResponse.Result, AppMapper.Instance, serviceName);

                #endregion
            }
            #region Exception

            catch (Exception x)
            {
                XLogger.Error($"Exception : {x}");
                return ServiceExecutionContext.PrepareResponseError<AppDTO, AppDTO>(request, serviceName);
            }

            #endregion
        }

        #endregion
        #region App : Delete

        [HttpPost]
        [Route("Delete")]
        public async Task<ServiceExecutionResponseDTO<bool>> AppDelete(ServiceExecutionRequestDTO<string> request)
        {
            string serviceName = "xcore.config.apps.delete";

            try
            {
                #region request.

                // ...
                bool isValidRequest;
                ServiceExecutionRequestDTO<string> requestDTO = request;
                ServiceExecutionRequest<string> requestDMN;
                ServiceExecutionResponse<bool> responseDMN ;
                ServiceExecutionResponseDTO<bool> responseDTO;

                // ...
                responseDTO = ServiceExecutionContext.HandleRequestDTO<string, string, bool, bool>(requestDTO, NativeMapper<string>.Instance, out requestDMN, out isValidRequest, httpRequest: base.Request);
                if (!isValidRequest) return responseDTO;

                #endregion
                #region BL.

                var domainResponse = await ConfigHandler.DeleteApp(requestDMN.Content, requestDMN.ToRequestContext());

                #endregion
                #region response.

                return ServiceExecutionContext.PrepareResponse<string, bool, bool>(requestDTO, responseDTO, domainResponse.State, domainResponse.Message, domainResponse.Result, NativeMapper<bool>.Instance, serviceName);

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
        #region App : Get

        [HttpPost("Get")]
        public async Task<ServiceExecutionResponseDTO<SearchResultsDTO<AppDTO>>> AppGet(ServiceExecutionRequestDTO<AppSearchCriteriaDTO> request)
        
        {
            string serviceName = "xcore.config.apps.get";

            try
            {
                #region request.

                // ...
                ServiceExecutionRequestDTO<AppSearchCriteriaDTO> requestDTO = request;
                ServiceExecutionRequest<AppSearchCriteria> requestDMN;
                ServiceExecutionResponse<SearchResults<App>> responseDMN;
                ServiceExecutionResponseDTO<SearchResultsDTO<AppDTO>> responseDTO;

                // ...
                responseDTO = ServiceExecutionContext.HandleRequestDTO<AppSearchCriteriaDTO, AppSearchCriteria, SearchResultsDTO<AppDTO>, SearchResults<App>>(requestDTO, AppSearchCriteriaMapper.Instance, out requestDMN, out bool isValidRequest, httpRequest: base.Request);
                if (!isValidRequest) return responseDTO;

                #endregion
                #region BL.

                var domainResponse = await ConfigHandler.Get(requestDMN.Content, requestDMN.ToRequestContext());

                #endregion
                #region response.

                return ServiceExecutionContext.PrepareResponse<AppSearchCriteriaDTO, SearchResults<App>, SearchResultsDTO<AppDTO>>(requestDTO, responseDTO, domainResponse.State, domainResponse.Message, domainResponse.Result, AppSearchResultsMapper.Instance, serviceName);

                #endregion
            }
            #region Exception

            catch (Exception x)
            {
                XLogger.Error($"Exception : {x}");
                return ServiceExecutionContext.PrepareResponseError<AppSearchCriteriaDTO, SearchResultsDTO<AppDTO>>(request, serviceName);
            }
            #endregion
        }

        #endregion
        #region App : Activate
        [HttpPost]
        [Route("Activate")]
        public async Task<ServiceExecutionResponseDTO<bool>> ActivateApp(ServiceExecutionRequestDTO<string> request)
        {
            string serviceName = "xcore.config.apps.activate";

            try
            {
                #region request.

                // ...
                bool isValidRequest;
                ServiceExecutionRequestDTO<string> requestDTO = request;
                ServiceExecutionRequest<string> requestDMN;
                ServiceExecutionResponse<bool> responseDMN ;
                ServiceExecutionResponseDTO<bool> responseDTO;

                // ...
                responseDTO = ServiceExecutionContext.HandleRequestDTO<string, string, bool, bool>(requestDTO, NativeMapper<string>.Instance, out requestDMN, out isValidRequest, httpRequest: base.Request);
                if (!isValidRequest) return responseDTO;

                #endregion
                #region BL.

                var domainResponse = await ConfigHandler.ActivateApp(requestDMN.Content, requestDMN.ToRequestContext());

                #endregion
                #region response.

                return ServiceExecutionContext.PrepareResponse<string, bool, bool>(requestDTO, responseDTO, domainResponse.State, domainResponse.Message, domainResponse.Result, NativeMapper<bool>.Instance, serviceName);

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
        #region App : DeActivate

        [HttpPost]
        [Route("Deactivate")]
        public async Task<ServiceExecutionResponseDTO<bool>> DeActivateApp(ServiceExecutionRequestDTO<string> request)
        {
            string serviceName = "xcore.config.apps.deactivate";

            try
            {
                #region request.

                // ...
                bool isValidRequest;
                ServiceExecutionRequestDTO<string> requestDTO = request;
                ServiceExecutionRequest<string> requestDMN;
                ServiceExecutionResponse<bool> responseDMN;
                ServiceExecutionResponseDTO<bool> responseDTO;

                // ...
                responseDTO = ServiceExecutionContext.HandleRequestDTO<string, string, bool, bool>(requestDTO, NativeMapper<string>.Instance, out requestDMN, out isValidRequest, httpRequest: base.Request);
                if (!isValidRequest) return responseDTO;

                #endregion
                #region BL.

                var domainResponse = await ConfigHandler.DeactivateApp(requestDMN.Content, requestDMN.ToRequestContext());

                #endregion
                #region response.

                return ServiceExecutionContext.PrepareResponse<string, bool, bool>(requestDTO, responseDTO, domainResponse.State, domainResponse.Message, domainResponse.Result, NativeMapper<bool>.Instance, serviceName);

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

            isValid = isValid && (ConfigHandler?.Initialized ?? false);

            return isValid;
        }

        #endregion
    }
}