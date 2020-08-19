using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using XCore.Framework.Framework.Services.Rest;
using XCore.Framework.Infrastructure.Config.Contracts;
using XCore.Framework.Infrastructure.Context.Services.Models;
using XCore.Services.Attachments.SDK.Contracts;
using XCore.Services.Attachments.SDK.Models;

namespace XCore.Services.Attachments.SDK.Client
{
    public class AttachmentClient : IAttachmentClient
    {
        #region props.
        public bool Initialized { get; private set; }
        protected virtual IRestHandler<AttachmentClientConfig> RestHandler { get; set; }
        protected virtual IConfigProvider<AttachmentClientConfig> ConfigProvider { get; set; }

        #endregion

        #region cst.

        public AttachmentClient(IRestHandler<AttachmentClientConfig> restHandler) : this(restHandler,null)
        {
        }
       
        public AttachmentClient(IRestHandler<AttachmentClientConfig> restHandler, IConfigProvider<AttachmentClientConfig> configProvider)
        {
            this.ConfigProvider = configProvider;
            this.RestHandler = restHandler;
            this.Initialized = Initialize();
        }

        #endregion

        #region IAttachmentsClient
        public async Task<RestResponse<ServiceExecutionResponseDTO<SearchResultsDTO<AttachmentDTO>>>> Get(ServiceExecutionRequestDTO<AttachmentSearchCriteriaDTO> request)
        {
            #region validate.

            if (!this.Initialized)
            {
                throw new Exception("Client is not properly Initialized. please check the client config configurations.");
            }

            #endregion
            return await this.RestHandler.CallAsync<ServiceExecutionRequestDTO<AttachmentSearchCriteriaDTO>, ServiceExecutionResponseDTO<SearchResultsDTO<AttachmentDTO>>>(HttpMethod.POST, request, "/api/v0.1/attachment/get/");
        }
        public async Task<RestResponse<ServiceExecutionResponseDTO<AttachmentDTO>>> Create(ServiceExecutionRequestDTO<AttachmentDTO> request)
        {
            #region validate.

            if (!this.Initialized)
            {
                throw new Exception("Client is not properly Initialized. please check the client config configurations.");
            }

            #endregion
            return await this.RestHandler.CallAsync<ServiceExecutionRequestDTO<AttachmentDTO>, ServiceExecutionResponseDTO<AttachmentDTO>>(HttpMethod.POST, request, "/api/v0.1/attachment/create/");
        }
        public async Task<RestResponse<ServiceExecutionResponseDTO<bool>>> CreateConfirm(ServiceExecutionRequestDTO<List<AttachmentDTO>> request)
        {
            #region validate.

            if (!this.Initialized)
            {
                throw new Exception("Client is not properly Initialized. please check the client config configurations.");
            }

            #endregion
            return await this.RestHandler.CallAsync<ServiceExecutionRequestDTO<List<AttachmentDTO>>, ServiceExecutionResponseDTO<bool>>(HttpMethod.POST, request, "/api/v0.1/attachment/CreateConfirm/");
        }
        public async Task<RestResponse<ServiceExecutionResponseDTO<bool>>> Delete(ServiceExecutionRequestDTO<string> request)
        {
            #region validate.

            if (!this.Initialized)
            {
                throw new Exception("Client is not properly Initialized. please check the client config configurations.");
            }

            #endregion

            return await this.RestHandler.CallAsync<ServiceExecutionRequestDTO<string>, ServiceExecutionResponseDTO<bool>>(HttpMethod.POST, request , "/api/v0.1/attachment/delete/");
        }
        public async Task<RestResponse<ServiceExecutionResponseDTO<bool>>> DeleteListConfirm(ServiceExecutionRequestDTO<List<string>> request)
        {
            #region validate.

            if (!this.Initialized)
            {
                throw new Exception("Client is not properly Initialized. please check the client config configurations.");
            }

            #endregion

            return await this.RestHandler.CallAsync<ServiceExecutionRequestDTO<List<string>>, ServiceExecutionResponseDTO<bool>>(HttpMethod.POST, request, "/api/v0.1/attachment/DeleteListConfirm/");
        }
        public async Task<RestResponse<ServiceExecutionResponseDTO<bool>>> DeleteSoft(ServiceExecutionRequestDTO<string> request)
        {
            #region validate.

            if (!this.Initialized)
            {
                throw new Exception("Client is not properly Initialized. please check the client config configurations.");
            }

            #endregion

            return await this.RestHandler.CallAsync<ServiceExecutionRequestDTO<string>, ServiceExecutionResponseDTO<bool>>(HttpMethod.POST, request, "/api/v0.1/attachment/DeleteSoft/");
        }
        public async Task<RestResponse<ServiceExecutionResponseDTO<bool>>> ConfirmStatus (ServiceExecutionRequestDTO<AttachmentConfirmationAction> request)
        {
            #region validate.

            if (!this.Initialized)
            {
                throw new Exception("Client is not properly Initialized. please check the client config configurations.");
            }

            #endregion
            return await this.RestHandler.CallAsync<ServiceExecutionRequestDTO<AttachmentConfirmationAction>, ServiceExecutionResponseDTO<bool>>(HttpMethod.POST, request, "/api/v0.1/attachment/ConfirmStatus/");
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