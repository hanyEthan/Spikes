using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;
using XCore.Framework.Infrastructure.Context.Services.Handlers;
using XCore.Framework.Infrastructure.Context.Services.Models;
using XCore.Framework.Infrastructure.Entities.Repositories.Models;
using XCore.Framework.Utilities;
using XCore.Services.Audit.API.Mappers;
using XCore.Services.Audit.API.Models;
using XCore.Services.Audit.Core.Contracts;
using XCore.Services.Audit.Core.Models;

namespace XCore.Services.Audit.API.Controllers
{
    [ApiVersion("0.1")]
    [ApiController]
    [Route("Api/v{version:apiVersion}/[controller]")]
    public class AuditController : ControllerBase
    {
        #region props.

        public bool? Initialized { get; protected set; }
        private readonly IAuditHandler _AuditHandler;

        #endregion
        #region cst.

        public AuditController(IAuditHandler auditHandler)
        {
            this._AuditHandler = auditHandler;
            this.Initialized = Initialize();
        }

        #endregion
        #region actions.

        [HttpPost]
        [Route("Get")]
        public async Task<ServiceExecutionResponseDTO<SearchResultsDTO<AuditTrailDTO>>> Get([FromBody]ServiceExecutionRequestDTO<AuditSearchCriteriaDTO> request)
        {
            string serviceName = "xcore.audit.get";

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
                ServiceExecutionRequestDTO<AuditSearchCriteriaDTO> requestDTO = request;
                ServiceExecutionRequest<AuditSearchCriteria> requestDMN;
                ServiceExecutionResponse<SearchResults<AuditTrail>> responseDMN;
                ServiceExecutionResponseDTO<SearchResultsDTO<AuditTrailDTO>> responseDTO;

                // ...
                responseDTO = ServiceExecutionContext.HandleRequestDTO<AuditSearchCriteriaDTO, AuditSearchCriteria, SearchResultsDTO<AuditTrailDTO>, SearchResults<AuditTrail>>(requestDTO, AuditGetRequestMapper.Instance, out requestDMN, out isValidRequest, httpRequest: base.Request);
                if (!isValidRequest) return responseDTO;

                #endregion
                #region BL.

                var domainResponse = await this._AuditHandler.Get(requestDMN.Content, requestDMN.ToRequestContext());

                #endregion
                #region response.

                return ServiceExecutionContext.PrepareResponse<AuditSearchCriteriaDTO, SearchResults<AuditTrail>, SearchResultsDTO<AuditTrailDTO>>(requestDTO, responseDTO, domainResponse.State, domainResponse.Message, domainResponse.Result, AuditGetResponseMapper.Instance, serviceName);

                #endregion
            }
            #region Exception

            catch (Exception x)
            {
                XLogger.Error($"Exception : {x}");
                return ServiceExecutionContext.PrepareResponseError<AuditSearchCriteriaDTO, SearchResultsDTO<AuditTrailDTO>>(request, serviceName);
            }

            #endregion
        }

        [HttpPost]
        [Route("Create")]
        public async Task<ServiceExecutionResponseDTO<bool>> Create([FromBody]ServiceExecutionRequestDTO<AuditTrailDTO> request)
        {
            string serviceName = "xcore.audit.create";

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
                ServiceExecutionRequestDTO<AuditTrailDTO> requestDTO = request;
                ServiceExecutionRequest<AuditTrail> requestDMN;
                ServiceExecutionResponse<AuditTrail> responseDMN;
                ServiceExecutionResponseDTO<bool> responseDTO;

                // ...
                responseDTO = ServiceExecutionContext.HandleRequestDTO<AuditTrailDTO, AuditTrail, bool, AuditTrail>(requestDTO, AuditMapper.Instance, out requestDMN, out isValidRequest, httpRequest: base.Request);
                if (!isValidRequest) return responseDTO;

                #endregion
                #region BL.

                var domainResponse = await this._AuditHandler.Create(requestDMN.Content, requestDMN.ToRequestContext());

                #endregion
                #region response.

                return ServiceExecutionContext.PrepareResponse<AuditTrailDTO, AuditTrail, bool>(requestDTO, responseDTO, domainResponse.State, domainResponse.Message, domainResponse.Result, AuditCreateResponseMapper.Instance, serviceName);

                #endregion
            }
            #region Exception

            catch (Exception x)
            {
                XLogger.Error($"Exception : {x}");
                return ServiceExecutionContext.PrepareResponseError<AuditTrailDTO, bool>(request, serviceName);
            }

            #endregion
        }

        #endregion
        #region helpers.

        private bool Initialize()
        {
            bool isValid = true;

            isValid = isValid && ( this._AuditHandler?.Initialized ?? false );

            return isValid;
        }

        #endregion
    }
}
