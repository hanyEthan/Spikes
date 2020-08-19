using System;
using System.Threading.Tasks;
using XCore.Framework.Framework.ServiceBus.Contracts;
using XCore.Framework.Framework.Services.Rest;
using XCore.Framework.Infrastructure.Config.Contracts;
using XCore.Framework.Infrastructure.Context.Services.Models;
using XCore.Services.Notifications.SDK.Contracts;
using XCore.Services.Notifications.SDK.Model;
using XCore.Services.Notifications.SDK.Models.Support;

namespace XCore.Services.Notifications.SDK.Client
{
    public  class NotificationsMessageTemplateClient :INotificationsMessageTemplateClient
    {
        #region prop
        public bool Initialized { get; private set; }
        protected virtual IRestHandler<NotificationsClientConfig> RestHandler { get; set; }
        protected virtual IConfigProvider<NotificationsClientConfig> ConfigProvider { get; set; }

        public IServiceBus ServiceBus { get; set; }
        #endregion
        #region cst 

        public NotificationsMessageTemplateClient(IRestHandler<NotificationsClientConfig> restHandler) : this(restHandler, null, null)
        {
        }
        public NotificationsMessageTemplateClient(IRestHandler<NotificationsClientConfig> restHandler, IConfigProvider<NotificationsClientConfig> configProvider) : this(restHandler, null, configProvider)
        {
        }
        public NotificationsMessageTemplateClient(IRestHandler<NotificationsClientConfig> restHandler, IServiceBus serviceBus, IConfigProvider<NotificationsClientConfig> configProvider)
        {
            this.ServiceBus = serviceBus;
            this.ConfigProvider = configProvider;
            this.RestHandler = restHandler;

            this.Initialized = Initialize();
        }


        #endregion
        #region actions

        public async Task<RestResponse<ServiceExecutionResponseDTO<MessageTemplateDTO>>> Create(ServiceExecutionRequestDTO<MessageTemplateDTO> request)
        {
            #region validate.

            if (!this.Initialized)
            {
                throw new Exception("Client is not properly Initialized. please check the client config configurations.");
            }

            #endregion

            return await this.RestHandler.CallAsync<ServiceExecutionRequestDTO<MessageTemplateDTO>, ServiceExecutionResponseDTO<MessageTemplateDTO>>(HttpMethod.POST, request, "/api/v0.1/MessageTemplate/Create");

        }

        public async Task<RestResponse<ServiceExecutionResponseDTO<MessageTemplateDTO>>> Edit(ServiceExecutionRequestDTO<MessageTemplateDTO> request)
        {
            #region validate.

            if (!this.Initialized)
            {
                throw new Exception("Client is not properly Initialized. please check the client config configurations.");
            }

            #endregion

            return await this.RestHandler.CallAsync<ServiceExecutionRequestDTO<MessageTemplateDTO>, ServiceExecutionResponseDTO<MessageTemplateDTO>>(HttpMethod.POST, request, "/api/v0.1/MessageTemplate/Edit");

        }

        public async Task<RestResponse<ServiceExecutionResponseDTO<bool>>> Delete(ServiceExecutionRequestDTO<int> request)
        {
            #region validate.

            if (!this.Initialized)
            {
                throw new Exception("Client is not properly Initialized. please check the client config configurations.");
            }

            #endregion

            return await this.RestHandler.CallAsync<ServiceExecutionRequestDTO<int>, ServiceExecutionResponseDTO<bool>>(HttpMethod.POST, request, "/api/v0.1/MessageTemplate/Delete");

        }

        public async Task<RestResponse<ServiceExecutionResponseDTO<SearchResultsDTO<MessageTemplateDTO>>>> Get(ServiceExecutionRequestDTO<MessageTemplateSearchCriteriaDTO> request)
        {
            #region validate.

            if (!this.Initialized)
            {
                throw new Exception("Client is not properly Initialized. please check the client config configurations.");
            }

            #endregion

            return await this.RestHandler.CallAsync<ServiceExecutionRequestDTO<MessageTemplateSearchCriteriaDTO>, ServiceExecutionResponseDTO<SearchResultsDTO<MessageTemplateDTO>>>(HttpMethod.POST, request, "/api/v0.1/MessageTemplate/Get");

        }

        public async Task<RestResponse<ServiceExecutionResponseDTO<bool>>> Activate(ServiceExecutionRequestDTO<int> request)
        {
            #region validate.

            if (!this.Initialized)
            {
                throw new Exception("Client is not properly Initialized. please check the client config configurations.");
            }

            #endregion

            return await this.RestHandler.CallAsync<ServiceExecutionRequestDTO<int>, ServiceExecutionResponseDTO<bool>>(HttpMethod.POST, request, "/api/v0.1/MessageTemplate/ActivateMessageTemplate");

        }

        public async Task<RestResponse<ServiceExecutionResponseDTO<bool>>> DeActivate(ServiceExecutionRequestDTO<int> request)
        {
            #region validate.

            if (!this.Initialized)
            {
                throw new Exception("Client is not properly Initialized. please check the client config configurations.");
            }

            #endregion

            return await this.RestHandler.CallAsync<ServiceExecutionRequestDTO<int>, ServiceExecutionResponseDTO<bool>>(HttpMethod.POST, request, "/api/v0.1/MessageTemplate/DeActivateMessageTemplate");

        }

        public async Task<RestResponse<ServiceExecutionResponseDTO<ResolveResponseDTO>>> Resolve(ServiceExecutionRequestDTO<ResolveRequestDTO> request)
        {
            #region validate.

            if (!this.Initialized)
            {
                throw new Exception("Client is not properly Initialized. please check the client config configurations.");
            }

            #endregion

            return await this.RestHandler.CallAsync<ServiceExecutionRequestDTO<ResolveRequestDTO>, ServiceExecutionResponseDTO<ResolveResponseDTO>>(HttpMethod.POST, request, "/api/v0.1/MessageTemplate/Resolve");

        }

        #endregion
        #region helpers.

        private bool Initialize()
        {
            bool isValid = true;

            isValid = isValid && this.ServiceBus != null;
            isValid = isValid && this.ServiceBus.Initialized;

            isValid = isValid && this.ConfigProvider != null;
            isValid = isValid && this.ConfigProvider.Initialized;

            isValid = isValid && this.RestHandler != null;
            isValid = isValid && this.RestHandler.Initialized;

            return isValid;
        }

     

        #endregion
    }
}
