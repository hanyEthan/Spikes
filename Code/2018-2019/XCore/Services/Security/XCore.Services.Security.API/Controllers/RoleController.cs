using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using XCore.Framework.Infrastructure.Context.Services.Handlers;
using XCore.Framework.Infrastructure.Context.Services.Models;
using XCore.Framework.Infrastructure.Entities.Mappers;
using XCore.Framework.Utilities;
using XCore.Services.Security.Core.Contracts;
using XCore.Services.Security.Core.Models.Domain;
using XCore.Services.Security.API.Model;
using XCore.Framework.Infrastructure.Entities.Repositories.Models;
using XCore.Services.Security.Core.Models.Support;
using XCore.Services.Security.API.Mappers;

namespace XCore.Services.Security.API.Controllers
{
    [ApiVersion("0.1")]
    [ApiController]
    [Route("Api/v{version:apiVersion}/[controller]")]
    public class RoleController : ControllerBase
    {
        #region props.
        public bool? Initialized { get; protected set; }
        private readonly IRoleHandler RoleHandler;
        #endregion
        #region cst.

        public RoleController(IRoleHandler roleHandler)
        {
            RoleHandler = roleHandler;
            this.Initialized = Initialize();
        }

        #endregion

        #region actions.

        #region Role : Create

        [HttpPost]
        [Route("Create")]
        public async Task<ServiceExecutionResponseDTO<RoleDTO>> RoleCreate(ServiceExecutionRequestDTO<RoleDTO> request)
        {
            string serviceName = "xcore.security.Roles.Register";

            try
            {
                #region check.

                Check();

                #endregion

                #region request.

                // ...
                ServiceExecutionRequestDTO<RoleDTO> requestDTO = request;
                ServiceExecutionRequest<Role> requestDMN;
                ServiceExecutionResponse<Role> responseDMN;
                ServiceExecutionResponseDTO<RoleDTO> responseDTO;

                // ...
                responseDTO = ServiceExecutionContext.HandleRequestDTO<RoleDTO, Role, RoleDTO, Role>(requestDTO, RoleMapper.Instance, out requestDMN, out bool isValidRequest);
                if (!isValidRequest) return responseDTO;

                #endregion
                #region BL.

                var domainResponse = await RoleHandler.Create(requestDMN.Content, requestDMN.ToRequestContext());

                #endregion
                #region response.

                var res = ServiceExecutionContext.PrepareResponse<RoleDTO, Role, RoleDTO>(requestDTO, responseDTO, domainResponse.State, domainResponse.Message, domainResponse.Result, RoleMapper.Instance, serviceName);
                return res;
                #endregion
            }
            #region Exception

            catch (Exception x)
            {
                XLogger.Error($"Exception : {x}");
                return ServiceExecutionContext.PrepareResponseError<RoleDTO, RoleDTO>(request, serviceName);
            }

            #endregion
        }

        #endregion
        #region Role : Edit

        [HttpPost]
        [Route("Edit")]
        public async Task<ServiceExecutionResponseDTO<RoleDTO>> RoleEdit(ServiceExecutionRequestDTO<RoleDTO> request)
        {
            string serviceName = "xcore.security.Roles.edit";

            try
            {
                #region check.

                Check();

                #endregion
                #region request.

                // ...
                bool isValidRequest;
                ServiceExecutionRequestDTO<RoleDTO> requestDTO = request;
                ServiceExecutionRequest<Role> requestDMN;
                ServiceExecutionResponse<Role> responseDMN;
                ServiceExecutionResponseDTO<RoleDTO> responseDTO;

                // ...
                responseDTO = ServiceExecutionContext.HandleRequestDTO<RoleDTO, Role, RoleDTO, Role>(requestDTO, RoleMapper.Instance


                    , out requestDMN, out isValidRequest);
                if (!isValidRequest) return responseDTO;

                #endregion
                #region BL.

                var domainResponse = await RoleHandler.Edit(requestDMN.Content, requestDMN.ToRequestContext());

                #endregion
                #region response.

                return ServiceExecutionContext.PrepareResponse<RoleDTO, Role, RoleDTO>(requestDTO, responseDTO, domainResponse.State, domainResponse.Message, domainResponse.Result, RoleMapper.Instance, serviceName);

                #endregion
            }
            #region Exception

            catch (Exception x)
            {
                XLogger.Error($"Exception : {x}");
                return ServiceExecutionContext.PrepareResponseError<RoleDTO, RoleDTO>(request, serviceName);
            }

            #endregion
        }

