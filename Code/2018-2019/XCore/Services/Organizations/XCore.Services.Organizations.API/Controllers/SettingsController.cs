using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using XCore.Framework.Infrastructure.Context.Services.Handlers;
using XCore.Framework.Infrastructure.Context.Services.Models;
using XCore.Framework.Infrastructure.Entities.Mappers;
using XCore.Framework.Infrastructure.Entities.Repositories.Models;
using XCore.Framework.Utilities;
using XCore.Services.Organizations.API.Mappers;
using XCore.Services.Organizations.API.Models;
using XCore.Services.Organizations.API.Models.Settings;
using XCore.Services.Organizations.Core.Contracts;
using XCore.Services.Organizations.Core.Models.Domain;
using XCore.Services.Organizations.Core.Models.Support;

namespace XCore.Services.Organizations.API.Controllers
{
    [ApiVersion("0.1")]
    [ApiController]
    [Route("Api/v{version:apiVersion}/[controller]")]
    public class SettingsController : ControllerBase
    {
        #region props.
        public bool? Initialized { get; protected set; }
        private readonly ISettingsHandler SettingsHandler;
        #endregion
        #region cst.

        public SettingsController(ISettingsHandler settingsHandler)
        {
            SettingsHandler = settingsHandler;
            this.Initialized = Initialize();
        }

        #endregion
        #region actions.

        #region Settings : Create

        [HttpPost]
        [Route("Create")]
        public async Task<ServiceExecutionResponseDTO<SettingsDTO>> CreateSettings(ServiceExecutionRequestDTO<SettingsDTO> request)
        {
            string serviceName = "xcore.org.Settings.create";

            try
            {
                #region request.

                // ...
                ServiceExecutionRequestDTO<SettingsDTO> requestDTO = request;
                ServiceExecutionRequest<Settings> requestDMN;
                ServiceExecutionResponse<Settings> responseDMN;
                ServiceExecutionResponseDTO<SettingsDTO> responseDTO;

                // ...
                responseDTO = ServiceExecutionContext.HandleRequestDTO<SettingsDTO, Settings, SettingsDTO, Settings>(requestDTO, SettingsMapper.Instance, out requestDMN, out bool isValidRequest);
                if (!isValidRequest) return responseDTO;

                #endregion
                #region BL.

                var domainResponse = await SettingsHandler.Create(requestDMN.Content, requestDMN.ToRequestContext());

                #endregion
                #region response.

                var res = ServiceExecutionContext.PrepareResponse<SettingsDTO, Settings, SettingsDTO>(requestDTO, responseDTO, domainResponse.State, domainResponse.Message, domainResponse.Result, SettingsMapper.Instance, serviceName);
                return res;
                #endregion
            }
            #region Exception

            catch (Exception x)
            {
                XLogger.Error($"Exception : {x}");
                return ServiceExecutionContext.PrepareResponseError<SettingsDTO, SettingsDTO>(request, serviceName);
            }

            #endregion
        }

        #endregion
        #region Settings : Edit

        [HttpPost]
        [Route("Edit")]
        public async Task<ServiceExecutionResponseDTO<SettingsDTO>> EditSettings(ServiceExecutionRequestDTO<SettingsDTO> request)
        {
            string serviceName = "xcore.org.Settings.edit";

            try
            {
                #region request.

                // ...
                bool isValidRequest;
                ServiceExecutionRequestDTO<SettingsDTO> requestDTO = request;
                ServiceExecutionRequest<Settings> requestDMN;
                ServiceExecutionResponse<Settings> responseDMN;
                ServiceExecutionResponseDTO<SettingsDTO> responseDTO;

                // ...
                responseDTO = ServiceExecutionContext.HandleRequestDTO<SettingsDTO, Settings, SettingsDTO, Settings>(requestDTO, SettingsMapper.Instance


                    , out requestDMN, out isValidRequest);
                if (!isValidRequest) return responseDTO;

                #endregion
                #region BL.

                var domainResponse = await SettingsHandler.Edit(requestDMN.Content, requestDMN.ToRequestContext());

                #endregion
                #region response.

                return ServiceExecutionContext.PrepareResponse<SettingsDTO, Settings, SettingsDTO>(requestDTO, responseDTO, domainResponse.State, domainResponse.Message, domainResponse.Result, SettingsMapper.Instance, serviceName);

                #endregion
            }
            #region Exception

            catch (Exception x)
            {
                XLogger.Error($"Exception : {x}");
                return ServiceExecutionContext.PrepareResponseError<SettingsDTO, SettingsDTO>(request, serviceName);
            }

            #endregion
        }

