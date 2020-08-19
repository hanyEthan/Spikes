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
    public class ConfigController : ControllerBase
    {
        #region props.

        public bool? Initialized { get; protected set; }
        private readonly IConfigHandler ConfigHandler;

        #endregion
        #region cst.

        public ConfigController(IConfigHandler configHandler)
        {
            ConfigHandler = configHandler;
            this.Initialized = Initialize();
        }

        #endregion
        #region actions.

        #region Config : Create

        [HttpPost]
        [Route("Create")]
        public async Task<ServiceExecutionResponseDTO<ConfigDTO>> ConfigCreate(ServiceExecutionRequestDTO<ConfigDTO> request)
        {
            string serviceName = "xcore.config.config.create";

            try
            {
                #region request.

                // ...
                ServiceExecutionRequestDTO<ConfigDTO> requestDTO = request;
                ServiceExecutionRequest<ConfigItem> requestDMN;
                ServiceExecutionResponse<ConfigItem> responseDMN;
                ServiceExecutionResponseDTO<ConfigDTO> responseDTO;

                // ...
                responseDTO = ServiceExecutionContext.HandleRequestDTO<ConfigDTO, ConfigItem, ConfigDTO, ConfigItem>(requestDTO, ConfigMapper.Instance, out requestDMN, out bool isValidRequest, httpRequest: base.Request);
                if (!isValidRequest) return responseDTO;

                #endregion
                #region BL.

                var domainResponse = await ConfigHandler.Create(requestDMN.Content, requestDMN.ToRequestContext());

                #endregion
                #region response.

                return ServiceExecutionContext.PrepareResponse<ConfigDTO, ConfigItem, ConfigDTO>(requestDTO, responseDTO, domainResponse.State, domainResponse.Message, domainResponse.Result, ConfigMapper.Instance, serviceName);

                #endregion
            }
            #region Exception

            catch (Exception x)
            {
                XLogger.Error($"Exception : {x}");
                return ServiceExecutionContext.PrepareResponseError<ConfigDTO, ConfigDTO>(request, serviceName);
            }

            #endregion
        }

        #endregion
        #region Config : Edit

        [HttpPost]
        [Route("Edit")]
        public async Task<ServiceExecutionResponseDTO<ConfigDTO>> ConfigEdit(ServiceExecutionRequestDTO<ConfigDTO> request)
        {
            string serviceName = "xcore.config.config.edit";

            try
            {
                #region request.

                // ...
                ServiceExecutionRequestDTO<ConfigDTO> requestDTO = request;
                ServiceExecutionRequest<ConfigItem> requestDMN;
                ServiceExecutionResponse<ConfigItem> responseDMN;
                ServiceExecutionResponseDTO<ConfigDTO> responseDTO;

                // ...
                responseDTO = ServiceExecutionContext.HandleRequestDTO<ConfigDTO, ConfigItem, ConfigDTO, ConfigItem>(requestDTO, ConfigMapper.Instance, out requestDMN, out bool isValidRequest, httpRequest: base.Request);
                if (!isValidRequest) return responseDTO;

                #endregion
                #region BL.

                var domainResponse = await ConfigHandler.Edit(requestDMN.Content, requestDMN.ToRequestContext());

                #endregion
                #region response.

                return ServiceExecutionContext.PrepareResponse<ConfigDTO, ConfigItem, ConfigDTO>(requestDTO, responseDTO, domainResponse.State, domainResponse.Message, domainResponse.Result, ConfigMapper.Instance, serviceName);

                #endregion
            }
            #region Exception

            catch (Exception x)
            {
                XLogger.Error($"Exception : {x}");
                return ServiceExecutionContext.PrepareResponseError<ConfigDTO, ConfigDTO>(request, serviceName);
            }

            #endregion
        }

        #endregion
        #region Config : Delete

        [HttpPost]
        [Route("Delete")]
        public async Task<ServiceExecutionResponseDTO<bool>> ConfigDelete(ServiceExecutionRequestDTO<string> request)
        {
            string serviceName = "xcore.config.config.delete";

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

                var domainResponse = await ConfigHandler.DeleteConfig(requestDMN.Content, requestDMN.ToRequestContext());

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
        #region Config : Activate

        [HttpPost]
        [Route("Activate")]
        public async Task<ServiceExecutionResponseDTO<bool>> ActivateConfig(ServiceExecutionRequestDTO<string> request)
        {
            string serviceName = "xcore.config.config.activate";

            try
            {
                #region request.

                // ...
                bool isValidRequest;
                ServiceExecutionRequestDTO<string> requestDTO = request;
                ServiceExecutionRequest<string> requestDMN;
                ServiceExecutionResponse<bool> responseDMN;
                ServiceExecutionResponseDTO<bool> responseDTO;
                int x = 0;
                // ...
                responseDTO = ServiceExecutionContext.HandleRequestDTO<string, string, bool, bool>(requestDTO, NativeMapper<string>.Instance, out requestDMN, out isValidRequest, httpRequest: base.Request);
                if (!isValidRequest) return responseDTO;

                #endregion
                #region BL.

                var domainResponse = await ConfigHandler.ActivateConfig(requestDMN.Content, requestDMN.ToRequestContext());

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
        #region Config : DeActivate

        [HttpPost]
        [Route("Deactivate")]
        public async Task<ServiceExecutionResponseDTO<bool>> DeActivateConfig(ServiceExecutionRequestDTO<string> request)
        {
            string serviceName = "xcore.config.config.deactivate";

            try
            {
                #region request.

                // ...
                bool isValidRequest;
                ServiceExecutionRequestDTO<string> requestDTO = request;
                ServiceExecutionRequest<string> requestDMN;
                ServiceExecutionResponse<bool> responseDMN;
                ServiceExecutionResponseDTO<bool> responseDTO;
                int x = 0;
                // ...
                responseDTO = ServiceExecutionContext.HandleRequestDTO<string, string, bool, bool>(requestDTO, NativeMapper<string>.Instance, out requestDMN, out isValidRequest, httpRequest: base.Request);
                if (!isValidRequest) return responseDTO;

                #endregion
                #region BL.

                var domainResponse = await ConfigHandler.DeactivateConfig(requestDMN.Content, requestDMN.ToRequestContext());

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
        #region Config : Set

        [HttpPost]
        [Route("Set")]
        public async Task<ServiceExecutionResponseDTO<bool>> ConfigSet(ServiceExecutionRequestDTO<ConfigSetRequest> request)
        {
            string serviceName = "xcore.config.config.create";

            try
            {
                #region request.

                // ...
                ServiceExecutionRequestDTO<ConfigSetRequest> requestDTO = request;
                ServiceExecutionRequest<ConfigSetRequest> requestDMN;



                ServiceExecutionResponse<bool> responseDMN;
                ServiceExecutionResponseDTO<bool> responseDTO;

                // ...
                responseDTO = ServiceExecutionContext.HandleRequestDTO<ConfigSetRequest, ConfigSetRequest, bool, bool>(requestDTO, NativeMapper<ConfigSetRequest>.Instance, out requestDMN, out bool isValidRequest, httpRequest: base.Request);

                if (!isValidRequest) return responseDTO;

                #endregion
                #region BL.

                var domainResponse = await ConfigHandler.Set(requestDMN.Content.key, requestDMN.Content.value, requestDMN.ToRequestContext());

                #endregion
                #region response.

                return ServiceExecutionContext.PrepareResponse<ConfigSetRequest, bool, bool>(requestDTO, responseDTO, domainResponse.State, domainResponse.Message, domainResponse.Result, NativeMapper<bool>.Instance, serviceName);

                #endregion
            }
            #region Exception

            catch (Exception x)
            {
                XLogger.Error($"Exception : {x}");
                return ServiceExecutionContext.PrepareResponseError<ConfigSetRequest, bool>(request, serviceName);
            }

            #endregion
        }

        #endregion
        #region Config : Get

        [HttpPost]
        [Route("Get")]
        public async Task<ServiceExecutionResponseDTO<ConfigDTO>> ConfigGet(ServiceExecutionRequestDTO<ConfigKeyDTO> request)
        {
            string serviceName = "xcore.config.config.get";

            try
            {
                #region request.

                // ...
                ServiceExecutionRequestDTO<ConfigKeyDTO> requestDTO = request;
                ServiceExecutionRequest<ConfigKey> requestDMN;
                ServiceExecutionResponse<ConfigItem> responseDMN = new ServiceExecutionResponse<ConfigItem>();
                ServiceExecutionResponseDTO<ConfigDTO> responseDTO;

                // ...
                responseDTO = ServiceExecutionContext.HandleRequestDTO<ConfigKeyDTO, ConfigKey, ConfigDTO, ConfigItem>(requestDTO, ConfigKeyMapper.Instance, out requestDMN, out bool isValidRequest, httpRequest: base.Request);
                if (!isValidRequest) return responseDTO;

                #endregion
                #region BL.

                var domainResponse = await ConfigHandler.Get(requestDMN.Content, requestDMN.ToRequestContext());

                #endregion
                #region response.

                return ServiceExecutionContext.PrepareResponse<ConfigKeyDTO, ConfigItem, ConfigDTO>(requestDTO, responseDTO, domainResponse.State, domainResponse.Message, domainResponse.Result, ConfigMapper.Instance, serviceName);

                #endregion
            }
            #region Exception

            catch (Exception x)
            {
                XLogger.Error($"Exception : {x}");
                return ServiceExecutionContext.PrepareResponseError<ConfigKeyDTO, ConfigDTO>(request, serviceName);
            }
            #endregion
        }

        [HttpPost]
        [Route("ConfigGet")]
        public async Task<ServiceExecutionResponseDTO<SearchResultsDTO<ConfigDTO>>> Get(ServiceExecutionRequestDTO<ConfigSearchCriteriaDTO> request)
        {
            string serviceName = "xcore.config.config.get";

            try
            {
                #region request.

                // ...
              

                // ...
                bool isValidRequest;
                ServiceExecutionRequestDTO<ConfigSearchCriteriaDTO> requestDTO = request;
                ServiceExecutionRequest<ConfigSearchCriteria> requestDMN;
                ServiceExecutionResponse<SearchResults<ConfigItem>> responseDMN ;
                ServiceExecutionResponseDTO<SearchResultsDTO<ConfigDTO>> responseDTO;

                // ...
                responseDTO = ServiceExecutionContext.HandleRequestDTO<ConfigSearchCriteriaDTO, ConfigSearchCriteria, SearchResultsDTO<ConfigDTO>, SearchResults<ConfigItem>>(requestDTO, ConfigSearchCriteriaMapper.Instance, out requestDMN, out isValidRequest, httpRequest: base.Request);
               
                if (!isValidRequest) return responseDTO;

                #endregion
                #region BL.

                var domainResponse = await this.ConfigHandler.Get(requestDMN.Content, requestDMN.ToRequestContext());

                #endregion
                #region response.

                return ServiceExecutionContext.PrepareResponse<ConfigSearchCriteriaDTO, SearchResults<ConfigItem>, SearchResultsDTO<ConfigDTO>>(requestDTO, responseDTO, domainResponse.State, domainResponse.Message, domainResponse.Result, ConfigSearchResultsMapper.Instance, serviceName);
                #endregion
            }
            #region Exception

            catch (Exception x)
            {
                XLogger.Error($"Exception : {x}");
                return ServiceExecutionContext.PrepareResponseError<ConfigSearchCriteriaDTO, SearchResultsDTO<ConfigDTO>>(request, serviceName);
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