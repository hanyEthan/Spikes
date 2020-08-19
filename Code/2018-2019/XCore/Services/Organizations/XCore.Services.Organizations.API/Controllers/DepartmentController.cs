using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using XCore.Framework.Infrastructure.Context.Services.Handlers;
using XCore.Framework.Infrastructure.Context.Services.Models;
using XCore.Framework.Infrastructure.Entities.Mappers;
using XCore.Framework.Infrastructure.Entities.Repositories.Models;
using XCore.Framework.Utilities;
using XCore.Services.Organizations.API.Mappers;
using XCore.Services.Organizations.API.Models;
using XCore.Services.Organizations.API.Models.Department;
using XCore.Services.Organizations.Core.Contracts;
using XCore.Services.Organizations.Core.Models.Domain;
using XCore.Services.Organizations.Core.Models.Support;

namespace XCore.Services.Organizations.API.Controllers
{
    [ApiVersion("0.1")]
    [ApiController]
    [Route("Api/v{version:apiVersion}/[controller]")]
    public class DepartmentController : ControllerBase
    {
        #region props.
        public bool? Initialized { get; protected set; }
        private readonly IDepartmentHandler DepartmentHandler;
        #endregion
        #region cst.

        public DepartmentController(IDepartmentHandler organizationHandler)
        {
            DepartmentHandler = organizationHandler;
            this.Initialized = Initialize();
        }

        #endregion
        #region actions.
        #region Department : Create

        [HttpPost]
        [Route("Create")]
        public async Task<ServiceExecutionResponseDTO<DepartmentDTO>> CreateDepartment(ServiceExecutionRequestDTO<DepartmentDTO> request)
        {
            string serviceName = "xcore.org.Department.create";

            try
            {
                #region request.

                // ...
                ServiceExecutionRequestDTO<DepartmentDTO> requestDTO = request;
                ServiceExecutionRequest<Department> requestDMN;
                ServiceExecutionResponse<Department> responseDMN;
                ServiceExecutionResponseDTO<DepartmentDTO> responseDTO;

                // ...
                responseDTO = ServiceExecutionContext.HandleRequestDTO<DepartmentDTO, Department, DepartmentDTO, Department>(requestDTO, DepartmentMapper.Instance, out requestDMN, out bool isValidRequest);
                if (!isValidRequest) return responseDTO;

                #endregion
                #region BL.

                var domainResponse = await DepartmentHandler.Create(requestDMN.Content, requestDMN.ToRequestContext());

                #endregion
                #region response.

                var res = ServiceExecutionContext.PrepareResponse<DepartmentDTO, Department, DepartmentDTO>(requestDTO, responseDTO, domainResponse.State, domainResponse.Message, domainResponse.Result, DepartmentMapper.Instance, serviceName);
                return res;
                #endregion
            }
            #region Exception

            catch (Exception x)
            {
                XLogger.Error($"Exception : {x}");
                return ServiceExecutionContext.PrepareResponseError<DepartmentDTO, DepartmentDTO>(request, serviceName);
            }

            #endregion
        }

        #endregion
        #region Department : Edit

        [HttpPost]
        [Route("Edit")]
        public async Task<ServiceExecutionResponseDTO<DepartmentDTO>> EditDepartment(ServiceExecutionRequestDTO<DepartmentDTO> request)
        {
            string serviceName = "xcore.org.Department.edit";

            try
            {
                #region request.

                // ...
                bool isValidRequest;
                ServiceExecutionRequestDTO<DepartmentDTO> requestDTO = request;
                ServiceExecutionRequest<Department> requestDMN;
                ServiceExecutionResponse<Department> responseDMN;
                ServiceExecutionResponseDTO<DepartmentDTO> responseDTO;

                // ...
                responseDTO = ServiceExecutionContext.HandleRequestDTO<DepartmentDTO, Department, DepartmentDTO, Department>(requestDTO, DepartmentMapper.Instance


                    , out requestDMN, out isValidRequest);
                if (!isValidRequest) return responseDTO;

                #endregion
                #region BL.

                var domainResponse = await DepartmentHandler.Edit(requestDMN.Content, requestDMN.ToRequestContext());

                #endregion
                #region response.

                return ServiceExecutionContext.PrepareResponse<DepartmentDTO, Department, DepartmentDTO>(requestDTO, responseDTO, domainResponse.State, domainResponse.Message, domainResponse.Result, DepartmentMapper.Instance, serviceName);

                #endregion
            }
            #region Exception

            catch (Exception x)
            {
                XLogger.Error($"Exception : {x}");
                return ServiceExecutionContext.PrepareResponseError<DepartmentDTO, DepartmentDTO>(request, serviceName);
            }

            #endregion
        }

