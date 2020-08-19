using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using XCore.Framework.Infrastructure.Context.Services.Handlers;
using XCore.Framework.Infrastructure.Context.Services.Models;
using XCore.Framework.Infrastructure.Entities.Mappers;
using XCore.Framework.Infrastructure.Entities.Repositories.Models;
using XCore.Framework.Utilities;
using XCore.Services.Personnel.Core.Contracts.Organizations;
using XCore.Services.Personnel.Models.Organizations;
using XCore.Services.Personnel.API.Mappers.Organizations;
using XCore.Services.Personnel.Models.DTO.Organizations;
using XCore.Services.Personnel.Models.DTO.Support;
using XCore.Services.Personnel.Models.DTO.Essential.Organizations;
using XCore.Services.Personnel.Models.DTO.Support.Enum;
using XCore.Services.Personnel.Models.Enums;

namespace XCore.Services.Personnel.API.Controllers
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
            this.OrganizationHandler = organizationHandler;
            this.Initialized = Initialize();
        }

        #endregion
        #region actions.

        #region Organization : Create

        [HttpPost]
        [Route("Create")]
        public async Task<ServiceExecutionResponseDTO<OrganizationDTO>> OrganizationCreate(ServiceExecutionRequestDTO<OrganizationEssentialDTO> request)
        {
            string serviceName = "xcore.personnel.Organization.create";

            try
            {
                #region check.
                Check();
                #endregion
                #region request.

                // ...
                ServiceExecutionRequestDTO<OrganizationEssentialDTO> requestDTO = request;
                ServiceExecutionRequest<Organization> requestDMN;
                ServiceExecutionResponse<Organization> responseDMN;
                ServiceExecutionResponseDTO<OrganizationDTO> responseDTO;

                // ...
                responseDTO = ServiceExecutionContext.HandleRequestDTO<OrganizationEssentialDTO, Organization, OrganizationDTO, Organization>(requestDTO, OrganizationEssentialMapper<Organization,OrganizationEssentialDTO>.Instance, out requestDMN, out bool isValidRequest);
                if (!isValidRequest) return responseDTO;

                #endregion
                #region BL.
                requestDMN = Map(requestDMN);
                var domainResponse = await OrganizationHandler.Create(requestDMN.Content, requestDMN.ToRequestContext());

                #endregion
                #region response.

                var res = ServiceExecutionContext.PrepareResponse<OrganizationEssentialDTO, Organization, OrganizationDTO>(requestDTO, responseDTO, domainResponse.State, domainResponse.Message, domainResponse.Result, OrganizationMapper.Instance, serviceName);
                return res;
                #endregion
            }
            #region Exception

            catch (Exception x)
            {
                XLogger.Error($"Exception : {x}");
                return ServiceExecutionContext.PrepareResponseError<OrganizationEssentialDTO, OrganizationDTO>(request, serviceName);
            }

            #endregion
        }

        #endregion
        #region Organization : Edit

        [HttpPost]
        [Route("Edit")]
        public async Task<ServiceExecutionResponseDTO<OrganizationDTO>> OrganizationEdit(ServiceExecutionRequestDTO<OrganizationEssentialDTO> request)
        {
            string serviceName = "xcore.personnel.Organization.edit";

            try
            {
                #region check.
                Check();
                #endregion
                #region request.

                // ...
                bool isValidRequest;
                ServiceExecutionRequestDTO<OrganizationEssentialDTO> requestDTO = request;
                ServiceExecutionRequest<Organization> requestDMN;
                ServiceExecutionResponse<Organization> responseDMN;
                ServiceExecutionResponseDTO<OrganizationDTO> responseDTO;

                // ...
                responseDTO = ServiceExecutionContext.HandleRequestDTO<OrganizationEssentialDTO, Organization, OrganizationDTO, Organization>(requestDTO, OrganizationEssentialMapper<Organization,OrganizationEssentialDTO>.Instance


                    , out requestDMN, out isValidRequest);
                if (!isValidRequest) return responseDTO;

                #endregion
                #region BL.
                requestDMN = Map(requestDMN);

                var domainResponse = await OrganizationHandler.Edit(requestDMN.Content, requestDMN.ToRequestContext());

                #endregion
                #region response.

                return ServiceExecutionContext.PrepareResponse<OrganizationEssentialDTO, Organization, OrganizationDTO>(requestDTO, responseDTO, domainResponse.State, domainResponse.Message, domainResponse.Result, OrganizationMapper.Instance, serviceName);

                #endregion
            }
            #region Exception

            catch (Exception x)
            {
                XLogger.Error($"Exception : {x}");
                return ServiceExecutionContext.PrepareResponseError<OrganizationEssentialDTO, OrganizationDTO>(request, serviceName);
            }

            #endregion
        }

        #endregion
        #region Organization : Delete

        [HttpPost]
        [Route("Delete")]
        public async Task<ServiceExecutionResponseDTO<bool>> OrganizationDelete(ServiceExecutionRequestDTO<int> request)
        {
            string serviceName = "xcore.personnel.Organization.delete";

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
        public async Task<ServiceExecutionResponseDTO<SearchResultsDTO<OrganizationDTO>>> OrganizationGet(ServiceExecutionRequestDTO<OrganizationSearchCriteriaDTO> request, SearchIncludesEnumDTO searchIncludes)
        {
            string serviceName = "xcore.personnel.Organization.get";

            try
            {
                #region check.
                Check();
                #endregion
                #region request.

                // ...
                ServiceExecutionRequestDTO<OrganizationSearchCriteriaDTO> requestDTO = request;
                ServiceExecutionRequest<OrganizationSearchCriteria> requestDMN;
                ServiceExecutionResponse<SearchResults<Organization>> responseDMN = new ServiceExecutionResponse<SearchResults<Organization>>();
                ServiceExecutionResponseDTO<SearchResultsDTO<OrganizationDTO>> responseDTO;

                // ...
                responseDTO = ServiceExecutionContext.HandleRequestDTO<OrganizationSearchCriteriaDTO, OrganizationSearchCriteria, SearchResultsDTO<OrganizationDTO>, SearchResults<Organization>>(requestDTO, OrganizationGetRequestMapper.Instance, out requestDMN, out bool isValidRequest);
                if (!isValidRequest) return responseDTO;

                #endregion
                #region BL.

                var domainResponse = await OrganizationHandler.Get(requestDMN.Content, requestDMN.ToRequestContext());

                #endregion
                #region response.

                return ServiceExecutionContext.PrepareResponse<OrganizationSearchCriteriaDTO, SearchResults<Organization>, SearchResultsDTO<OrganizationDTO>>(requestDTO, responseDMN, domainResponse.State, domainResponse.Message, domainResponse.Result, OrganizationGetResponseMapper.Instance, serviceName);

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

        #endregion
        #region helpers.

        private bool Initialize()
        {
            bool isValid = true;

            isValid = isValid && (OrganizationHandler?.Initialized ?? false);

            return isValid;
        }
        private void Check()
        {
            if (!this.Initialized.GetValueOrDefault())
            {
                throw new Exception("not initialized correctly.");
            }
        }

        private ServiceExecutionRequest<Organization> Map(ServiceExecutionRequest<Organization> requestDMN)
        {
            ServiceExecutionRequest<Organization> to = new ServiceExecutionRequest<Organization>();
            to.Content = new Organization();

            to.Content.AppId = requestDMN.ToRequestContext().AppId;
            to.Content.ModuleId = requestDMN.ToRequestContext().ModuleId;
            return to;
        }
        #endregion
    }
}