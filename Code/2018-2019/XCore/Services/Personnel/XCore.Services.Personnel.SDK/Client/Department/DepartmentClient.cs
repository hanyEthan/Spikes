using System;
using System.Threading.Tasks;
using XCore.Framework.Framework.Services.Rest;
using XCore.Framework.Infrastructure.Config.Contracts;
using XCore.Framework.Infrastructure.Context.Services.Models;
using XCore.Services.Personnel.Models.DTO.Departments;
using XCore.Services.Personnel.SDK.Contracts;
using XCore.Services.Personnel.SDK.Models;
using XCore.Services.Personnel.Models.DTO.Support;
using XCore.Services.Personnel.Models.DTO.Essential.Departments;

namespace XCore.Services.Personnel.SDK.Client
{
    public class DepartmentClient : IDepartmentClient
    {
        #region props.
        public bool Initialized { get; private set; }
        protected virtual IRestHandler<PersonnelClientConfig> RestHandler { get; set; }
        protected virtual IConfigProvider<PersonnelClientConfig> ConfigProvider { get; set; }

        #endregion
        #region cst.

        public DepartmentClient(IRestHandler<PersonnelClientConfig> restHandler) : this(restHandler,null)
        {
        }
       
        public DepartmentClient(IRestHandler<PersonnelClientConfig> restHandler, IConfigProvider<PersonnelClientConfig> configProvider)
        {
            this.ConfigProvider = configProvider;
            this.RestHandler = restHandler;
            this.Initialized = Initialize();
        }

        #endregion

        #region IDepartmentClient

        public async Task<RestResponse<ServiceExecutionResponseDTO<DepartmentDTO>>> Create(ServiceExecutionRequestDTO<DepartmentEssentialDTO> request)
        {
            #region validate.

            if (!this.Initialized)
            {
                throw new Exception("Client is not properly Initialized. please check the client config configurations.");
            }

            #endregion
            return await this.RestHandler.CallAsync<ServiceExecutionRequestDTO<DepartmentEssentialDTO>, ServiceExecutionResponseDTO<DepartmentDTO>>(HttpMethod.POST, request, "/api/v0.1/Personnel/Department/create/");
        }
        public async Task<RestResponse<ServiceExecutionResponseDTO<SearchResultsDTO<DepartmentDTO>>>> Get(ServiceExecutionRequestDTO<DepartmentSearchCriteriaDTO> request)
        {
            #region validate.

            if (!this.Initialized)
            {
                throw new Exception("Client is not properly Initialized. please check the client config configurations.");
            }

            #endregion
            return await this.RestHandler.CallAsync<ServiceExecutionRequestDTO<DepartmentSearchCriteriaDTO>, ServiceExecutionResponseDTO<SearchResultsDTO<DepartmentDTO>>>(HttpMethod.POST, request, "/api/v0.1/Personnel/Department/get/");
        }
        public async Task<RestResponse<ServiceExecutionResponseDTO<bool>>> Delete(ServiceExecutionRequestDTO<DepartmentDTO> request)
        {
            #region validate.

            if (!this.Initialized)
            {
                throw new Exception("Client is not properly Initialized. please check the client config configurations.");
            }

            #endregion
            return await this.RestHandler.CallAsync<ServiceExecutionRequestDTO<DepartmentDTO>, ServiceExecutionResponseDTO<bool>>(HttpMethod.POST, request, "/api/v0.1/Personnel/Department/delete/");
        }
        public async Task<RestResponse<ServiceExecutionResponseDTO<DepartmentDTO>>> Edit(ServiceExecutionRequestDTO<DepartmentEssentialDTO> request)
        {
            #region validate.

            if (!this.Initialized)
            {
                throw new Exception("Client is not properly Initialized. please check the client config configurations.");
            }

            #endregion
            return await this.RestHandler.CallAsync<ServiceExecutionRequestDTO<DepartmentEssentialDTO>, ServiceExecutionResponseDTO<DepartmentDTO>>(HttpMethod.POST, request, "/api/v0.1/Personnel/Department/Edit");
        }
        #endregion
        #region helpers.
        private bool Initialize()
        {
            bool isValid = true;

            isValid = isValid && this.ConfigProvider != null;
            isValid = isValid && this.ConfigProvider.Initialized;

            isValid = isValid && this.RestHandler != null;
            isValid = isValid && this.RestHandler.Initialized;

            return isValid;
        }
        #endregion
    }
}
