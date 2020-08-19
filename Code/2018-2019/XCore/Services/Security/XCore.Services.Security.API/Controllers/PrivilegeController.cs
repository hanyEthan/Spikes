using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using XCore.Framework.Infrastructure.Context.Services.Handlers;
using XCore.Framework.Infrastructure.Context.Services.Models;
using XCore.Framework.Infrastructure.Entities.Repositories.Models;
using XCore.Framework.Utilities;
using XCore.Services.Security.API.Mappers;
using XCore.Services.Security.API.Model;
using XCore.Services.Security.Core.Contracts;
using XCore.Services.Security.Core.Models.Domain;
using XCore.Services.Security.Core.Models.Support;

namespace XCore.Services.Security.API.Controllers
{
    [ApiVersion("0.1")]
    [ApiController]
    [Route("Api/v{version:apiVersion}/[controller]")]
    public class PrivilegeController : ControllerBase
    {
        #region props.

        public bool? Initialized { get; protected set; }
        private readonly IPrivilegeHandler PrivilegeHandler;

        #endregion
        #region cst.

        public PrivilegeController(IPrivilegeHandler privilegeHandler)
        {
            this.PrivilegeHandler = privilegeHandler;
            this.Initialized = Initialize();
        }

        #endregion
        #region Actions
        #region Privilege: Get

        [HttpPost("Get")]
        public async Task<ServiceExecutionResponseDTO<SearchResultsDTO<PrivilegeDTO>>> PrivilegeGet(ServiceExecutionRequestDTO<PrivilegeSearchCriteriaDTO> request)
        {
            string serviceName = "xcore.security.Privileges.get";

            try
            {
                #region check.

                Check();

                #endregion
                #region request.

                // ...
                ServiceExecutionRequestDTO<PrivilegeSearchCriteriaDTO> requestDTO = request;
                ServiceExecutionRequest<PrivilegeSearchCriteria> requestDMN;
                ServiceExecutionResponse<SearchResults<Privilege>> responseDMN = new ServiceExecutionResponse<SearchResults<Privilege>>();
                ServiceExecutionResponseDTO<SearchResultsDTO<PrivilegeDTO>> responseDTO;

                // ...
                responseDTO = ServiceExecutionContext.HandleRequestDTO<PrivilegeSearchCriteriaDTO, PrivilegeSearchCriteria, SearchResultsDTO<PrivilegeDTO>, SearchResults<Privilege>>(requestDTO, PrivilegeSearchCriteriaMapper.Instance, out requestDMN, out bool isValidRequest);
                if (!isValidRequest) return responseDTO;

                #endregion
                #region BL.

                var domainResponse = await PrivilegeHandler.Get(requestDMN.Content, requestDMN.ToRequestContext());

                #endregion
                #region response.

                return ServiceExecutionContext.PrepareResponse<PrivilegeSearchCriteriaDTO, SearchResults<Privilege>, SearchResultsDTO<PrivilegeDTO>>(requestDTO, responseDMN, domainResponse.State, domainResponse.Message, domainResponse.Result, PrivilegeSearchResultsMapper.Instance, serviceName);

                #endregion
            }
            #region Exception

            catch (Exception x)
            {
                XLogger.Error($"Exception : {x}");
                return ServiceExecutionContext.PrepareResponseError<PrivilegeSearchCriteriaDTO, SearchResultsDTO<PrivilegeDTO>>(request, serviceName);
            }
            #endregion
        }

        #endregion
        #endregion

        #region helpers.

        private bool Initialize()
        {
            bool isValid = true;

            isValid = isValid && (PrivilegeHandler?.Initialized ?? false);

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