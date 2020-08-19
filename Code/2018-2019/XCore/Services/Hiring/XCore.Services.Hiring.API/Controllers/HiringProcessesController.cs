using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;
using XCore.Framework.Infrastructure.Context.Services.Handlers;
using XCore.Framework.Infrastructure.Context.Services.Models;
using XCore.Framework.Utilities;
using XCore.Services.Hiring.Core.Contracts;
using XCore.Services.Hiring.Core.Models.Domain;
using XCore.Services.Hiring.API.Models.DTO;
using XCore.Services.Hiring.API.Mappers.HiringProcesses;
using XCore.Services.Hiring.API.Models.Search;
using XCore.Services.Hiring.Core.Models.Search;
using XCore.Services.HiringProcesses.API.Mappers.HiringProcesses;
using XCore.Framework.Infrastructure.Entities.Repositories.Models;
using XCore.Framework.Infrastructure.Entities.Mappers;

namespace XCore.Services.Hiring.API.Controllers
{
    [ApiVersion("0.1")]
    [ApiController]
    [Route("Api/v{version:apiVersion}/[controller]")]
    public class HiringProcessesController : ControllerBase
    {
        #region props.

        public bool? Initialized { get; protected set; }
        private readonly IHiringProcessesHandler HiringProcessesHandler;

        #endregion
        #region cst.

        public HiringProcessesController(IHiringProcessesHandler HiringProcessesHandler)
        {
            this.HiringProcessesHandler = HiringProcessesHandler;
            this.Initialized = Initialize();
        }

        #endregion

        #region actions. 
        [HttpPost]
        [Route("Get")]
        public async Task<ServiceExecutionResponseDTO<SearchResultsDTO<HiringProcessDTO>>> Get(ServiceExecutionRequestDTO<HiringProcessesSearchCriteriaDTO> request)
        {
            string serviceName = "Xcore.Hiring.HiringProcess.Get";

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
                ServiceExecutionRequestDTO<HiringProcessesSearchCriteriaDTO> requestDTO = request;
                ServiceExecutionRequest<HiringProcessesSearchCriteria> requestDMN;
                ServiceExecutionResponse<SearchResults<HiringProcess>> responseDMN;
                ServiceExecutionResponseDTO<SearchResultsDTO<HiringProcessDTO>> responseDTO;

                // ...
                responseDTO = ServiceExecutionContext.HandleRequestDTO<HiringProcessesSearchCriteriaDTO, HiringProcessesSearchCriteria, SearchResultsDTO<HiringProcessDTO>, SearchResults<HiringProcess>>(requestDTO, HiringProcessGetRequestMapper.Instance, out requestDMN, out isValidRequest);
                if (!isValidRequest) return responseDTO;

                #endregion
                #region BL.

                var domainResponse = await this.HiringProcessesHandler.Get(requestDMN.Content, requestDMN.ToRequestContext());

                #endregion
                #region response.

                return ServiceExecutionContext.PrepareResponse<HiringProcessesSearchCriteriaDTO, SearchResults<HiringProcess>, SearchResultsDTO<HiringProcessDTO>>(requestDTO, responseDTO, domainResponse.State, domainResponse.Message, domainResponse.Result, HiringProcessGetResponseMapper.Instance, serviceName);

                #endregion
            }
            #region Exception

            catch (Exception x)
            {
                XLogger.Error($"Exception : {x}");
                return ServiceExecutionContext.PrepareResponseError<HiringProcessesSearchCriteriaDTO, SearchResultsDTO<HiringProcessDTO>>(request, serviceName);
            }

