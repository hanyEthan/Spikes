using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using XCore.Framework.Framework.Services.Rest;
using XCore.Framework.Infrastructure.Config.Contracts;
using XCore.Framework.Infrastructure.Context.Execution.Support;
using XCore.Framework.Infrastructure.Context.Services.Models;
using XCore.Services.Docs.SDK.Contracts;
using XCore.Services.Docs.SDK.Models;

namespace XCore.Services.Docs.SDK.Client
{
    public class DocumentClient : IDocumentClient
    {
        #region props.
        public bool Initialized { get; private set; }
        protected virtual IRestHandler<DocumentClientConfig> RestHandler { get; set; }
        protected virtual IConfigProvider<DocumentClientConfig> ConfigProvider { get; set; }

        #endregion
        #region cst.

        public DocumentClient(IRestHandler<DocumentClientConfig> restHandler) : this(restHandler, null)
        {
        }
        public DocumentClient(IRestHandler<DocumentClientConfig> restHandler, IConfigProvider<DocumentClientConfig> configProvider)
        {
            this.ConfigProvider = configProvider;
            this.RestHandler = restHandler;
            this.Initialized = Initialize();
        }

        #endregion
        #region IDocumentClient

        public async Task<RestResponse<ServiceExecutionResponseDTO<DocumentDTO>>> Create(ServiceExecutionRequestDTO<DocumentDTO> request)
        {
            #region validate.

            if (!this.Initialized)
            {
                throw new Exception("Client is not properly Initialized. please check the client config configurations.");
            }

            #endregion
            return await this.RestHandler.CallAsync<ServiceExecutionRequestDTO<DocumentDTO>, ServiceExecutionResponseDTO<DocumentDTO>>(HttpMethod.POST, request, "/api/v0.1/document/create/");
        }
        public async Task<RestResponse<ServiceExecutionResponseDTO<List<DocumentDTO>>>> Create(ServiceExecutionRequestDTO<List<DocumentDTO>> request)
        {
            #region validate.

            if (!this.Initialized)
            {
                throw new Exception("Client is not properly Initialized. please check the client config configurations.");
            }

            #endregion
            return await this.RestHandler.CallAsync<ServiceExecutionRequestDTO<List<DocumentDTO>>, ServiceExecutionResponseDTO<List<DocumentDTO>>>(HttpMethod.POST, request, "/api/v0.1/document/createList/");
        }
        public async Task<RestResponse<ServiceExecutionResponseDTO<SearchResultsDTO<DocumentDTO>>>> Get(ServiceExecutionRequestDTO<DocumentSearchCriteriaDTO> request)
        {
            #region validate.

            if (!this.Initialized)
            {
                throw new Exception("Client is not properly Initialized. please check the client config configurations.");
            }

            #endregion
            return await this.RestHandler.CallAsync<ServiceExecutionRequestDTO<DocumentSearchCriteriaDTO>, ServiceExecutionResponseDTO<SearchResultsDTO<DocumentDTO>>>(HttpMethod.POST, request, "/api/v0.1/document/get/");
        }
        public async Task<RestResponse<ServiceExecutionResponseDTO<DocumentDTO>>> Edit(ServiceExecutionRequestDTO<DocumentDTO> request)
        {
            #region validate.

            if (!this.Initialized)
            {
                throw new Exception("Client is not properly Initialized. please check the client config configurations.");
            }

            #endregion
            return await this.RestHandler.CallAsync<ServiceExecutionRequestDTO<DocumentDTO>, ServiceExecutionResponseDTO<DocumentDTO>>(HttpMethod.POST, request, "/api/v0.1/document/edit/");
        }
        public async Task<RestResponse<ServiceExecutionResponseDTO<bool>>> Delete(ServiceExecutionRequestDTO<int> request)
        {
            #region validate.

            if (!this.Initialized)
            {
                throw new Exception("Client is not properly Initialized. please check the client config configurations.");
            }

            #endregion
            return await this.RestHandler.CallAsync<ServiceExecutionRequestDTO<int>, ServiceExecutionResponseDTO<bool>>(HttpMethod.POST, request, "/api/v0.1/document/delete/");
        }
        public async Task<RestResponse<ServiceExecutionResponseDTO<bool>>> Delete(ServiceExecutionRequestDTO<List<int>> request)
        {
            #region validate.

            if (!this.Initialized)
            {
                throw new Exception("Client is not properly Initialized. please check the client config configurations.");
            }

            #endregion
            return await this.RestHandler.CallAsync<ServiceExecutionRequestDTO<List<int>>, ServiceExecutionResponseDTO<bool>>(HttpMethod.POST, request, "/api/v0.1/document/deleteList/");
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
