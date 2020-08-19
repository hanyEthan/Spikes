using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using XCore.Framework.Infrastructure.Context.Services.Handlers;
using XCore.Framework.Infrastructure.Context.Services.Models;
using XCore.Framework.Infrastructure.Entities.Mappers;
using XCore.Framework.Infrastructure.Entities.Repositories.Models;
using XCore.Framework.Utilities;
using XCore.Services.Personnel.Models.DTO.Support;
using XCore.Services.Personnel.Core.Contracts.Accounts;
using XCore.Services.Personnel.Models.DTO.Accounts;
using XCore.Services.Personnel.Models.Accounts;
using XCore.Services.Personnel.API.Mappers.Accounts;
using XCore.Services.Personnel.Models.DTO.Essential.Accounts;
using XCore.Services.Personnel.Models.DTO.Support.Enum;
using XCore.Services.Personnel.Models.Enums;

namespace XCore.Services.Personnel.API.Controllers
{

    [ApiVersion("0.1")]
    [ApiController]
    [Route("Api/v{version:apiVersion}/[controller]")]
    public class OrganizationAccountController : ControllerBase
    {
        #region props.

        public bool? Initialized { get; protected set; }
        private readonly IOrganizationAccountHandler OrganizationAccountHandler;

        #endregion
        #region cst.

        public OrganizationAccountController(IOrganizationAccountHandler acountHandler)
        {
            this.OrganizationAccountHandler = acountHandler;
            this.Initialized = Initialize();
        }

        #endregion
        #region actions.

        #region OrganizationAccount : Create

        [HttpPost]
        [Route("Create")]
        public async Task<ServiceExecutionResponseDTO<OrganizationAccountDTO>> OrganizationAccountCreate(ServiceExecutionRequestDTO<OrganizationAccountEssentialDTO> request)
        {
            string serviceName = "xcore.personnel.OrganizationAccount.create";

            try
            {
                #region check.
                Check();
                #endregion
                #region request.

                // ...
                ServiceExecutionRequestDTO<OrganizationAccountEssentialDTO> requestDTO = request;
                ServiceExecutionRequest<OrganizationAccount> requestDMN;
                ServiceExecutionResponse<OrganizationAccount> responseDMN;
                ServiceExecutionResponseDTO<OrganizationAccountDTO> responseDTO;

                // ...
                responseDTO = ServiceExecutionContext.HandleRequestDTO<OrganizationAccountEssentialDTO, OrganizationAccount, OrganizationAccountDTO, OrganizationAccount>(requestDTO, OrganizationAccountEssentialMapper<OrganizationAccount, OrganizationAccountEssentialDTO>.Instance, out requestDMN, out bool isValidRequest, httpRequest: base.Request);
                if (!isValidRequest) return responseDTO;

                #endregion
                #region BL.

                var domainResponse = await OrganizationAccountHandler.Create(requestDMN.Content, requestDMN.ToRequestContext());

                #endregion
                #region response.
                var res = ServiceExecutionContext.PrepareResponse<OrganizationAccountEssentialDTO, OrganizationAccount, OrganizationAccountDTO>(requestDTO, responseDTO, domainResponse.State, domainResponse.Message, domainResponse.Result, OrganizationAccountMapper.Instance, serviceName);
                return res;
                #endregion
            }
            #region Exception

            catch (Exception x)
            {
                XLogger.Error($"Exception : {x}");
                return ServiceExecutionContext.PrepareResponseError<OrganizationAccountEssentialDTO, OrganizationAccountDTO>(request, serviceName);
            }

            #endregion
        }
        #endregion
        #region OrganizationAccount : Edit

