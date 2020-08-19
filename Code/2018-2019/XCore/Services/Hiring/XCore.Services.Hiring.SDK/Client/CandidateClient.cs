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
    public class CandidateClient : ICandidateClient
    {
        #region props.
        public bool Initialized { get; private set; }
        protected virtual IRestHandler<HiringClientConfig> RestHandler { get; set; }
        protected virtual IConfigProvider<HiringClientConfig> ConfigProvider { get; set; }

        #endregion
        #region cst.

        public CandidateClient(IRestHandler<HiringClientConfig> restHandler) : this(restHandler,null)
        {
        }
       
        public CandidateClient(IRestHandler<HiringClientConfig> restHandler, IConfigProvider<HiringClientConfig> configProvider)
        {
            this.ConfigProvider = configProvider;
            this.RestHandler = restHandler;
            this.Initialized = Initialize();
        }

        #endregion

        #region ICandidateClient

        public async Task<RestResponse<ServiceExecutionResponseDTO<CandidateDTO>>> Create(ServiceExecutionRequestDTO<CandidateDTO> request)
        {
            #region validate.

            if (!this.Initialized)
            {
                throw new Exception("Client is not properly Initialized. please check the client config configurations.");
            }

            #endregion
            return await this.RestHandler.CallAsync<ServiceExecutionRequestDTO<CandidateDTO>, ServiceExecutionResponseDTO<CandidateDTO>>(HttpMethod.POST, request, "/api/v0.1/Candidates/Create/");
        }

        public async Task<RestResponse<ServiceExecutionResponseDTO<SearchResultsDTO<CandidateDTO>>>> Get(ServiceExecutionRequestDTO<CandidatesSearchCriteriaDTO> request)
        {
            #region validate.

            if (!this.Initialized)
            {
                throw new Exception("Client is not properly Initialized. please check the client config configurations.");
            }

            #endregion
            return await this.RestHandler.CallAsync<ServiceExecutionRequestDTO<CandidatesSearchCriteriaDTO>, ServiceExecutionResponseDTO<SearchResultsDTO<CandidateDTO>>>(HttpMethod.POST, request, "/api/v0.1/Candidates/Get/");

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
