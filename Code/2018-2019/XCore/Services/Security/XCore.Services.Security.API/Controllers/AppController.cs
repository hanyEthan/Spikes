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
    public class AppController : ControllerBase
    {
        #region props.

        public bool? Initialized { get; protected set; }
        private readonly IAppHandler AppHandler;

        #endregion
        #region cst.

        public AppController(IAppHandler appHandler)
        {
            this.AppHandler = appHandler;
            this.Initialized = Initialize();
        }

        #endregion

        #region actions.

        #region App : Register

        [HttpPost]
        [Route("Register")]
        public async Task<ServiceExecutionResponseDTO<AppDTO>> AppCreate(ServiceExecutionRequestDTO<AppDTO> request)
        {
            string serviceName = "xcore.security.apps.Register";

            try
            {
                #region check.

                Check();

                #endregion
                #region request.

                // ...
                ServiceExecutionRequestDTO<AppDTO> requestDTO = request;
                ServiceExecutionRequest<App> requestDMN;
                ServiceExecutionResponse<App> responseDMN;
                ServiceExecutionResponseDTO<AppDTO> responseDTO;

                // ...
                responseDTO = ServiceExecutionContext.HandleRequestDTO<AppDTO, App, AppDTO, App>(requestDTO, AppMapper.Instance, out requestDMN, out bool isValidRequest);
                if (!isValidRequest) return responseDTO;

                #endregion
                #region BL.

                var domainResponse = await AppHandler.Register(requestDMN.Content, requestDMN.ToRequestContext());

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
            string serviceName = "xcore.security.apps.edit";

            try
            {
                #region check.

                Check();

                #endregion
                #region request.

                // ...
                bool isValidRequest;
                ServiceExecutionRequestDTO<AppDTO> requestDTO = request;
                ServiceExecutionRequest<App> requestDMN;
                ServiceExecutionResponse<App> responseDMN;
                ServiceExecutionResponseDTO<AppDTO> responseDTO;

                // ...
                responseDTO = ServiceExecutionContext.HandleRequestDTO<AppDTO, App, AppDTO, App>(requestDTO, AppMapper.Instance


                    , out requestDMN, out isValidRequest);
                if (!isValidRequest) return responseDTO;

                #endregion
                #region BL.

                var domainResponse = await AppHandler.Edit(requestDMN.Content, requestDMN.ToRequestContext());

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
        #region App : Unregister

        [HttpPost]
        [Route("Unregister")]
        public async Task<ServiceExecutionResponseDTO<bool>> AppDelete(ServiceExecutionRequestDTO<int> request)
        {
            string serviceName = "xcore.security.apps.unregister";

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

                var domainResponse = await AppHandler.UnregisterApp(requestDMN.Content, requestDMN.ToRequestContext());

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
        #region App : Get


        [HttpPost("Get")]
        public async Task<ServiceExecutionResponseDTO<SearchResultsDTO<AppDTO>>> AppGet(ServiceExecutionRequestDTO<AppSearchCriteriaDTO> request)
        {
            string serviceName = "xcore.security.apps.get";

            try
            {
                #region check.

                Check();

                #endregion
                #region request.

                // ...
                ServiceExecutionRequestDTO<AppSearchCriteriaDTO> requestDTO = request;
                ServiceExecutionRequest<AppSearchCriteria> requestDMN;
                ServiceExecutionResponse<SearchResults<App>> responseDMN = new ServiceExecutionResponse<SearchResults<App>>();
                ServiceExecutionResponseDTO<SearchResultsDTO<AppDTO>> responseDTO;

                // ...
                responseDTO = ServiceExecutionContext.HandleRequestDTO<AppSearchCriteriaDTO, AppSearchCriteria, SearchResultsDTO<AppDTO>, SearchResults<App>>(requestDTO, AppSearchCriteriaMapper.Instance, out requestDMN, out bool isValidRequest);
                if (!isValidRequest) return responseDTO;

                #endregion
                #region BL.

                var domainResponse = await AppHandler.Get(requestDMN.Content, requestDMN.ToRequestContext());

                #endregion
                #region response.

                return ServiceExecutionContext.PrepareResponse<AppSearchCriteriaDTO, SearchResults<App>, SearchResultsDTO<AppDTO>>(requestDTO, responseDMN, domainResponse.State, domainResponse.Message, domainResponse.Result, AppSearchResultsMapper.Instance, serviceName);

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
        [Route("ActivateApp")]
        public async Task<ServiceExecutionResponseDTO<bool>> ActivateApp(ServiceExecutionRequestDTO<int> request)
        {
            string serviceName = "xcore.config.apps.activate";

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

                var domainResponse = await AppHandler.ActivateApp(requestDMN.Content, requestDMN.ToRequestContext());

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
        [Route("DeActivateApp")]
        public async Task<ServiceExecutionResponseDTO<bool>> DeActivateApp(ServiceExecutionRequestDTO<int> request)
        {
            string serviceName = "xcore.config.apps.deactivate";

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

                var domainResponse = await AppHandler.DeactivateApp(requestDMN.Content, requestDMN.ToRequestContext());

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

            isValid = isValid && (AppHandler?.Initialized ?? false);

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