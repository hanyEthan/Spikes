using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using XCore.Framework.Infrastructure.Context.Services.Handlers;
using XCore.Framework.Infrastructure.Context.Services.Models;
using XCore.Framework.Infrastructure.Entities.Mappers;
using XCore.Framework.Infrastructure.Entities.Repositories.Models;
using XCore.Framework.Utilities;
using XCore.Services.Personnel.Core.Contracts.Departments;
using XCore.Services.Personnel.Models.Departments;
using XCore.Services.Personnel.API.Mappers.Departments;
using XCore.Services.Personnel.Models.DTO.Departments;
using XCore.Services.Personnel.Models.DTO.Support;
using XCore.Services.Personnel.Models.DTO.Essential.Departments;
using XCore.Services.Personnel.Models.DTO.Support.Enum;
using XCore.Services.Personnel.Models.Enums;

namespace XCore.Services.Personnel.API.Controllers
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

        public DepartmentController(IDepartmentHandler departmentHandler)
        {
            this.DepartmentHandler = departmentHandler;
            this.Initialized = Initialize();
        }

        #endregion
        #region actions.

        #region Department : Create

        [HttpPost]
        [Route("Create")]
        public async Task<ServiceExecutionResponseDTO<DepartmentDTO>> DepartmentCreate(ServiceExecutionRequestDTO<DepartmentEssentialDTO> request)
        {
            string serviceName = "xcore.personnel.department.create";

            try
            {
                #region check.

                Check();

                #endregion
                #region request.

                // ...
                ServiceExecutionRequestDTO<DepartmentEssentialDTO> requestDTO = request;
                ServiceExecutionRequest<Department> requestDMN;
                ServiceExecutionResponse<Department> responseDMN;
                ServiceExecutionResponseDTO<DepartmentDTO> responseDTO;

                // ...
                responseDTO = ServiceExecutionContext.HandleRequestDTO<DepartmentEssentialDTO, Department, DepartmentDTO, Department>(requestDTO, DepartmentEssentialMapper<Department, DepartmentEssentialDTO>.Instance, out requestDMN, out bool isValidRequest);
                if (!isValidRequest) return responseDTO;

                #endregion
                #region BL.
                requestDMN = Map(requestDMN);
                var domainResponse = await DepartmentHandler.Create(requestDMN.Content, requestDMN.ToRequestContext());

                #endregion
                #region response.

                var res = ServiceExecutionContext.PrepareResponse<DepartmentEssentialDTO, Department, DepartmentDTO>(requestDTO, responseDTO, domainResponse.State, domainResponse.Message, domainResponse.Result, DepartmentMapper.Instance, serviceName);
                return res;
                #endregion
            }
            #region Exception

            catch (Exception x)
            {
                XLogger.Error($"Exception : {x}");
                return ServiceExecutionContext.PrepareResponseError<DepartmentEssentialDTO, DepartmentDTO>(request, serviceName);
            }

            #endregion
        }

        #endregion
        #region Department : Edit

        [HttpPost]
        [Route("Edit")]
        public async Task<ServiceExecutionResponseDTO<DepartmentDTO>> DepartmentEdit(ServiceExecutionRequestDTO<DepartmentEssentialDTO> request)
        {
            string serviceName = "xcore.personnel.department.edit";

            try
            {
                #region check.

                Check();

                #endregion
                #region request.

                // ...
                bool isValidRequest;
                ServiceExecutionRequestDTO<DepartmentEssentialDTO> requestDTO = request;
                ServiceExecutionRequest<Department> requestDMN;
                ServiceExecutionResponse<Department> responseDMN;
                ServiceExecutionResponseDTO<DepartmentDTO> responseDTO;

                // ...
                responseDTO = ServiceExecutionContext.HandleRequestDTO<DepartmentEssentialDTO, Department, DepartmentDTO, Department>(requestDTO, DepartmentEssentialMapper<Department,DepartmentEssentialDTO>.Instance


                    , out requestDMN, out isValidRequest);
                if (!isValidRequest) return responseDTO;

                #endregion
                #region BL.
                requestDMN = Map(requestDMN);

                var domainResponse = await DepartmentHandler.Edit(requestDMN.Content, requestDMN.ToRequestContext());

                #endregion
                #region response.

                return ServiceExecutionContext.PrepareResponse<DepartmentEssentialDTO, Department, DepartmentDTO>(requestDTO, responseDTO, domainResponse.State, domainResponse.Message, domainResponse.Result, DepartmentMapper.Instance, serviceName);

                #endregion
            }
            #region Exception

            catch (Exception x)
            {
                XLogger.Error($"Exception : {x}");
                return ServiceExecutionContext.PrepareResponseError<DepartmentEssentialDTO, DepartmentDTO>(request, serviceName);
            }

            #endregion
        }

        #endregion
        #region Department : Delete

        [HttpPost]
        [Route("Delete")]
        public async Task<ServiceExecutionResponseDTO<bool>> DepartmentDelete(ServiceExecutionRequestDTO<int> request)
        {
            string serviceName = "xcore.personnel.department.delete";

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
        #region Department : Get
        [HttpPost("Get")]
        public async Task<ServiceExecutionResponseDTO<SearchResultsDTO<DepartmentDTO>>> DepartmentGet(ServiceExecutionRequestDTO<DepartmentSearchCriteriaDTO> request, SearchIncludesEnumDTO searchIncludes)
        {
            string serviceName = "xcore.personnel.department.get";

            try
            {
                #region check.

                Check();

                #endregion
                #region request.

                // ...
                if (!this.Initialized.GetValueOrDefault())
                {
                    throw new Exception("Service is not properly initialized.");
                }

                // ...
                bool isValidRequest;
                ServiceExecutionRequestDTO<DepartmentSearchCriteriaDTO> requestDTO = request;
                ServiceExecutionRequest<DepartmentSearchCriteria> requestDMN;
                ServiceExecutionResponse<SearchResults<Department>> responseDMN;
                ServiceExecutionResponseDTO<SearchResultsDTO<DepartmentDTO>> responseDTO;

                // ...
                responseDTO = ServiceExecutionContext.HandleRequestDTO<DepartmentSearchCriteriaDTO, DepartmentSearchCriteria, SearchResultsDTO<DepartmentDTO>, SearchResults<Department>>(requestDTO, DepartmentGetRequestMapper.Instance, out requestDMN, out isValidRequest, httpRequest: base.Request);
                if (!isValidRequest) return responseDTO;

                #endregion
                #region BL.

                var domainResponse = await this.DepartmentHandler.Get(requestDMN.Content, requestDMN.ToRequestContext());

                #endregion
                #region response.

                return ServiceExecutionContext.PrepareResponse<DepartmentSearchCriteriaDTO, SearchResults<Department>, SearchResultsDTO<DepartmentDTO>>(requestDTO, responseDTO, domainResponse.State, domainResponse.Message, domainResponse.Result, DepartmentGetResponseMapper.Instance, serviceName);

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

        #endregion
        #region helpers.

        private bool Initialize()
        {
            bool isValid = true;

            isValid = isValid && (DepartmentHandler?.Initialized ?? false);

            return isValid;
        }
        private void Check()
        {
            if (!this.Initialized.GetValueOrDefault())
            {
                throw new Exception("not initialized correctly.");
            }
        }

        private ServiceExecutionRequest<Department> Map(ServiceExecutionRequest<Department> requestDMN)
        {
            ServiceExecutionRequest<Department> to = new ServiceExecutionRequest<Department>();
            to.Content = new Department();

            to.Content.AppId = requestDMN.ToRequestContext().AppId;
            to.Content.ModuleId = requestDMN.ToRequestContext().ModuleId;
            return to;
        }
        #endregion
    }
}