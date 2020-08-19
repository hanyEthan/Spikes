using System;
using System.Threading.Tasks;
using XCore.Framework.Framework.Services.Rest;
using XCore.Framework.Infrastructure.Config.Contracts;
using XCore.Framework.Infrastructure.Context.Services.Models;
using XCore.Services.Hiring.SDK.Contracts;
using XCore.Services.Hiring.SDK.Models;
using XCore.Services.Hiring.SDK.Models.DTO;
using XCore.Services.Hiring.SDK.Models.Search;

namespace XCore.Services.Hiring.SDK.Client
{
    public class HiringProcessClient : IHiringProcessClient
    {
        #region props.
        public bool Initialized { get; private set; }
        protected virtual IRestHandler<HiringClientConfig> RestHandler { get; set; }
        protected virtual IConfigProvider<HiringClientConfig> ConfigProvider { get; set; }

        #endregion
        #region cst.

        public HiringProcessClient(IRestHandler<HiringClientConfig> restHandler) : this(restHandler,null)
        {
        }
       
        public HiringProcessClient(IRestHandler<HiringClientConfig> restHandler, IConfigProvider<HiringClientConfig> configProvider)
        {
            this.ConfigProvider = configProvider;
            this.RestHandler = restHandler;
            this.Initialized = Initialize();
        }

        #endregion

        #region IHiringProcessClient

        public async Task<RestResponse<ServiceExecutionResponseDTO<HiringProcessDTO>>> Create(ServiceExecutionRequestDTO<HiringProcessDTO> request)
        {
            #region validate.

            if (!this.Initialized)
            {
                throw new Exception("Client is not properly Initialized. please check the client config configurations.");
            }

            #endregion
            return await this.RestHandler.CallAsync<ServiceExecutionRequestDTO<HiringProcessDTO>, ServiceExecutionResponseDTO<HiringProcessDTO>>(HttpMethod.POST, request, "/api/v0.1/HiringProcesss/Create/");
        }

        public async Task<RestResponse<ServiceExecutionResponseDTO<SearchResultsDTO<HiringProcessDTO>>>> Get(ServiceExecutionRequestDTO<HiringProcessesSearchCriteriaDTO> request)
        {
            #region validate.

            if (!this.Initialized)
            {
                throw new Exception("Client is not properly Initialized. please check the client config configurations.");
            }

            #endregion
            return await this.RestHandler.CallAsync<ServiceExecutionRequestDTO<HiringProcessesSearchCriteriaDTO>, ServiceExecutionResponseDTO<SearchResultsDTO<HiringProcessDTO>>>(HttpMethod.POST, request, "/api/v0.1/HiringProcesss/Get/");

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
