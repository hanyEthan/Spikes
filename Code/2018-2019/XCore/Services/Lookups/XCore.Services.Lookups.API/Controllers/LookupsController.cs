using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using XCore.Framework.Infrastructure.Context.Services.Handlers;
using XCore.Framework.Infrastructure.Context.Services.Models;
using XCore.Framework.Infrastructure.Entities.Mappers;
using XCore.Framework.Utilities;
using XCore.Services.Lookups.API.Mappers;
using XCore.Services.Lookups.API.Models;
using XCore.Services.Lookups.Core.Contracts;
using XCore.Services.Lookups.Core.Models.Domain;

namespace XCore.Services.Lookups.API.Controllers
{
    [ApiVersion("0.1")]
    [ApiController]
    [Route("Api/v{version:apiVersion}/[controller]")]
    public class LookupsController : ControllerBase
    {
        #region props.

        public bool? Initialized { get; protected set; }
        private readonly ILookupsHandler _LookupsHandler;

        #endregion
        #region cst.

        public LookupsController(ILookupsHandler lookupsHandler)
        {
            this._LookupsHandler = lookupsHandler;
            this.Initialized = Initialize();
        }

        #endregion

        #region actions.

        #region LookupCategory : Create

        [HttpPost]
        [Route("Create")]
        public async Task<ServiceExecutionResponseDTO<LookupCategoryDTO>> LookupCategoryCreate(ServiceExecutionRequestDTO<LookupCategoryDTO> request)
        {
            string serviceName = "xcore.lookups.lookup.categories.create";

            try
            {
                #region request.

                // ...
                ServiceExecutionRequestDTO<LookupCategoryDTO> requestDTO = request;
                ServiceExecutionRequest<LookupCategory> requestDMN;
                ServiceExecutionResponse<LookupCategory> responseDMN;
                ServiceExecutionResponseDTO<LookupCategoryDTO> responseDTO;

                // ...
                responseDTO = ServiceExecutionContext.HandleRequestDTO<LookupCategoryDTO, LookupCategory, LookupCategoryDTO, LookupCategory>(requestDTO, LookupCategoryMapper.Instance, out requestDMN, out bool isValidRequest);
                if (!isValidRequest) return responseDTO;

                #endregion
                #region BL.

                var domainResponse = await _LookupsHandler.Create(requestDMN.Content, requestDMN.ToRequestContext());

                #endregion
                #region response.

                var res = ServiceExecutionContext.PrepareResponse<LookupCategoryDTO, LookupCategory, LookupCategoryDTO>(requestDTO, responseDTO, domainResponse.State, domainResponse.Message, domainResponse.Result, LookupCategoryMapper.Instance, serviceName);
                return res;
                #endregion
            }
            #region Exception

            catch (Exception x)
            {
                XLogger.Error($"Exception : {x}");
                return ServiceExecutionContext.PrepareResponseError<LookupCategoryDTO, LookupCategoryDTO>(request, serviceName);
            }

            #endregion
        }

        #endregion
        #region LookupCategory : Edit

        [HttpPost]
        [Route("Edit")]
        public async Task<ServiceExecutionResponseDTO<bool>> LookupCategoryEdit(ServiceExecutionRequestDTO<LookupCategoryDTO> request)
        {
            string serviceName = "xcore.lookups.lookup.categories.edit";

            try
            {
                #region request.

                // ...
                bool isValidRequest;
                ServiceExecutionRequestDTO<LookupCategoryDTO> requestDTO = request;
                ServiceExecutionRequest<LookupCategory> requestDMN;
                ServiceExecutionResponse<bool> responseDMN;
                ServiceExecutionResponseDTO<bool> responseDTO;

                // ...
                responseDTO = ServiceExecutionContext.HandleRequestDTO<LookupCategoryDTO, LookupCategory, bool, bool>(requestDTO, LookupCategoryMapper.Instance , out requestDMN, out isValidRequest);
                if (!isValidRequest) return responseDTO;

                #endregion
                #region BL.

                var domainResponse = await _LookupsHandler.Edit(requestDMN.Content, requestDMN.ToRequestContext());

                #endregion
                #region response.

                return ServiceExecutionContext.PrepareResponse<LookupCategoryDTO, bool, bool>(requestDTO, responseDTO, domainResponse.State, domainResponse.Message, domainResponse.Result, NativeMapper<bool>.Instance, serviceName);

                #endregion
            }
            #region Exception

            catch (Exception x)
            {
                XLogger.Error($"Exception : {x}");
                return ServiceExecutionContext.PrepareResponseError<LookupCategoryDTO, bool>(request, serviceName);
            }

            #endregion
        }

        #endregion

        #endregion

        #region helpers.

        private bool Initialize()
        {
            bool isValid = true;

            isValid = isValid && (_LookupsHandler?.Initialized ?? false);

            return isValid;
        }

        #endregion
    }
}