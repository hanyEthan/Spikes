using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;
using XCore.Framework.Infrastructure.Context.Services.Handlers;
using XCore.Framework.Infrastructure.Context.Services.Models;
using XCore.Framework.Utilities;
using XCore.Services.Hiring.Core.Contracts;
using XCore.Services.Hiring.Core.Models.Domain;
using XCore.Services.Hiring.API.Models.DTO;
using XCore.Services.Hiring.API.Mappers.Skills;
using XCore.Services.Hiring.API.Models.Search;
using XCore.Services.Hiring.Core.Models.Search;
using XCore.Services.Skills.API.Mappers.Skills;
using XCore.Framework.Infrastructure.Entities.Repositories.Models;
using XCore.Framework.Infrastructure.Entities.Mappers;

namespace XCore.Services.Hiring.API.Controllers
{
    [ApiVersion("0.1")]
    [ApiController]
    [Route("Api/v{version:apiVersion}/[controller]")]
    public class SkillsController : ControllerBase
    {
        #region props.

        public bool? Initialized { get; protected set; }
        private readonly ISkillsHandler SkillsHandler;

        #endregion
        #region cst.

        public SkillsController(ISkillsHandler SkillsHandler)
        {
            this.SkillsHandler = SkillsHandler;
            this.Initialized = Initialize();
        }

        #endregion

        #region actions. 
        [HttpPost]
        [Route("Get")]
        public async Task<ServiceExecutionResponseDTO<SearchResultsDTO<SkillDTO>>> Get(ServiceExecutionRequestDTO<SkillsSearchCriteriaDTO> request)
        {
            string serviceName = "Xcore.Hiring.Skill.Get";

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
                ServiceExecutionRequestDTO<SkillsSearchCriteriaDTO> requestDTO = request;
                ServiceExecutionRequest<SkillsSearchCriteria> requestDMN;
                ServiceExecutionResponse<SearchResults<Skill>> responseDMN;
                ServiceExecutionResponseDTO<SearchResultsDTO<SkillDTO>> responseDTO;

                // ...
                responseDTO = ServiceExecutionContext.HandleRequestDTO<SkillsSearchCriteriaDTO, SkillsSearchCriteria, SearchResultsDTO<SkillDTO>, SearchResults<Skill>>(requestDTO, SkillGetRequestMapper.Instance, out requestDMN, out isValidRequest);
                if (!isValidRequest) return responseDTO;

                #endregion
                #region BL.

                var domainResponse = await this.SkillsHandler.Get(requestDMN.Content, requestDMN.ToRequestContext());

                #endregion
                #region response.

                return ServiceExecutionContext.PrepareResponse<SkillsSearchCriteriaDTO, SearchResults<Skill>, SearchResultsDTO<SkillDTO>>(requestDTO, responseDTO, domainResponse.State, domainResponse.Message, domainResponse.Result, SkillGetResponseMapper.Instance, serviceName);

                #endregion
            }
            #region Exception

            catch (Exception x)
            {
                XLogger.Error($"Exception : {x}");
                return ServiceExecutionContext.PrepareResponseError<SkillsSearchCriteriaDTO, SearchResultsDTO<SkillDTO>>(request, serviceName);
            }

            #endregion
        }
        [HttpPost]
        [Route("Create")]
        public async Task<ServiceExecutionResponseDTO<SkillDTO>> Create(ServiceExecutionRequestDTO<SkillDTO> request)
        {
            string serviceName = "Xcore.Hiring.Skills.Create";

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
                ServiceExecutionRequestDTO<SkillDTO> requestDTO = request;
                ServiceExecutionRequest<Skill> requestDMN;
                ServiceExecutionResponse<Skill> responseDMN;
                ServiceExecutionResponseDTO<SkillDTO> responseDTO;

                // ...
                responseDTO = ServiceExecutionContext.HandleRequestDTO<SkillDTO, Skill, SkillDTO, Skill>(requestDTO, SkillMapper.Instance, out requestDMN, out isValidRequest);
                if (!isValidRequest) return responseDTO;

                #endregion
                #region BL.

                var domainResponse = await this.SkillsHandler.Create(requestDMN.Content, requestDMN.ToRequestContext());

                #endregion
                #region response.

                return ServiceExecutionContext.PrepareResponse<SkillDTO, Skill, SkillDTO>(requestDTO, responseDTO, domainResponse.State, domainResponse.Message, domainResponse.Result, SkillMapper.Instance, serviceName);

                #endregion
            }
            #region Exception

            catch (Exception x)
            {
                XLogger.Error($"Exception : {x}");
                return ServiceExecutionContext.PrepareResponseError<SkillDTO, SkillDTO>(request, serviceName);
            }

            #endregion
        }
        [HttpPost]
        [Route("Edit")]
        public async Task<ServiceExecutionResponseDTO<SkillDTO>> Edit(ServiceExecutionRequestDTO<SkillDTO> request)
        {
            string serviceName = "xcore.Hiring.Skills.Edit";

            try
            {
                #region request.

                // ...
                bool isValidRequest;
                ServiceExecutionRequestDTO<SkillDTO> requestDTO = request;
                ServiceExecutionRequest<Skill> requestDMN;
                ServiceExecutionResponse<Skill> responseDMN;
                ServiceExecutionResponseDTO<SkillDTO> responseDTO;

                // ...
                responseDTO = ServiceExecutionContext.HandleRequestDTO<SkillDTO, Skill, SkillDTO, Skill>(requestDTO, SkillMapper.Instance, out requestDMN, out isValidRequest, httpRequest: base.Request);
                if (!isValidRequest) return responseDTO;

                #endregion
                #region BL.

                var domainResponse = await this.SkillsHandler.Edit(requestDMN.Content, requestDMN.ToRequestContext());

                #endregion
                #region response.

                return ServiceExecutionContext.PrepareResponse<SkillDTO, Skill, SkillDTO>(requestDTO, responseDTO, domainResponse.State, domainResponse.Message, domainResponse.Result, SkillMapper.Instance, serviceName);

                #endregion
            }
            #region Exception

            catch (Exception x)
            {
                XLogger.Error($"Exception : {x}");
                return ServiceExecutionContext.PrepareResponseError<SkillDTO, SkillDTO>(request, serviceName);
            }

            #endregion
        }
        [HttpPost]
        [Route("ActivateById")]
        public async Task<ServiceExecutionResponseDTO<bool>> Activate(ServiceExecutionRequestDTO<int> request)
        {
            string serviceName = "Xcore.Hiring.Skills.Activate";

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

                var domainResponse = await SkillsHandler.Activate(requestDMN.Content, requestDMN.ToRequestContext());

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
            string serviceName = "Xcore.Hiring.Skills.Activate";

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

                var domainResponse = await SkillsHandler.Activate(requestDMN.Content, requestDMN.ToRequestContext());

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
            string serviceName = "Xcore.Hiring.Skills.Deactivate";

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

                var domainResponse = await SkillsHandler.Deactivate(requestDMN.Content, requestDMN.ToRequestContext());

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
            string serviceName = "Xcore.Hiring.Skills.Deactivate";

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

                var domainResponse = await SkillsHandler.Deactivate(requestDMN.Content, requestDMN.ToRequestContext());

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
            isValid = isValid && (this.SkillsHandler?.Initialized ?? false);
            return isValid;
        }

        #endregion
    }
}