        [HttpPost]
        [Route("Edit")]
        public async Task<ServiceExecutionResponseDTO<OrganizationAccountDTO>> OrganizationAccountEdit(ServiceExecutionRequestDTO<OrganizationAccountEssentialDTO> request)
        {
            string serviceName = "xcore.personnel.OrganizationAccount.edit";

            try
            {
                #region check.
                Check();
                #endregion
                #region request.

                // ...
                bool isValidRequest;
                ServiceExecutionRequestDTO<OrganizationAccountEssentialDTO> requestDTO = request;
                ServiceExecutionRequest<OrganizationAccount> requestDMN;
                ServiceExecutionResponse<OrganizationAccount> responseDMN;
                ServiceExecutionResponseDTO<OrganizationAccountDTO> responseDTO;

                //TReqContentDTO, TReqContentDMN, TResContentDTO , TResContentDMN
                // ...
                responseDTO = ServiceExecutionContext.HandleRequestDTO<OrganizationAccountEssentialDTO, OrganizationAccount,
                              OrganizationAccountDTO, OrganizationAccount>(requestDTO, OrganizationAccountEssentialMapper<OrganizationAccount, OrganizationAccountEssentialDTO>.Instance, out requestDMN, out isValidRequest);
                if (!isValidRequest) return responseDTO;

                #endregion
                #region BL.

                var domainResponse = await OrganizationAccountHandler.Edit(requestDMN.Content, requestDMN.ToRequestContext());

                #endregion
                #region response.
                return ServiceExecutionContext.PrepareResponse<OrganizationAccountEssentialDTO, OrganizationAccount, OrganizationAccountDTO>(requestDTO, responseDTO, domainResponse.State, domainResponse.Message, domainResponse.Result, OrganizationAccountMapper.Instance, serviceName);

                #endregion
            }
            #region Exception

            catch (Exception x)
            {
                XLogger.Error($"Exception : {x}");
                return ServiceExecutionContext.PrepareResponseError<OrganizationAccountEssentialDTO, OrganizationAccountDTO>(request, serviceName);
            }

            #endregion
        }

        #endregion
        #region OrganizationAccount : Delete

        [HttpPost]
        [Route("Delete")]
        public async Task<ServiceExecutionResponseDTO<bool>> OrganizationAccountDelete(ServiceExecutionRequestDTO<int> request)
        {
            string serviceName = "xcore.personnel.OrganizationAccount.delete";

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

                var domainResponse = await OrganizationAccountHandler.Delete(requestDMN.Content, requestDMN.ToRequestContext());

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
        #region OrganizationAccount : Get

        [HttpPost("Get")]
        public async Task<ServiceExecutionResponseDTO<SearchResultsDTO<OrganizationAccountDTO>>> OrganizationAccountGet(ServiceExecutionRequestDTO<OrganizationAccountSearchCriteriaDTO> request, SearchIncludesEnumDTO searchIncludes)
        {
            string serviceName = "xcore.personnel.OrganizationAccount.get";

            try
            {
                #region check.
                Check();
                #endregion
                #region request.

                // ...
                ServiceExecutionRequestDTO<OrganizationAccountSearchCriteriaDTO> requestDTO = request;
                ServiceExecutionRequest<OrganizationAccountSearchCriteria> requestDMN;
                ServiceExecutionResponse<SearchResults<OrganizationAccount>> responseDMN = new ServiceExecutionResponse<SearchResults<OrganizationAccount>>();
                ServiceExecutionResponseDTO<SearchResultsDTO<OrganizationAccountDTO>> responseDTO;

                responseDTO = ServiceExecutionContext.HandleRequestDTO<OrganizationAccountSearchCriteriaDTO, OrganizationAccountSearchCriteria, SearchResultsDTO<OrganizationAccountDTO>, SearchResults<OrganizationAccount>>(requestDTO, OrganizationAccountGetRequestMapper.Instance, out requestDMN, out bool isValidRequest);
                if (!isValidRequest) return responseDTO;

                #endregion
                #region BL.

                var domainResponse = await OrganizationAccountHandler.Get(requestDMN.Content, requestDMN.ToRequestContext());

                #endregion
                #region response.

                return ServiceExecutionContext.PrepareResponse(requestDTO, responseDMN, domainResponse.State, domainResponse.Message, domainResponse.Result, OrganizationAccountGetResponseMapper.Instance, serviceName);

                #endregion
            }
            #region Exception

            catch (Exception x)
            {
                XLogger.Error($"Exception : {x}");
                return ServiceExecutionContext.PrepareResponseError<OrganizationAccountSearchCriteriaDTO, SearchResultsDTO<OrganizationAccountDTO>>(request, serviceName);
            }
            #endregion
        }

        #endregion

        #endregion
        #region helpers.

        private bool Initialize()
        {
            bool isValid = true;

            isValid = isValid && (OrganizationAccountHandler?.Initialized ?? false);

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