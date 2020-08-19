using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using XCore.Framework.Infrastructure.Context.Services.Handlers;
using XCore.Framework.Infrastructure.Context.Services.Models;
using XCore.Framework.Infrastructure.Entities.Mappers;
using XCore.Framework.Infrastructure.Entities.Repositories.Models;
using XCore.Framework.Utilities;
using XCore.Services.Personnel.Core.Contracts.Settings;
using XCore.Services.Personnel.Models.Settings;
using XCore.Services.Personnel.API.Mappers.Settings;
using XCore.Services.Personnel.Models.DTO.Settings;
using XCore.Services.Personnel.Models.DTO.Support;
using XCore.Services.Personnel.Models.DTO.Essential.Settings;
using XCore.Services.Personnel.Models.Enums;
using XCore.Services.Personnel.Models.DTO.Support.Enum;

namespace XCore.Services.Personnel.API.Controllers
{
    [ApiVersion("0.1")]
    [ApiController]
    [Route("Api/v{version:apiVersion}/[controller]")]
    public class SettingController : ControllerBase
    {
        #region props.

        public bool? Initialized { get; protected set; }
        private readonly ISettingHandler SettingHandler;

        #endregion
        #region cst.

        public SettingController(ISettingHandler settingHandler)
        {
            this.SettingHandler = settingHandler;
            this.Initialized = Initialize();
        }

        #endregion
        #region actions.

        #region Setting : Create

        [HttpPost]
        [Route("Create")]
        public async Task<ServiceExecutionResponseDTO<SettingDTO>> SettingCreate(ServiceExecutionRequestDTO<SettingEssentialDTO> request)
        {
            string serviceName = "xcore.personnel.Setting.create";

            try
            {
                #region check.
                Check();
                #endregion
                #region request.

                // ...
                ServiceExecutionRequestDTO<SettingEssentialDTO> requestDTO = request;
                ServiceExecutionRequest<Setting> requestDMN;
                ServiceExecutionResponse<Setting> responseDMN;
                ServiceExecutionResponseDTO<SettingDTO> responseDTO;

                // ...
                responseDTO = ServiceExecutionContext.HandleRequestDTO<SettingEssentialDTO, Setting, SettingDTO, Setting>(requestDTO, SettingEssentialMapper<Setting,SettingEssentialDTO>.Instance, out requestDMN, out bool isValidRequest);
                if (!isValidRequest) return responseDTO;

                #endregion
                #region BL.

                var domainResponse = await SettingHandler.Create(requestDMN.Content, requestDMN.ToRequestContext());

                #endregion
                #region response.

                var res = ServiceExecutionContext.PrepareResponse<SettingEssentialDTO, Setting, SettingDTO>(requestDTO, responseDTO, domainResponse.State, domainResponse.Message, domainResponse.Result, SettingMapper.Instance, serviceName);
                return res;
                #endregion
            }
            #region Exception

            catch (Exception x)
            {
                XLogger.Error($"Exception : {x}");
                return ServiceExecutionContext.PrepareResponseError<SettingEssentialDTO, SettingDTO>(request, serviceName);
            }

            #endregion
        }

        #endregion
        #region Setting : Edit

        [HttpPost]
        [Route("Edit")]
        public async Task<ServiceExecutionResponseDTO<SettingDTO>> SettingEdit(ServiceExecutionRequestDTO<SettingEssentialDTO> request)
        {
            string serviceName = "xcore.personnel.Setting.edit";

            try
            {
                #region check.
                Check();
                #endregion
                #region request.

                // ...
                bool isValidRequest;
                ServiceExecutionRequestDTO<SettingEssentialDTO> requestDTO = request;
                ServiceExecutionRequest<Setting> requestDMN;
                ServiceExecutionResponse<Setting> responseDMN;
                ServiceExecutionResponseDTO<SettingDTO> responseDTO;

                // ...
                responseDTO = ServiceExecutionContext.HandleRequestDTO<SettingEssentialDTO, Setting, SettingDTO, Setting>(requestDTO, SettingEssentialMapper<Setting, SettingEssentialDTO>.Instance


                    , out requestDMN, out isValidRequest);
                if (!isValidRequest) return responseDTO;

                #endregion
                #region BL.

                var domainResponse = await SettingHandler.Edit(requestDMN.Content, requestDMN.ToRequestContext());

                #endregion
                #region response.

                return ServiceExecutionContext.PrepareResponse<SettingEssentialDTO, Setting, SettingDTO>(requestDTO, responseDTO, domainResponse.State, domainResponse.Message, domainResponse.Result, SettingMapper.Instance, serviceName);

                #endregion
            }
            #region Exception

            catch (Exception x)
            {
                XLogger.Error($"Exception : {x}");
                return ServiceExecutionContext.PrepareResponseError<SettingEssentialDTO, SettingDTO>(request, serviceName);
            }

            #endregion
        }

        #endregion
        #region Setting : Delete

        [HttpPost]
        [Route("Delete")]
        public async Task<ServiceExecutionResponseDTO<bool>> SettingDelete(ServiceExecutionRequestDTO<int> request)
        {
            string serviceName = "xcore.personnel.Setting.delete";

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

                var domainResponse = await SettingHandler.Delete(requestDMN.Content, requestDMN.ToRequestContext());

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
        #region Setting : Get

        [HttpPost("Get")]
        public async Task<ServiceExecutionResponseDTO<SearchResultsDTO<SettingDTO>>> SettingGet(ServiceExecutionRequestDTO<SettingSearchCriteriaDTO> request, SearchIncludesEnumDTO searchIncludes)
        {
            string serviceName = "xcore.personnel.Setting.get";

            try
            {
                #region check.
                Check();
                #endregion
                #region request.

                // ...
                ServiceExecutionRequestDTO<SettingSearchCriteriaDTO> requestDTO = request;
                ServiceExecutionRequest<SettingSearchCriteria> requestDMN;
                ServiceExecutionResponse<SearchResults<Setting>> responseDMN = new ServiceExecutionResponse<SearchResults<Setting>>();
                ServiceExecutionResponseDTO<SearchResultsDTO<SettingDTO>> responseDTO;

                // ...
                responseDTO = ServiceExecutionContext.HandleRequestDTO<SettingSearchCriteriaDTO, SettingSearchCriteria, SearchResultsDTO<SettingDTO>, SearchResults<Setting>>(requestDTO, SettingGetRequestMapper.Instance, out requestDMN, out bool isValidRequest);
                if (!isValidRequest) return responseDTO;

                #endregion
                #region BL.

                var domainResponse = await SettingHandler.Get(requestDMN.Content, requestDMN.ToRequestContext());

                #endregion
                #region response.

                return ServiceExecutionContext.PrepareResponse<SettingSearchCriteriaDTO, SearchResults<Setting>, SearchResultsDTO<SettingDTO>>(requestDTO, responseDMN, domainResponse.State, domainResponse.Message, domainResponse.Result, SettingGetResponseMapper.Instance, serviceName);

                #endregion
            }
            #region Exception

            catch (Exception x)
            {
                XLogger.Error($"Exception : {x}");
                return ServiceExecutionContext.PrepareResponseError<SettingSearchCriteriaDTO, SearchResultsDTO<SettingDTO>>(request, serviceName);
            }
            #endregion
        }

        #endregion

        #endregion
        #region helpers.

        private bool Initialize()
        {
            bool isValid = true;

            isValid = isValid && (SettingHandler?.Initialized ?? false);

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