        #endregion
        #region Department : Get
        [HttpPost("Get")]
        public async Task<ServiceExecutionResponseDTO<SearchResultsDTO<DepartmentDTO>>> GetDepartment(ServiceExecutionRequestDTO<DepartmentSearchCriteriaDTO> request)
        {
            string serviceName = "xcore.org.Department.get";

            try
            {
                #region request.

                // ...
                ServiceExecutionRequestDTO<DepartmentSearchCriteriaDTO> requestDTO = request;
                ServiceExecutionRequest<DepartmentSearchCriteria> requestDMN;
                ServiceExecutionResponse<SearchResults<Department>> responseDMN = new ServiceExecutionResponse<SearchResults<Department>>();
                ServiceExecutionResponseDTO<SearchResultsDTO<DepartmentDTO>> responseDTO;

                // ...
                responseDTO = ServiceExecutionContext.HandleRequestDTO<DepartmentSearchCriteriaDTO, DepartmentSearchCriteria, SearchResultsDTO<DepartmentDTO>, SearchResults<Department>>(requestDTO, DepartmentSearchCriteriaMapper.Instance, out requestDMN, out bool isValidRequest);
                if (!isValidRequest) return responseDTO;

                #endregion
                #region BL.

                var domainResponse = await DepartmentHandler.Get(requestDMN.Content, requestDMN.ToRequestContext());

                #endregion
                #region response.

                return ServiceExecutionContext.PrepareResponse<DepartmentSearchCriteriaDTO, SearchResults<Department>, SearchResultsDTO<DepartmentDTO>>(requestDTO, responseDMN, domainResponse.State, domainResponse.Message, domainResponse.Result, DepartmentSearchResultsMapper.Instance, serviceName);

                #endregion
            }
            #region Exception

            catch (Exception x)
            {
                XLogger.Error($"Exception : {x}");
                return ServiceExecutionContext.PrepareResponseError<DepartmentSearchCriteriaDTO, SearchResultsDTO<DepartmentDTO>>(request, serviceName);
            }
            #endregion
        }

        #endregion
        #region Department : Activate
        [HttpPost]
        [Route("Activate")]
        public async Task<ServiceExecutionResponseDTO<bool>> ActivateDepartment(ServiceExecutionRequestDTO<string> request)
        {
            string serviceName = "xcore.org.Department.activate";

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

                var domainResponse = await DepartmentHandler.Activate(requestDMN.Content, requestDMN.ToRequestContext());

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
        #region Department : DeActivate

        [HttpPost]
        [Route("DeActivate")]
        public async Task<ServiceExecutionResponseDTO<bool>> DeActivateDepartment(ServiceExecutionRequestDTO<string> request)
        {
            string serviceName = "xcore.org.Department.deactivate";

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

                var domainResponse = await DepartmentHandler.Deactivate(requestDMN.Content, requestDMN.ToRequestContext());

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
        #region Department : Delete
        [HttpPost]
        [Route("Delete")]
        public async Task<ServiceExecutionResponseDTO<bool>> Delete(ServiceExecutionRequestDTO<int> request)
        {
            string serviceName = "xcore.org.Department.delete";

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

                var domainResponse = await DepartmentHandler.Delete(requestDMN.Content, requestDMN.ToRequestContext());

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




        #endregion
        #region helpers.

        private bool Initialize()
        {
            bool isValid = true;

            isValid = isValid && (DepartmentHandler?.Initialized ?? false);

            return isValid;
        }

        #endregion













    }
}