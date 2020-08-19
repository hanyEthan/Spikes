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
using XCore.Services.Organizations.API.Models.Role;
using XCore.Services.Organizations.Core.Contracts;
using XCore.Services.Organizations.Core.Models.Domain;
using XCore.Services.Organizations.Core.Models.Support;

namespace XCore.Services.Organizations.API.Controllers
{
    [ApiVersion("0.1")]
    [ApiController]
    [Route("Api/v{version:apiVersion}/[controller]")]
    public class RoleController : ControllerBase
    {
        #region props.
        public bool? Initialized { get; protected set; }
        private readonly IRoleHandler RoletHandler;
        #endregion
        #region cst.

        public RoleController(IRoleHandler roleHandler)
        {
            RoletHandler = roleHandler;
            this.Initialized = Initialize();
        }

        #endregion
        #region actions.
        #region Role : Create

        [HttpPost]
        [Route("Create")]
        public async Task<ServiceExecutionResponseDTO<RoleDTO>> Create(ServiceExecutionRequestDTO<RoleDTO> request)
        {
            string serviceName = "xcore.org.Role.create";

            try
            {
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

                var domainResponse = await RoletHandler.Create(requestDMN.Content, requestDMN.ToRequestContext());

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
        public async Task<ServiceExecutionResponseDTO<RoleDTO>> Edit(ServiceExecutionRequestDTO<RoleDTO> request)
        {
            string serviceName = "xcore.org.Role.edit";

            try
            {
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

                var domainResponse = await RoletHandler.Edit(requestDMN.Content, requestDMN.ToRequestContext());

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
        #region Role : Get
        [HttpPost("Get")]
        public async Task<ServiceExecutionResponseDTO<SearchResultsDTO<RoleDTO>>> Get(ServiceExecutionRequestDTO<RoleSearchCriteriaDTO> request)
        {
            string serviceName = "xcore.org.Role.get";

            try
            {
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

                var domainResponse = await RoletHandler.Get(requestDMN.Content, requestDMN.ToRequestContext());

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
        [Route("Activate")]
        public async Task<ServiceExecutionResponseDTO<bool>> Activate(ServiceExecutionRequestDTO<string> request)
        {
            string serviceName = "xcore.org.Role.activate";

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

                var domainResponse = await RoletHandler.Activate(requestDMN.Content, requestDMN.ToRequestContext());

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
        #region Role : DeActivate

        [HttpPost]
        [Route("DeActivate")]
        public async Task<ServiceExecutionResponseDTO<bool>> DeActivate(ServiceExecutionRequestDTO<string> request)
        {
            string serviceName = "xcore.org.Role.deactivate";

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

                var domainResponse = await RoletHandler.Deactivate(requestDMN.Content, requestDMN.ToRequestContext());

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
        #region Role : Delete
        [HttpPost]
        [Route("Delete")]
        public async Task<ServiceExecutionResponseDTO<bool>> Delete(ServiceExecutionRequestDTO<string> request)
        {
            string serviceName = "xcore.org.Role.delete";

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

                var domainResponse = await RoletHandler.Delete(requestDMN.Content, requestDMN.ToRequestContext());

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

            isValid = isValid && (RoletHandler?.Initialized ?? false);

            return isValid;
        }

        #endregion













    }
}