        #endregion
        #region Role : Delete

        [HttpPost]
        [Route("Delete")]
        public async Task<ServiceExecutionResponseDTO<bool>> RoleDelete(ServiceExecutionRequestDTO<int> request)
        {
            string serviceName = "xcore.security.Roles.unregister";

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

                var domainResponse = await RoleHandler.DeleteRole(requestDMN.Content, requestDMN.ToRequestContext());

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
        #region Role : Get


        [HttpPost("Get")]
        public async Task<ServiceExecutionResponseDTO<SearchResultsDTO<RoleDTO>>> RoleGet(ServiceExecutionRequestDTO<RoleSearchCriteriaDTO> request)
        {
            string serviceName = "xcore.security.Roles.get";

            try
            {
                #region check.

                Check();

                #endregion
                #region request.

                // ...
                ServiceExecutionRequestDTO<RoleSearchCriteriaDTO> requestDTO = request;
                ServiceExecutionRequest<RoleSearchCriteria> requestDMN;
                ServiceExecutionResponse<SearchResults<Role>> responseDMN = new ServiceExecutionResponse<SearchResults<Role>>();
                ServiceExecutionResponseDTO<SearchResultsDTO<RoleDTO>> responseDTO;

                // ...
                responseDTO = ServiceExecutionContext.HandleRequestDTO<RoleSearchCriteriaDTO, RoleSearchCriteria, SearchResultsDTO<RoleDTO>, SearchResults<Role>>(requestDTO, RoleSearchCriteriaMapper.Instance, out requestDMN, out bool isValidRequest);
                if (!isValidRequest) return responseDTO;

                #endregion
                #region BL.

                var domainResponse = await RoleHandler.Get(requestDMN.Content, requestDMN.ToRequestContext());

                #endregion
                #region response.

                return ServiceExecutionContext.PrepareResponse<RoleSearchCriteriaDTO, SearchResults<Role>, SearchResultsDTO<RoleDTO>>(requestDTO, responseDMN, domainResponse.State, domainResponse.Message, domainResponse.Result, RoleSearchResultsMapper.Instance, serviceName);

                #endregion
            }
            #region Exception

            catch (Exception x)
            {
                XLogger.Error($"Exception : {x}");
                return ServiceExecutionContext.PrepareResponseError<RoleSearchCriteriaDTO, SearchResultsDTO<RoleDTO>>(request, serviceName);
            }
            #endregion
        }

        #endregion
        #region Role : Activate
        [HttpPost]
        [Route("ActivateRole")]
        public async Task<ServiceExecutionResponseDTO<bool>> ActivateRole(ServiceExecutionRequestDTO<int> request)
        {
            string serviceName = "xcore.config.Roles.activate";

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

                var domainResponse = await RoleHandler.ActivateRole(requestDMN.Content, requestDMN.ToRequestContext());

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
        #region Role : DeActivate

        [HttpPost]
        [Route("DeActivateRole")]
        public async Task<ServiceExecutionResponseDTO<bool>> DeActivateRole(ServiceExecutionRequestDTO<int> request)
        {
            string serviceName = "xcore.config.Roles.deactivate";

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

                var domainResponse = await RoleHandler.DeactivateRole(requestDMN.Content, requestDMN.ToRequestContext());

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

            isValid = isValid && (RoleHandler?.Initialized ?? false);

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