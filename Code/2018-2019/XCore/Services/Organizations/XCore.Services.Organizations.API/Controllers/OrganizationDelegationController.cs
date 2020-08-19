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
using XCore.Services.Organizations.API.Models.Organization;
using XCore.Services.Organizations.API.Models.OrganizationDelegation;
using XCore.Services.Organizations.Core.Contracts;
using XCore.Services.Organizations.Core.Models.Domain;
using XCore.Services.Organizations.Core.Models.Support;

namespace XCore.Services.Organizations.API.Controllers
{
    [ApiVersion("0.1")]
    [ApiController]
    [Route("Api/v{version:apiVersion}/[controller]")]
    public class OrganizationDelegationController : ControllerBase
    {
        #region props.
        public bool? Initialized { get; protected set; }
        private readonly IOrganizationDelegationHandler OrganizationDelegationHandler;
        #endregion
        #region cst.

        public OrganizationDelegationController(IOrganizationDelegationHandler organizationDelegationHandler)
        {
            OrganizationDelegationHandler = organizationDelegationHandler;
            this.Initialized = Initialize();
        }

        #endregion
        #region actions.
        #region OrganizationDelegation : Create
        [HttpPost]
        [Route("Create")]
        public async Task<ServiceExecutionResponseDTO<OrganizationDelegationDTO>> CreateOrganizationDelegation(ServiceExecutionRequestDTO<OrganizationDelegationDTO> request)
        {
            string serviceName = "xcore.Org.organization.create";

            try
            {
                #region request.

                // ...
                ServiceExecutionRequestDTO<OrganizationDelegationDTO> requestDTO = request;
                ServiceExecutionRequest<OrganizationDelegation> requestDMN;
                ServiceExecutionResponse<OrganizationDelegation> responseDMN;
                ServiceExecutionResponseDTO<OrganizationDelegationDTO> responseDTO;

                // ...
                responseDTO = ServiceExecutionContext.HandleRequestDTO<OrganizationDelegationDTO, OrganizationDelegation, OrganizationDelegationDTO, OrganizationDelegation>(requestDTO, OrganizationDelegationMapper.Instance, out requestDMN, out bool isValidRequest);
                if (!isValidRequest) return responseDTO;

                #endregion
                #region BL.

                var domainResponse = await OrganizationDelegationHandler.Create(requestDMN.Content, requestDMN.ToRequestContext());

                #endregion
                #region response.

                var res = ServiceExecutionContext.PrepareResponse<OrganizationDelegationDTO, OrganizationDelegation, OrganizationDelegationDTO>(requestDTO, responseDTO, domainResponse.State, domainResponse.Message, domainResponse.Result, OrganizationDelegationMapper.Instance, serviceName);
                return res;
                #endregion
            }
            #region Exception

            catch (Exception x)
            {
                XLogger.Error($"Exception : {x}");
                return ServiceExecutionContext.PrepareResponseError<OrganizationDelegationDTO, OrganizationDelegationDTO>(request, serviceName);
            }

            #endregion
        }


        #endregion
        #region OrganizationDelegation : Edit

        [HttpPost]
        [Route("Edit")]
        public async Task<ServiceExecutionResponseDTO<OrganizationDelegationDTO>> EditOrganizationDelegation(ServiceExecutionRequestDTO<OrganizationDelegationDTO> request)
        {
            string serviceName = "xcore.org.OrganizationDelegation.edit";

            try
            {
                #region request.

                // ...
                ServiceExecutionRequestDTO<OrganizationDelegationDTO> requestDTO = request;
                ServiceExecutionRequest<OrganizationDelegation> requestDMN;
                ServiceExecutionResponse<OrganizationDelegation> responseDMN;
                ServiceExecutionResponseDTO<OrganizationDelegationDTO> responseDTO;

                // ...
                responseDTO = ServiceExecutionContext.HandleRequestDTO<OrganizationDelegationDTO, OrganizationDelegation, OrganizationDelegationDTO, OrganizationDelegation>(requestDTO, OrganizationDelegationMapper.Instance, out requestDMN, out bool isValidRequest);
                if (!isValidRequest) return responseDTO;

                #endregion
                #region BL.

                var domainResponse = await OrganizationDelegationHandler.Edit(requestDMN.Content, requestDMN.ToRequestContext());

                #endregion
                #region response.

                var res = ServiceExecutionContext.PrepareResponse<OrganizationDelegationDTO, OrganizationDelegation, OrganizationDelegationDTO>(requestDTO, responseDTO, domainResponse.State, domainResponse.Message, domainResponse.Result, OrganizationDelegationMapper.Instance, serviceName);
                return res;
                #endregion
            }
            #region Exception

            catch (Exception x)
            {
                XLogger.Error($"Exception : {x}");
                return ServiceExecutionContext.PrepareResponseError<OrganizationDelegationDTO, OrganizationDelegationDTO>(request, serviceName);
            }

            #endregion
        }

