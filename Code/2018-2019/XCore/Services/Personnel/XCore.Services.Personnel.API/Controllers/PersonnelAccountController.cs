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
    public class PersonnelAccountController : ControllerBase
    {
        #region props.

        public bool? Initialized { get; protected set; }
        private readonly IPersonnelAccountHandler PersonnelAccountHandler;

        #endregion
        #region cst.

        public PersonnelAccountController(IPersonnelAccountHandler acountHandler)
        {
            this.PersonnelAccountHandler = acountHandler;
            this.Initialized = Initialize();
        }

        #endregion
        #region actions.

        #region PersonnelAccount : Create

        [HttpPost]
        [Route("Create")]
        public async Task<ServiceExecutionResponseDTO<PersonnelAccountDTO>> PersonnelAccountCreate(ServiceExecutionRequestDTO<PersonnelAccountEssentialDTO> request)
        {
            string serviceName = "xcore.personnel.PersonnelAccount.create";

            try
            {
                #region check.
                Check();
                #endregion
                #region request.

                // ...
                ServiceExecutionRequestDTO<PersonnelAccountEssentialDTO> requestDTO = request;
                ServiceExecutionRequest<PersonnelAccount> requestDMN;
                ServiceExecutionResponse<PersonnelAccount> responseDMN;
                ServiceExecutionResponseDTO<PersonnelAccountDTO> responseDTO;

                // ...
                responseDTO = ServiceExecutionContext.HandleRequestDTO<PersonnelAccountEssentialDTO, PersonnelAccount, PersonnelAccountDTO, PersonnelAccount>(requestDTO, PersonnelAccountEssentialMapper<PersonnelAccount,PersonnelAccountEssentialDTO>.Instance, out requestDMN, out bool isValidRequest, httpRequest: base.Request);
                if (!isValidRequest) return responseDTO;

                #endregion
                #region BL.

                var domainResponse = await PersonnelAccountHandler.Create(requestDMN.Content, requestDMN.ToRequestContext());

                #endregion
                #region response.
                var res = ServiceExecutionContext.PrepareResponse<PersonnelAccountEssentialDTO, PersonnelAccount, PersonnelAccountDTO>(requestDTO, responseDTO, domainResponse.State, domainResponse.Message, domainResponse.Result, PersonnelAccountMapper.Instance, serviceName);
                return res;
                #endregion
            }
            #region Exception

            catch (Exception x)
            {
                XLogger.Error($"Exception : {x}");
                return ServiceExecutionContext.PrepareResponseError<PersonnelAccountEssentialDTO, PersonnelAccountDTO>(request, serviceName);
            }

            #endregion
        }
        #endregion
        #region PersonnelAccount : Edit

        [HttpPost]
        [Route("Edit")]
        public async Task<ServiceExecutionResponseDTO<PersonnelAccountDTO>> PersonnelAccountEdit(ServiceExecutionRequestDTO<PersonnelAccountEssentialDTO> request)
        {
            string serviceName = "xcore.personnel.PersonnelAccount.edit";

            try
            {
                #region check.
                Check();
                #endregion
                #region request.

                // ...
                bool isValidRequest;
                ServiceExecutionRequestDTO<PersonnelAccountEssentialDTO> requestDTO = request;
                ServiceExecutionRequest<PersonnelAccount> requestDMN;
                ServiceExecutionResponse<PersonnelAccount> responseDMN;
                ServiceExecutionResponseDTO<PersonnelAccountDTO> responseDTO;

                //TReqContentDTO, TReqContentDMN, TResContentDTO , TResContentDMN
                // ...
                responseDTO = ServiceExecutionContext.HandleRequestDTO<PersonnelAccountEssentialDTO, PersonnelAccount,
                              PersonnelAccountDTO, PersonnelAccount>(requestDTO, PersonnelAccountEssentialMapper<PersonnelAccount, PersonnelAccountEssentialDTO>.Instance, out requestDMN, out isValidRequest);
                if (!isValidRequest) return responseDTO;

                #endregion
                #region BL.

                var domainResponse = await PersonnelAccountHandler.Edit(requestDMN.Content, requestDMN.ToRequestContext());

                #endregion
                #region response.
                return ServiceExecutionContext.PrepareResponse<PersonnelAccountEssentialDTO, PersonnelAccount, PersonnelAccountDTO>(requestDTO, responseDTO, domainResponse.State, domainResponse.Message, domainResponse.Result, PersonnelAccountMapper.Instance, serviceName);

                #endregion
            }
            #region Exception

            catch (Exception x)
            {
                XLogger.Error($"Exception : {x}");
                return ServiceExecutionContext.PrepareResponseError<PersonnelAccountEssentialDTO, PersonnelAccountDTO>(request, serviceName);
            }

            #endregion
        }

        #endregion
        #region PersonnelAccount : Delete

        [HttpPost]
        [Route("Delete")]
        public async Task<ServiceExecutionResponseDTO<bool>> PersonnelAccountDelete(ServiceExecutionRequestDTO<int> request)
        {
            string serviceName = "xcore.personnel.PersonnelAccount.delete";

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

                var domainResponse = await PersonnelAccountHandler.Delete(requestDMN.Content, requestDMN.ToRequestContext());

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
        #region PersonnelAccount : Get

        [HttpPost("Get")]
        public async Task<ServiceExecutionResponseDTO<SearchResultsDTO<PersonnelAccountDTO>>> PersonnelAccountGet(ServiceExecutionRequestDTO<PersonnelAccountSearchCriteriaDTO> request, SearchIncludesEnumDTO searchIncludes)
        {
            string serviceName = "xcore.personnel.PersonnelAccount.get";

            try
            {
                #region check.
                Check();
                #endregion
                #region request.

                // ...
                ServiceExecutionRequestDTO<PersonnelAccountSearchCriteriaDTO> requestDTO = request;
                ServiceExecutionRequest<PersonnelAccountSearchCriteria> requestDMN;
                ServiceExecutionResponse<SearchResults<PersonnelAccount>> responseDMN = new ServiceExecutionResponse<SearchResults<PersonnelAccount>>();
                ServiceExecutionResponseDTO<SearchResultsDTO<PersonnelAccountDTO>> responseDTO;

                responseDTO = ServiceExecutionContext.HandleRequestDTO<PersonnelAccountSearchCriteriaDTO, PersonnelAccountSearchCriteria, SearchResultsDTO<PersonnelAccountDTO>, SearchResults<PersonnelAccount>>(requestDTO, PersonnelAccountGetRequestMapper.Instance, out requestDMN, out bool isValidRequest);
                if (!isValidRequest) return responseDTO;

                #endregion
                #region BL.

                var domainResponse = await PersonnelAccountHandler.Get(requestDMN.Content, requestDMN.ToRequestContext());

                #endregion
                #region response.

                return ServiceExecutionContext.PrepareResponse(requestDTO, responseDMN, domainResponse.State, domainResponse.Message, domainResponse.Result, PersonnelAccountGetResponseMapper.Instance, serviceName);

                #endregion
            }
            #region Exception

            catch (Exception x)
            {
                XLogger.Error($"Exception : {x}");
                return ServiceExecutionContext.PrepareResponseError<PersonnelAccountSearchCriteriaDTO, SearchResultsDTO<PersonnelAccountDTO>>(request, serviceName);
            }
            #endregion
        }

        #endregion

        #endregion
        #region helpers.

        private bool Initialize()
        {
            bool isValid = true;

            isValid = isValid && (PersonnelAccountHandler?.Initialized ?? false);

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