        #endregion
        #region Settings : Get
        [HttpPost("Get")]
        public async Task<ServiceExecutionResponseDTO<SearchResultsDTO<SettingsDTO>>> GetSettings(ServiceExecutionRequestDTO<SettingsSearchCriteriaDTO> request)
        {
            string serviceName = "xcore.org.Settings.get";

            try
            {
                #region request.

                // ...
                ServiceExecutionRequestDTO<SettingsSearchCriteriaDTO> requestDTO = request;
                ServiceExecutionRequest<SettingsSearchCriteria> requestDMN;
                ServiceExecutionResponse<SearchResults<Settings>> responseDMN = new ServiceExecutionResponse<SearchResults<Settings>>();
                ServiceExecutionResponseDTO<SearchResultsDTO<SettingsDTO>> responseDTO;

                // ...
                responseDTO = ServiceExecutionContext.HandleRequestDTO<SettingsSearchCriteriaDTO, SettingsSearchCriteria, SearchResultsDTO<SettingsDTO>, SearchResults<Settings>>(requestDTO, SettingsSearchCriteriaMapper.Instance, out requestDMN, out bool isValidRequest);
                if (!isValidRequest) return responseDTO;

                #endregion
                #region BL.

                var domainResponse = await SettingsHandler.Get(requestDMN.Content, requestDMN.ToRequestContext());

                #endregion
                #region response.

                return ServiceExecutionContext.PrepareResponse<SettingsSearchCriteriaDTO, SearchResults<Settings>, SearchResultsDTO<SettingsDTO>>(requestDTO, responseDMN, domainResponse.State, domainResponse.Message, domainResponse.Result, SettingsSearchResultsMapper.Instance, serviceName);

                #endregion
            }
            #region Exception

            catch (Exception x)
            {
                XLogger.Error($"Exception : {x}");
                return ServiceExecutionContext.PrepareResponseError<SettingsSearchCriteriaDTO, SearchResultsDTO<SettingsDTO>>(request, serviceName);
            }
            #endregion
        }

        #endregion
        #region Settings : Activate
        [HttpPost]
        [Route("Activate")]
        public async Task<ServiceExecutionResponseDTO<bool>> ActivateSettings(ServiceExecutionRequestDTO<string> request)
        {
            string serviceName = "xcore.org.Settings.activate";

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

                var domainResponse = await SettingsHandler.Activate(requestDMN.Content, requestDMN.ToRequestContext());

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
        #region Settings : DeActivate

        [HttpPost]
        [Route("DeActivate")]
        public async Task<ServiceExecutionResponseDTO<bool>> DeActivateSettings(ServiceExecutionRequestDTO<string> request)
        {
            string serviceName = "xcore.org.Settings.deactivate";

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

                var domainResponse = await SettingsHandler.Deactivate(requestDMN.Content, requestDMN.ToRequestContext());

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
        #region Settings : Delete 
        [HttpPost]
        [Route("Delete")]
        public async Task<ServiceExecutionResponseDTO<bool>> Delete(ServiceExecutionRequestDTO<int> request)
        {
            string serviceName = "xcore.org.Settings.delete";

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

                var domainResponse = await SettingsHandler.Delete(requestDMN.Content, requestDMN.ToRequestContext());

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

            isValid = isValid && (SettingsHandler?.Initialized ?? false);

            return isValid;
        }

        #endregion
    }
}