        #endregion
        #region OrganizationDelegation : Get
        [HttpPost("Get")]
        public async Task<ServiceExecutionResponseDTO<SearchResultsDTO<OrganizationDelegationDTO>>> GetOrganizationDelegation(ServiceExecutionRequestDTO<OrganizationDelegationSearchCriteriaDTO> request)
        {
            string serviceName = "xcore.org.OrganizationDelegation.get";

            try
            {
                #region request.

                // ...
                ServiceExecutionRequestDTO<OrganizationDelegationSearchCriteriaDTO> requestDTO = request;
                ServiceExecutionRequest<OrganizationDelegationSearchCriteria> requestDMN;
                ServiceExecutionResponse<SearchResults<OrganizationDelegation>> responseDMN = new ServiceExecutionResponse<SearchResults<OrganizationDelegation>>();
                ServiceExecutionResponseDTO<SearchResultsDTO<OrganizationDelegationDTO>> responseDTO;

                // ...
                responseDTO = ServiceExecutionContext.HandleRequestDTO<OrganizationDelegationSearchCriteriaDTO, OrganizationDelegationSearchCriteria, SearchResultsDTO<OrganizationDelegationDTO>, SearchResults<OrganizationDelegation>>(requestDTO, OrganizationDelegationSearchCriteriaMapper.Instance, out requestDMN, out bool isValidRequest);
                if (!isValidRequest) return responseDTO;

                #endregion
                #region BL.

                var domainResponse = await OrganizationDelegationHandler.Get(requestDMN.Content, requestDMN.ToRequestContext());

                #endregion
                #region response.

                return ServiceExecutionContext.PrepareResponse<OrganizationDelegationSearchCriteriaDTO, SearchResults<OrganizationDelegation>, SearchResultsDTO<OrganizationDelegationDTO>>(requestDTO, responseDMN, domainResponse.State, domainResponse.Message, domainResponse.Result, OrganizationDelegationSearchResultsMapper.Instance, serviceName);

                #endregion
            }
            #region Exception

            catch (Exception x)
            {
                XLogger.Error($"Exception : {x}");
                return ServiceExecutionContext.PrepareResponseError<OrganizationDelegationSearchCriteriaDTO, SearchResultsDTO<OrganizationDelegationDTO>>(request, serviceName);
            }
            #endregion
        }

        #endregion
        #region OrganizationDelegation : Delete
        [HttpPost]
        [Route("Delete")]
        public async Task<ServiceExecutionResponseDTO<bool>> Delete(ServiceExecutionRequestDTO<string> request)
        {
            string serviceName = "xcore.org.OrganizationDelegation.delete";

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

                var domainResponse = await OrganizationDelegationHandler.Delete(requestDMN.Content, requestDMN.ToRequestContext());

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
        #region OrganizationDelegation : Activate
        [HttpPost]
        [Route("Activate")]
        public async Task<ServiceExecutionResponseDTO<bool>> ActivateOrganizationDelegation(ServiceExecutionRequestDTO<string> request)
        {
            string serviceName = "xcore.org.OrganizationDelegation.activate";

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

                var domainResponse = await OrganizationDelegationHandler.Activate(requestDMN.Content, requestDMN.ToRequestContext());

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
        #region OrganizationDelegation : DeActivate

        [HttpPost]
        [Route("DeActivate")]
        public async Task<ServiceExecutionResponseDTO<bool>> DeActivateOrganizationDelegation(ServiceExecutionRequestDTO<string> request)
        {
            string serviceName = "xcore.org.OrganizationDelegation.deactivate";

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

                var domainResponse = await OrganizationDelegationHandler.Deactivate(requestDMN.Content, requestDMN.ToRequestContext());

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

            isValid = isValid && (OrganizationDelegationHandler?.Initialized ?? false);

            return isValid;
        }

        #endregion
    }
}