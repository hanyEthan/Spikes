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
    public class ClaimController : ControllerBase
    {
        #region props.
        public bool? Initialized { get; protected set; }
        private readonly IClaimHandler ClaimHandler;
        #endregion
        #region cst.

        public ClaimController(IClaimHandler claimHandler)
        {
            ClaimHandler = claimHandler;
            this.Initialized = Initialize();
        }

        #endregion

        #region actions.

        #region Claim : Create

        [HttpPost]
        [Route("Create")]
        public async Task<ServiceExecutionResponseDTO<ClaimDTO>> ClaimCreate(ServiceExecutionRequestDTO<ClaimDTO> request)
        {
            string serviceName = "xcore.security.Claims.Register";

            try
            {
                #region check.

                Check();

                #endregion
                #region request.

                // ...
                ServiceExecutionRequestDTO<ClaimDTO> requestDTO = request;
                ServiceExecutionRequest<Claim> requestDMN;
                ServiceExecutionResponse<Claim> responseDMN;
                ServiceExecutionResponseDTO<ClaimDTO> responseDTO;

                // ...
                responseDTO = ServiceExecutionContext.HandleRequestDTO<ClaimDTO, Claim, ClaimDTO, Claim>(requestDTO, ClaimMapper.Instance, out requestDMN, out bool isValidRequest);
                if (!isValidRequest) return responseDTO;

                #endregion
                #region BL.

                var domainResponse = await ClaimHandler.Create(requestDMN.Content, requestDMN.ToRequestContext());

                #endregion
                #region response.

                var res = ServiceExecutionContext.PrepareResponse<ClaimDTO, Claim, ClaimDTO>(requestDTO, responseDTO, domainResponse.State, domainResponse.Message, domainResponse.Result, ClaimMapper.Instance, serviceName);
                return res;
                #endregion
            }
            #region Exception

            catch (Exception x)
            {
                XLogger.Error($"Exception : {x}");
                return ServiceExecutionContext.PrepareResponseError<ClaimDTO, ClaimDTO>(request, serviceName);
            }

            #endregion
        }

        #endregion
        #region Claim : Edit

        [HttpPost]
        [Route("Edit")]
        public async Task<ServiceExecutionResponseDTO<ClaimDTO>> ClaimEdit(ServiceExecutionRequestDTO<ClaimDTO> request)
        {
            string serviceName = "xcore.security.Claims.edit";

            try
            {
                #region check.

                Check();

                #endregion
                #region request.

                // ...
                bool isValidRequest;
                ServiceExecutionRequestDTO<ClaimDTO> requestDTO = request;
                ServiceExecutionRequest<Claim> requestDMN;
                ServiceExecutionResponse<Claim> responseDMN;
                ServiceExecutionResponseDTO<ClaimDTO> responseDTO;

                // ...
                responseDTO = ServiceExecutionContext.HandleRequestDTO<ClaimDTO, Claim, ClaimDTO, Claim>(requestDTO, ClaimMapper.Instance


                    , out requestDMN, out isValidRequest);
                if (!isValidRequest) return responseDTO;

                #endregion
                #region BL.

                var domainResponse = await ClaimHandler.Edit(requestDMN.Content, requestDMN.ToRequestContext());

                #endregion
                #region response.

                return ServiceExecutionContext.PrepareResponse<ClaimDTO, Claim, ClaimDTO>(requestDTO, responseDTO, domainResponse.State, domainResponse.Message, domainResponse.Result, ClaimMapper.Instance, serviceName);

                #endregion
            }
            #region Exception

            catch (Exception x)
            {
                XLogger.Error($"Exception : {x}");
                return ServiceExecutionContext.PrepareResponseError<ClaimDTO, ClaimDTO>(request, serviceName);
            }

            #endregion
        }

        #endregion
        #region Claim : Delete

        [HttpPost]
        [Route("Delete")]
        public async Task<ServiceExecutionResponseDTO<bool>> ClaimDelete(ServiceExecutionRequestDTO<int> request)
        {
            string serviceName = "xcore.security.Claims.unregister";

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

                var domainResponse = await ClaimHandler.DeleteClaim(requestDMN.Content, requestDMN.ToRequestContext());

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
        #region Claim : Get


        [HttpPost("Get")]
        public async Task<ServiceExecutionResponseDTO<SearchResultsDTO<ClaimDTO>>> ClaimGet(ServiceExecutionRequestDTO<ClaimSearchCriteriaDTO> request)
        {
            string serviceName = "xcore.security.Claims.get";

            try
            {
                #region check.

                Check();

                #endregion
                #region request.

                // ...
                ServiceExecutionRequestDTO<ClaimSearchCriteriaDTO> requestDTO = request;
                ServiceExecutionRequest<ClaimSearchCriteria> requestDMN;
                ServiceExecutionResponse<SearchResults<Claim>> responseDMN = new ServiceExecutionResponse<SearchResults<Claim>>();
                ServiceExecutionResponseDTO<SearchResultsDTO<ClaimDTO>> responseDTO;

                // ...
                responseDTO = ServiceExecutionContext.HandleRequestDTO<ClaimSearchCriteriaDTO, ClaimSearchCriteria, SearchResultsDTO<ClaimDTO>, SearchResults<Claim>>(requestDTO, ClaimSearchCriteriaMapper.Instance, out requestDMN, out bool isValidRequest);
                if (!isValidRequest) return responseDTO;

                #endregion
                #region BL.

                var domainResponse = await ClaimHandler.Get(requestDMN.Content, requestDMN.ToRequestContext());

                #endregion
                #region response.

                return ServiceExecutionContext.PrepareResponse<ClaimSearchCriteriaDTO, SearchResults<Claim>, SearchResultsDTO<ClaimDTO>>(requestDTO, responseDMN, domainResponse.State, domainResponse.Message, domainResponse.Result, ClaimSearchResultsMapper.Instance, serviceName);

                #endregion
            }
            #region Exception

            catch (Exception x)
            {
                XLogger.Error($"Exception : {x}");
                return ServiceExecutionContext.PrepareResponseError<ClaimSearchCriteriaDTO, SearchResultsDTO<ClaimDTO>>(request, serviceName);
            }
            #endregion
        }

        #endregion

        #endregion

        #region helpers.

        private bool Initialize()
        {
            bool isValid = true;

            isValid = isValid && (ClaimHandler?.Initialized ?? false);

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