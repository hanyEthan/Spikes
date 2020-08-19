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
using XCore.Services.Organizations.Core.Contracts;
using XCore.Services.Organizations.Core.Models.Domain;
using XCore.Services.Organizations.Core.Models.Support;

namespace XCore.Services.Organizations.API.Controllers
{
    [ApiVersion("0.1")]
    [ApiController]
    [Route("Api/v{version:apiVersion}/[controller]")]
    public class OrganizationController : ControllerBase
    {
        #region props.
        public bool? Initialized { get; protected set; }
        private readonly IOrganizationHandler OrganizationHandler;
        #endregion
        #region cst.

        public OrganizationController(IOrganizationHandler organizationHandler)
        {
            OrganizationHandler = organizationHandler;
            this.Initialized = Initialize();
        }

        #endregion
        #region actions.

        #region Organization : Create

        [HttpPost]
        [Route("Create")]
        public async Task<ServiceExecutionResponseDTO<OrganizationDTO>> OrganizationCreate(ServiceExecutionRequestDTO<OrganizationDTO> request)
        {
            string serviceName = "xcore.Org.organization.create";

            try
            {
                #region request.

                // ...
                ServiceExecutionRequestDTO<OrganizationDTO> requestDTO = request;
                ServiceExecutionRequest<Organization> requestDMN;
                ServiceExecutionResponse<Organization> responseDMN;
                ServiceExecutionResponseDTO<OrganizationDTO> responseDTO;

                // ...
                responseDTO = ServiceExecutionContext.HandleRequestDTO<OrganizationDTO, Organization, OrganizationDTO, Organization>(requestDTO, OrganizationMapper.Instance, out requestDMN, out bool isValidRequest);
                if (!isValidRequest) return responseDTO;

                #endregion
                #region BL.

                var domainResponse =await OrganizationHandler.Create(requestDMN.Content, requestDMN.ToRequestContext());

                #endregion
                #region response.

                var res = ServiceExecutionContext.PrepareResponse<OrganizationDTO, Organization, OrganizationDTO>(requestDTO, responseDTO, domainResponse.State, domainResponse.Message, domainResponse.Result, OrganizationMapper.Instance, serviceName);
                return res;
                #endregion
            }
            #region Exception

            catch (Exception x)
            {
                XLogger.Error($"Exception : {x}");
                return ServiceExecutionContext.PrepareResponseError<OrganizationDTO, OrganizationDTO>(request, serviceName);
            }

            #endregion
        }

        #endregion
        #region Organization : Edit

        [HttpPost]
        [Route("Edit")]
        public async Task<ServiceExecutionResponseDTO<OrganizationDTO>> OrganizationEdit(ServiceExecutionRequestDTO<OrganizationDTO> request)
        {
            string serviceName = "xcore.org.Organization.edit";

            try
            {
                #region request.

                // ...
                bool isValidRequest;
                ServiceExecutionRequestDTO<OrganizationDTO> requestDTO = request;
                ServiceExecutionRequest<Organization> requestDMN;
                ServiceExecutionResponse<Organization> responseDMN;
                ServiceExecutionResponseDTO<OrganizationDTO> responseDTO;

                // ...
                responseDTO = ServiceExecutionContext.HandleRequestDTO<OrganizationDTO, Organization, OrganizationDTO, Organization>(requestDTO, OrganizationMapper.Instance


                    , out requestDMN, out isValidRequest);
                if (!isValidRequest) return responseDTO;

                #endregion
                #region BL.

                var domainResponse = await OrganizationHandler.Edit(requestDMN.Content, requestDMN.ToRequestContext());

                #endregion
                #region response.

                return ServiceExecutionContext.PrepareResponse<OrganizationDTO, Organization, OrganizationDTO>(requestDTO, responseDTO, domainResponse.State, domainResponse.Message, domainResponse.Result, OrganizationMapper.Instance, serviceName);

                #endregion
            }
            #region Exception

            catch (Exception x)
            {
                XLogger.Error($"Exception : {x}");
                return ServiceExecutionContext.PrepareResponseError<OrganizationDTO, OrganizationDTO>(request, serviceName);
            }

            #endregion
        }

        #endregion
        #region Organization : Delete

        [HttpPost]
        [Route("Delete")]
        public async Task<ServiceExecutionResponseDTO<bool>> OrganizationDelete(ServiceExecutionRequestDTO<int> request)
        {
            string serviceName = "xcore.org.organization.delete";

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

                var domainResponse = await OrganizationHandler.Delete(requestDMN.Content, requestDMN.ToRequestContext());

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
        #region Organization : Get
        [HttpPost("Get")]
        public async Task<ServiceExecutionResponseDTO<SearchResultsDTO<OrganizationDTO>>> OrganizationGet(ServiceExecutionRequestDTO<OrganizationSearchCriteriaDTO> request)
        {
            string serviceName = "xcore.org.organization.get";

            try
            {
                #region request.

                // ...
                ServiceExecutionRequestDTO<OrganizationSearchCriteriaDTO> requestDTO = request;
                ServiceExecutionRequest<OrganizationSearchCriteria> requestDMN;
                ServiceExecutionResponse<SearchResults<Organization>> responseDMN = new ServiceExecutionResponse<SearchResults<Organization>>();
                ServiceExecutionResponseDTO<SearchResultsDTO<OrganizationDTO>> responseDTO;

                // ...
                responseDTO = ServiceExecutionContext.HandleRequestDTO<OrganizationSearchCriteriaDTO, OrganizationSearchCriteria, SearchResultsDTO<OrganizationDTO>, SearchResults<Organization>>(requestDTO, OrganizationSearchCriteriaMapper.Instance, out requestDMN, out bool isValidRequest);
                if (!isValidRequest) return responseDTO;

                #endregion
                #region BL.

                var domainResponse = await OrganizationHandler.Get(requestDMN.Content, requestDMN.ToRequestContext());

                #endregion
                #region response.

                return ServiceExecutionContext.PrepareResponse<OrganizationSearchCriteriaDTO, SearchResults<Organization>, SearchResultsDTO<OrganizationDTO>>(requestDTO, responseDMN, domainResponse.State, domainResponse.Message, domainResponse.Result, OrganizationSearchResultsMapper.Instance, serviceName);

                #endregion
            }
            #region Exception

            catch (Exception x)
            {
                XLogger.Error($"Exception : {x}");
                return ServiceExecutionContext.PrepareResponseError<OrganizationSearchCriteriaDTO, SearchResultsDTO<OrganizationDTO>>(request, serviceName);
            }
            #endregion
        }

        #endregion
        #region Organization : Activate
        [HttpPost]
        [Route("Activate")]
        public async Task<ServiceExecutionResponseDTO<bool>> ActivateOrganization(ServiceExecutionRequestDTO<string> request)
        {
            string serviceName = "xcore.org.organization.activate";

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

                var domainResponse = await OrganizationHandler.Activate(requestDMN.Content, requestDMN.ToRequestContext());

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
        #region Organization : DeActivate

        [HttpPost]
        [Route("DeActivate")]
        public async Task<ServiceExecutionResponseDTO<bool>> DeActivateOrganization(ServiceExecutionRequestDTO<string> request)
        {
            string serviceName = "xcore.org.organization.deactivate";

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

                var domainResponse = await OrganizationHandler.Deactivate(requestDMN.Content, requestDMN.ToRequestContext());

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

            isValid = isValid && (OrganizationHandler?.Initialized ?? false);

            return isValid;
        }

        #endregion
    }
}