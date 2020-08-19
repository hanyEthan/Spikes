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
    public class ModuleController : ControllerBase
    {
        #region props.

        public bool? Initialized { get; protected set; }
        private readonly IConfigHandler ConfigHandler;

        #endregion
        #region cst.

        public ModuleController(IConfigHandler configHandler)
        {
            ConfigHandler = configHandler;
            this.Initialized = Initialize();
        }

        #endregion
        #region actions.

        #region Module : Create

        [HttpPost]
        [Route("Create")]
        public async Task<ServiceExecutionResponseDTO<ModuleDTO>> ModuleCreate(ServiceExecutionRequestDTO<ModuleDTO> request)
        {
            string serviceName = "xcore.config.module.create";

            try
            {
                #region request.

                // ...
                ServiceExecutionRequestDTO<ModuleDTO> requestDTO = request;
                ServiceExecutionRequest<Module> requestDMN;
                ServiceExecutionResponse<Module> responseDMN;
                ServiceExecutionResponseDTO<ModuleDTO> responseDTO;

                // ...
                responseDTO = ServiceExecutionContext.HandleRequestDTO<ModuleDTO, Module, ModuleDTO, Module>(requestDTO, ModuleMapper.Instance, out requestDMN, out bool isValidRequest, httpRequest: base.Request);
                if (!isValidRequest) return responseDTO;

                #endregion
                #region BL.

                var domainResponse = await ConfigHandler.Create(requestDMN.Content, requestDMN.ToRequestContext());

                #endregion
                #region response.

                return ServiceExecutionContext.PrepareResponse<ModuleDTO, Module, ModuleDTO>(requestDTO, responseDTO, domainResponse.State, domainResponse.Message, domainResponse.Result, ModuleMapper.Instance, serviceName);

                #endregion
            }
            #region Exception

            catch (Exception x)
            {
                XLogger.Error($"Exception : {x}");
                return ServiceExecutionContext.PrepareResponseError<ModuleDTO, ModuleDTO>(request, serviceName);
            }

            #endregion
        }

        #endregion
        #region Module : Edit

        [HttpPost]
        [Route("Edit")]
        public async Task<ServiceExecutionResponseDTO<ModuleDTO>> ModuleEdit(ServiceExecutionRequestDTO<ModuleDTO> request)
        {
            string serviceName = "xcore.config.module.edit";

            try
            {
                #region request.

                // ...
                ServiceExecutionRequestDTO<ModuleDTO> requestDTO = request;
                ServiceExecutionRequest<Module> requestDMN;
                ServiceExecutionResponse<Module> responseDMN;
                ServiceExecutionResponseDTO<ModuleDTO> responseDTO;

                // ...
                responseDTO = ServiceExecutionContext.HandleRequestDTO<ModuleDTO, Module, ModuleDTO, Module>(requestDTO, ModuleMapper.Instance, out requestDMN, out bool isValidRequest, httpRequest: base.Request);
                if (!isValidRequest) return responseDTO;

                #endregion
                #region BL.

                var domainResponse = await ConfigHandler.Edit(requestDMN.Content, requestDMN.ToRequestContext());

                #endregion
                #region response.

                return ServiceExecutionContext.PrepareResponse<ModuleDTO, Module, ModuleDTO>(requestDTO, responseDTO, domainResponse.State, domainResponse.Message, domainResponse.Result, ModuleMapper.Instance, serviceName);

                #endregion
            }
            #region Exception

            catch (Exception x)
            {
                XLogger.Error($"Exception : {x}");
                return ServiceExecutionContext.PrepareResponseError<ModuleDTO, ModuleDTO>(request, serviceName);
            }

            #endregion
        }

        #endregion
        #region Module : Delete

        [HttpPost]
        [Route("Delete")]
        public async Task<ServiceExecutionResponseDTO<bool>> ModuleDelete(ServiceExecutionRequestDTO<string> request)
        {
            string serviceName = "xcore.config.module.delete";

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

                var domainResponse = await ConfigHandler.DeleteModule(requestDMN.Content, requestDMN.ToRequestContext());

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
        #region Module : Activate

        [HttpPost]
        [Route("Activate")]
        public async Task<ServiceExecutionResponseDTO<bool>> ActivateModule(ServiceExecutionRequestDTO<string> request)
        {
            string serviceName = "xcore.config.modules.activate";

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

                var domainResponse = await ConfigHandler.ActivateModule(requestDMN.Content, requestDMN.ToRequestContext());

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
        #region Module : DeActivate

        [HttpPost]
        [Route("Deactivate")]
        public async Task<ServiceExecutionResponseDTO<bool>> DeActivateModule(ServiceExecutionRequestDTO<string> request)
        {
            string serviceName = "xcore.config.modules.deactivate";

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

                var domainResponse = await ConfigHandler.DeactivateModule(requestDMN.Content, requestDMN.ToRequestContext());

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
        #region Module : Get

        [HttpPost]
        [Route("Get")]
        public async Task<ServiceExecutionResponseDTO<SearchResultsDTO<ModuleDTO>>> ModuleGet(ServiceExecutionRequestDTO<ModuleSearchCriteriaDTO> request)
        {
            string serviceName = "xcore.config.modules.get";

            try
            {
                #region request.

                // ...
                ServiceExecutionRequestDTO<ModuleSearchCriteriaDTO> requestDTO = request;
                ServiceExecutionRequest<ModuleSearchCriteria> requestDMN;
                ServiceExecutionResponse<SearchResults<Module>> responseDMN;
                ServiceExecutionResponseDTO<SearchResultsDTO<ModuleDTO>> responseDTO;

                // ...
                responseDTO = ServiceExecutionContext.HandleRequestDTO<ModuleSearchCriteriaDTO, ModuleSearchCriteria, SearchResultsDTO<ModuleDTO>, SearchResults<Module>>(requestDTO, ModuleSearchCriteriaMapper.Instance, out requestDMN, out bool isValidRequest, httpRequest: base.Request);
                if (!isValidRequest) return responseDTO;

                #endregion
                #region BL.

                var domainResponse = await ConfigHandler.Get(requestDMN.Content, requestDMN.ToRequestContext());

                #endregion
                #region response.

                return ServiceExecutionContext.PrepareResponse<ModuleSearchCriteriaDTO, SearchResults<Module>, SearchResultsDTO<ModuleDTO>>(requestDTO, responseDTO, domainResponse.State, domainResponse.Message, domainResponse.Result, ModuleSearchResultsMapper.Instance, serviceName);

                #endregion
            }
            #region Exception

            catch (Exception x)
            {
                XLogger.Error($"Exception : {x}");
                return ServiceExecutionContext.PrepareResponseError<ModuleSearchCriteriaDTO, SearchResultsDTO<ModuleDTO>>(request, serviceName);
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