            #endregion
        }
        [HttpPost]
        [Route("Create")]
        public async Task<ServiceExecutionResponseDTO<HiringProcessDTO>> Create(ServiceExecutionRequestDTO<HiringProcessDTO> request)
        {
            string serviceName = "Xcore.Hiring.HiringProcesses.Create";

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
                ServiceExecutionRequestDTO<HiringProcessDTO> requestDTO = request;
                ServiceExecutionRequest<HiringProcess> requestDMN;
                ServiceExecutionResponse<HiringProcess> responseDMN;
                ServiceExecutionResponseDTO<HiringProcessDTO> responseDTO;

                // ...
                responseDTO = ServiceExecutionContext.HandleRequestDTO<HiringProcessDTO, HiringProcess, HiringProcessDTO, HiringProcess>(requestDTO, HiringProcessMapper.Instance, out requestDMN, out isValidRequest);
                if (!isValidRequest) return responseDTO;

                #endregion
                #region BL.

                var domainResponse = await this.HiringProcessesHandler.Create(requestDMN.Content, requestDMN.ToRequestContext());

                #endregion
                #region response.

                return ServiceExecutionContext.PrepareResponse<HiringProcessDTO, HiringProcess, HiringProcessDTO>(requestDTO, responseDTO, domainResponse.State, domainResponse.Message, domainResponse.Result, HiringProcessMapper.Instance, serviceName);

                #endregion
            }
            #region Exception

            catch (Exception x)
            {
                XLogger.Error($"Exception : {x}");
                return ServiceExecutionContext.PrepareResponseError<HiringProcessDTO, HiringProcessDTO>(request, serviceName);
            }

            #endregion
        }
        [HttpPost]
        [Route("Edit")]
        public async Task<ServiceExecutionResponseDTO<HiringProcessDTO>> Edit(ServiceExecutionRequestDTO<HiringProcessDTO> request)
        {
            string serviceName = "xcore.Hiring.HiringProcesses.Edit";

            try
            {
                #region request.

                // ...
                bool isValidRequest;
                ServiceExecutionRequestDTO<HiringProcessDTO> requestDTO = request;
                ServiceExecutionRequest<HiringProcess> requestDMN;
                ServiceExecutionResponse<HiringProcess> responseDMN;
                ServiceExecutionResponseDTO<HiringProcessDTO> responseDTO;

                // ...
                responseDTO = ServiceExecutionContext.HandleRequestDTO<HiringProcessDTO, HiringProcess, HiringProcessDTO, HiringProcess>(requestDTO, HiringProcessMapper.Instance, out requestDMN, out isValidRequest, httpRequest: base.Request);
                if (!isValidRequest) return responseDTO;

                #endregion
                #region BL.

                var domainResponse = await this.HiringProcessesHandler.Edit(requestDMN.Content, requestDMN.ToRequestContext());

                #endregion
                #region response.

                return ServiceExecutionContext.PrepareResponse<HiringProcessDTO, HiringProcess, HiringProcessDTO>(requestDTO, responseDTO, domainResponse.State, domainResponse.Message, domainResponse.Result, HiringProcessMapper.Instance, serviceName);

                #endregion
            }
            #region Exception

            catch (Exception x)
            {
                XLogger.Error($"Exception : {x}");
                return ServiceExecutionContext.PrepareResponseError<HiringProcessDTO, HiringProcessDTO>(request, serviceName);
            }

            #endregion
        }
        [HttpPost]
        [Route("ActivateById")]
        public async Task<ServiceExecutionResponseDTO<bool>> Activate(ServiceExecutionRequestDTO<int> request)
        {
            string serviceName = "Xcore.Hiring.HiringProcesses.Activate";

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

                var domainResponse = await HiringProcessesHandler.Activate(requestDMN.Content, requestDMN.ToRequestContext());

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
            string serviceName = "Xcore.Hiring.HiringProcesses.Activate";

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

                var domainResponse = await HiringProcessesHandler.Activate(requestDMN.Content, requestDMN.ToRequestContext());

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
            string serviceName = "Xcore.Hiring.HiringProcesses.Deactivate";

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

                var domainResponse = await HiringProcessesHandler.Deactivate(requestDMN.Content, requestDMN.ToRequestContext());

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
            string serviceName = "Xcore.Hiring.HiringProcesses.Deactivate";

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

                var domainResponse = await HiringProcessesHandler.Deactivate(requestDMN.Content, requestDMN.ToRequestContext());

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
            isValid = isValid && ( this.HiringProcessesHandler?.Initialized ?? false );
            return isValid;
        }

        #endregion
    }
}
