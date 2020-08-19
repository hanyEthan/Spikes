using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;
using XCore.Framework.Infrastructure.Context.Services.Handlers;
using XCore.Framework.Infrastructure.Context.Services.Models;
using XCore.Framework.Utilities;
using XCore.Services.Hiring.Core.Contracts;
using XCore.Services.Hiring.Core.Models.Domain;
using XCore.Services.Hiring.API.Models.DTO;
using XCore.Services.Hiring.API.Mappers.Applications;
using XCore.Services.Hiring.API.Models.Search;
using XCore.Services.Hiring.Core.Models.Search;
using XCore.Services.Applications.API.Mappers.Applications;
using XCore.Framework.Infrastructure.Entities.Repositories.Models;
using XCore.Framework.Infrastructure.Entities.Mappers;

namespace XCore.Services.Hiring.API.Controllers
{
    [ApiVersion("0.1")]
    [ApiController]
    [Route("Api/v{version:apiVersion}/[controller]")]
    public class ApplicationsController : ControllerBase
    {
        #region props.

        public bool? Initialized { get; protected set; }
        private readonly IApplicationsHandler ApplicationsHandler;

        #endregion
        #region cst.

        public ApplicationsController(IApplicationsHandler ApplicationsHandler)
        {
            this.ApplicationsHandler = ApplicationsHandler;
            this.Initialized = Initialize();
        }

        #endregion

        #region actions. 
        [HttpPost]
        [Route("Get")]
        public async Task<ServiceExecutionResponseDTO<SearchResultsDTO<ApplicationDTO>>> Get(ServiceExecutionRequestDTO<ApplicationsSearchCriteriaDTO> request)
        {
            string serviceName = "Xcore.Hiring.Application.Get";

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
                ServiceExecutionRequestDTO<ApplicationsSearchCriteriaDTO> requestDTO = request;
                ServiceExecutionRequest<ApplicationsSearchCriteria> requestDMN;
                ServiceExecutionResponse<SearchResults<Application>> responseDMN;
                ServiceExecutionResponseDTO<SearchResultsDTO<ApplicationDTO>> responseDTO;

                // ...
                responseDTO = ServiceExecutionContext.HandleRequestDTO<ApplicationsSearchCriteriaDTO, ApplicationsSearchCriteria, SearchResultsDTO<ApplicationDTO>, SearchResults<Application>>(requestDTO, ApplicationGetRequestMapper.Instance, out requestDMN, out isValidRequest);
                if (!isValidRequest) return responseDTO;

                #endregion
                #region BL.

                var domainResponse = await this.ApplicationsHandler.Get(requestDMN.Content, requestDMN.ToRequestContext());

                #endregion
                #region response.

                return ServiceExecutionContext.PrepareResponse<ApplicationsSearchCriteriaDTO, SearchResults<Application>, SearchResultsDTO<ApplicationDTO>>(requestDTO, responseDTO, domainResponse.State, domainResponse.Message, domainResponse.Result, ApplicationGetResponseMapper.Instance, serviceName);

                #endregion
            }
            #region Exception

            catch (Exception x)
            {
                XLogger.Error($"Exception : {x}");
                return ServiceExecutionContext.PrepareResponseError<ApplicationsSearchCriteriaDTO, SearchResultsDTO<ApplicationDTO>>(request, serviceName);
            }

            #endregion
        }
        [HttpPost]
        [Route("Create")]
        public async Task<ServiceExecutionResponseDTO<ApplicationDTO>> Create(ServiceExecutionRequestDTO<ApplicationDTO> request)
        {
            string serviceName = "Xcore.Hiring.Applications.Create";

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
                ServiceExecutionRequestDTO<ApplicationDTO> requestDTO = request;
                ServiceExecutionRequest<Application> requestDMN;
                ServiceExecutionResponse<Application> responseDMN;
                ServiceExecutionResponseDTO<ApplicationDTO> responseDTO;

                // ...
                responseDTO = ServiceExecutionContext.HandleRequestDTO<ApplicationDTO, Application, ApplicationDTO, Application>(requestDTO, ApplicationMapper.Instance, out requestDMN, out isValidRequest);
                if (!isValidRequest) return responseDTO;

                #endregion
                #region BL.

                var domainResponse = await this.ApplicationsHandler.Create(requestDMN.Content, requestDMN.ToRequestContext());

                #endregion
                #region response.

                return ServiceExecutionContext.PrepareResponse<ApplicationDTO, Application, ApplicationDTO>(requestDTO, responseDTO, domainResponse.State, domainResponse.Message, domainResponse.Result, ApplicationMapper.Instance, serviceName);

                #endregion
            }
            #region Exception

            catch (Exception x)
            {
                XLogger.Error($"Exception : {x}");
                return ServiceExecutionContext.PrepareResponseError<ApplicationDTO, ApplicationDTO>(request, serviceName);
            }

            #endregion
        }
        [HttpPost]
        [Route("Edit")]
        public async Task<ServiceExecutionResponseDTO<ApplicationDTO>> Edit(ServiceExecutionRequestDTO<ApplicationDTO> request)
        {
            string serviceName = "xcore.Hiring.Applications.Edit";

            try
            {
                #region request.

                // ...
                bool isValidRequest;
                ServiceExecutionRequestDTO<ApplicationDTO> requestDTO = request;
                ServiceExecutionRequest<Application> requestDMN;
                ServiceExecutionResponse<Application> responseDMN;
                ServiceExecutionResponseDTO<ApplicationDTO> responseDTO;

                // ...
                responseDTO = ServiceExecutionContext.HandleRequestDTO<ApplicationDTO, Application, ApplicationDTO, Application>(requestDTO, ApplicationMapper.Instance, out requestDMN, out isValidRequest, httpRequest: base.Request);
                if (!isValidRequest) return responseDTO;

                #endregion
                #region BL.

                var domainResponse = await this.ApplicationsHandler.Edit(requestDMN.Content, requestDMN.ToRequestContext());

                #endregion
                #region response.

                return ServiceExecutionContext.PrepareResponse<ApplicationDTO, Application, ApplicationDTO>(requestDTO, responseDTO, domainResponse.State, domainResponse.Message, domainResponse.Result, ApplicationMapper.Instance, serviceName);

                #endregion
            }
            #region Exception

            catch (Exception x)
            {
                XLogger.Error($"Exception : {x}");
                return ServiceExecutionContext.PrepareResponseError<ApplicationDTO, ApplicationDTO>(request, serviceName);
            }

            #endregion
        }
        [HttpPost]
        [Route("ActivateById")]
        public async Task<ServiceExecutionResponseDTO<bool>> Activate(ServiceExecutionRequestDTO<int> request)
        {
            string serviceName = "Xcore.Hiring.Applications.Activate";

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

                var domainResponse = await ApplicationsHandler.Activate(requestDMN.Content, requestDMN.ToRequestContext());

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
            string serviceName = "Xcore.Hiring.Applications.Activate";

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

                var domainResponse = await ApplicationsHandler.Activate(requestDMN.Content, requestDMN.ToRequestContext());

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
            string serviceName = "Xcore.Hiring.Applications.Deactivate";

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

                var domainResponse = await ApplicationsHandler.Deactivate(requestDMN.Content, requestDMN.ToRequestContext());

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
            string serviceName = "Xcore.Hiring.Applications.Deactivate";

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

                var domainResponse = await ApplicationsHandler.Deactivate(requestDMN.Content, requestDMN.ToRequestContext());

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
            isValid = isValid && ( this.ApplicationsHandler?.Initialized ?? false );
            return isValid;
        }

        #endregion
    }
}
