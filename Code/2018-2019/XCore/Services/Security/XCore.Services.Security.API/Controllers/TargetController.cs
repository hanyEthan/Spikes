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
using XCore.Services.Security.API.Model;
using XCore.Services.Security.API.MTargeters;
using XCore.Services.Security.Core.Contracts;
using XCore.Services.Security.Core.Models.Domain;
using XCore.Services.Security.Core.Models.Support;

namespace XCore.Services.Security.API.Controllers
{
    [ApiVersion("0.1")]
    [ApiController]
    [Route("Api/v{version:apiVersion}/[controller]")]
    public class TargetController : ControllerBase
    {
        #region props.

        public bool? Initialized { get; protected set; }
        private readonly ITargetHandler TargetHandler;

        #endregion
        #region cst.

        public TargetController(ITargetHandler targetHandler)
        {
            TargetHandler = targetHandler;
            this.Initialized = Initialize();
        }

        #endregion
        #region Actions
        #region Target: Get

        [HttpPost("Get")]
        public async Task<ServiceExecutionResponseDTO<SearchResultsDTO<TargetDTO>>> TargetGet(ServiceExecutionRequestDTO<TargetSearchCriteriaDTO> request)
        {
            string serviceName = "xcore.security.Targets.get";

            try
            {
                #region check.

                Check();

                #endregion
                #region request.

                // ...
                ServiceExecutionRequestDTO<TargetSearchCriteriaDTO> requestDTO = request;
                ServiceExecutionRequest<TargetSearchCriteria> requestDMN;
                ServiceExecutionResponse<SearchResults<Target>> responseDMN = new ServiceExecutionResponse<SearchResults<Target>>();
                ServiceExecutionResponseDTO<SearchResultsDTO<TargetDTO>> responseDTO;

                // ...
                responseDTO = ServiceExecutionContext.HandleRequestDTO<TargetSearchCriteriaDTO, TargetSearchCriteria, SearchResultsDTO<TargetDTO>, SearchResults<Target>>(requestDTO, TargetSearchCriteriaMapper.Instance, out requestDMN, out bool isValidRequest);
                if (!isValidRequest) return responseDTO;

                #endregion
                #region BL.

                var domainResponse = await TargetHandler.Get(requestDMN.Content, requestDMN.ToRequestContext());

                #endregion
                #region response.

                return ServiceExecutionContext.PrepareResponse<TargetSearchCriteriaDTO, SearchResults<Target>, SearchResultsDTO<TargetDTO>>(requestDTO, responseDMN, domainResponse.State, domainResponse.Message, domainResponse.Result, TargetSearchResultsMapper.Instance, serviceName);

                #endregion
            }
            #region Exception

            catch (Exception x)
            {
                XLogger.Error($"Exception : {x}");
                return ServiceExecutionContext.PrepareResponseError<TargetSearchCriteriaDTO, SearchResultsDTO<TargetDTO>>(request, serviceName);
            }
            #endregion
        }

        #endregion
        #endregion
        #region helpers.

        private bool Initialize()
        {
            bool isValid = true;

            isValid = isValid && (TargetHandler?.Initialized ?? false);

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