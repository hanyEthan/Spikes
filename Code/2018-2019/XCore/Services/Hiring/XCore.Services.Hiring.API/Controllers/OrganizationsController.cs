using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;
using XCore.Framework.Infrastructure.Context.Services.Handlers;
using XCore.Framework.Infrastructure.Context.Services.Models;
using XCore.Framework.Utilities;
using XCore.Services.Hiring.Core.Contracts;
using XCore.Services.Hiring.Core.Models.Domain;
using XCore.Services.Hiring.API.Models.DTO;
using XCore.Services.Hiring.API.Mappers.Organizations;
using XCore.Services.Hiring.API.Models.Search;
using XCore.Services.Hiring.Core.Models.Search;
using XCore.Services.Organizations.API.Mappers.Organizations;
using XCore.Framework.Infrastructure.Entities.Repositories.Models;
using XCore.Framework.Infrastructure.Entities.Mappers;

namespace XCore.Services.Hiring.API.Controllers
{
    [ApiVersion("0.1")]
    [ApiController]
    [Route("Api/v{version:apiVersion}/[controller]")]
    public class OrganizationsController : ControllerBase
    {
        #region props.

        public bool? Initialized { get; protected set; }
        private readonly IOrganizationsHandler OrganizationsHandler;

        #endregion
        #region cst.

        public OrganizationsController(IOrganizationsHandler OrganizationsHandler)
        {
            this.OrganizationsHandler = OrganizationsHandler;
            this.Initialized = Initialize();
        }

        #endregion

        #region actions. 
        [HttpPost]
        [Route("Get")]
        public async Task<ServiceExecutionResponseDTO<SearchResultsDTO<OrganizationDTO>>> Get(ServiceExecutionRequestDTO<OrganizationsSearchCriteriaDTO> request)
        {
            string serviceName = "Xcore.Hiring.Organization.Get";

            try
            {
                #region request.

                // ...
                if (!this.Initialized.GetValueOrDefault())
                {
                    throw new Exception("Service is not properly initialized.");
                }

                // ...
                bool isValidRequest;
                ServiceExecutionRequestDTO<OrganizationsSearchCriteriaDTO> requestDTO = request;
                ServiceExecutionRequest<OrganizationsSearchCriteria> requestDMN;
                ServiceExecutionResponse<SearchResults<Organization>> responseDMN;
                ServiceExecutionResponseDTO<SearchResultsDTO<OrganizationDTO>> responseDTO;

                // ...
                responseDTO = ServiceExecutionContext.HandleRequestDTO<OrganizationsSearchCriteriaDTO, OrganizationsSearchCriteria, SearchResultsDTO<OrganizationDTO>, SearchResults<Organization>>(requestDTO, OrganizationGetRequestMapper.Instance, out requestDMN, out isValidRequest);
                if (!isValidRequest) return responseDTO;

                #endregion
                #region BL.

                var domainResponse = await this.OrganizationsHandler.Get(requestDMN.Content, requestDMN.ToRequestContext());

                #endregion
                #region response.

                return ServiceExecutionContext.PrepareResponse<OrganizationsSearchCriteriaDTO, SearchResults<Organization>, SearchResultsDTO<OrganizationDTO>>(requestDTO, responseDTO, domainResponse.State, domainResponse.Message, domainResponse.Result, OrganizationGetResponseMapper.Instance, serviceName);

                #endregion
            }
            #region Exception

            catch (Exception x)
            {
                XLogger.Error($"Exception : {x}");
                return ServiceExecutionContext.PrepareResponseError<OrganizationsSearchCriteriaDTO, SearchResultsDTO<OrganizationDTO>>(request, serviceName);
            }

            #endregion
        }
        [HttpPost]
        [Route("Create")]
        public async Task<ServiceExecutionResponseDTO<OrganizationDTO>> Create(ServiceExecutionRequestDTO<OrganizationDTO> request)
        {
            string serviceName = "Xcore.Hiring.Organizations.Create";

            try
            {
                #region request.

                // ...
                if (!this.Initialized.GetValueOrDefault())
                {
                    throw new Exception("Service is not properly initialized.");
                }

                // ...
                bool isValidRequest;
                ServiceExecutionRequestDTO<OrganizationDTO> requestDTO = request;
                ServiceExecutionRequest<Organization> requestDMN;
                ServiceExecutionResponse<Organization> responseDMN;
                ServiceExecutionResponseDTO<OrganizationDTO> responseDTO;

                // ...
                responseDTO = ServiceExecutionContext.HandleRequestDTO<OrganizationDTO, Organization, OrganizationDTO, Organization>(requestDTO, OrganizationMapper.Instance, out requestDMN, out isValidRequest);
                if (!isValidRequest) return responseDTO;

                #endregion
                #region BL.

                var domainResponse = await this.OrganizationsHandler.Create(requestDMN.Content, requestDMN.ToRequestContext());

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
        [HttpPost]
        [Route("Edit")]
        public async Task<ServiceExecutionResponseDTO<OrganizationDTO>> Edit(ServiceExecutionRequestDTO<OrganizationDTO> request)
        {
            string serviceName = "xcore.Hiring.Organizations.Edit";

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
                responseDTO = ServiceExecutionContext.HandleRequestDTO<OrganizationDTO, Organization, OrganizationDTO, Organization>(requestDTO, OrganizationMapper.Instance, out requestDMN, out isValidRequest, httpRequest: base.Request);
                if (!isValidRequest) return responseDTO;

                #endregion
                #region BL.

                var domainResponse = await this.OrganizationsHandler.Edit(requestDMN.Content, requestDMN.ToRequestContext());

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
        [HttpPost]
        [Route("ActivateById")]
        public async Task<ServiceExecutionResponseDTO<bool>> Activate(ServiceExecutionRequestDTO<int> request)
        {
            string serviceName = "Xcore.Hiring.Organizations.Activate";

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

                var domainResponse = await OrganizationsHandler.Activate(requestDMN.Content, requestDMN.ToRequestContext());

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
        [HttpPost]
        [Route("ActivateByCode")]
        public async Task<ServiceExecutionResponseDTO<bool>> Activate(ServiceExecutionRequestDTO<string> request)
        {
            string serviceName = "Xcore.Hiring.Organizations.Activate";

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

                var domainResponse = await OrganizationsHandler.Activate(requestDMN.Content, requestDMN.ToRequestContext());

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
        [HttpPost]
        [Route("DeActivateById")]
        public async Task<ServiceExecutionResponseDTO<bool>> DeActivate(ServiceExecutionRequestDTO<int> request)
        {
            string serviceName = "Xcore.Hiring.Organizations.Deactivate";

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

                var domainResponse = await OrganizationsHandler.Deactivate(requestDMN.Content, requestDMN.ToRequestContext());

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
        [HttpPost]
        [Route("DeActivateByCode")]
        public async Task<ServiceExecutionResponseDTO<bool>> DeActivate(ServiceExecutionRequestDTO<string> request)
        {
            string serviceName = "Xcore.Hiring.Organizations.Deactivate";

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

                var domainResponse = await OrganizationsHandler.Deactivate(requestDMN.Content, requestDMN.ToRequestContext());

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
        #region helpers.

        private bool Initialize()
        {
            bool isValid = true;
            isValid = isValid && ( this.OrganizationsHandler?.Initialized ?? false );
            return isValid;
        }

        #endregion
    }
}
