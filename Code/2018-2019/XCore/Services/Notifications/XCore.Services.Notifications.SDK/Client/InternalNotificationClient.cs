using System;
using System.Threading.Tasks;
using XCore.Framework.Framework.ServiceBus.Contracts;
using XCore.Framework.Infrastructure.Config.Contracts;
using XCore.Framework.Infrastructure.Context.Execution.Support;
using XCore.Framework.Infrastructure.Context.Services.Models;
using XCore.Services.Notifications.SDK.Contracts;
using XCore.Services.Notifications.SDK.Model;
using XCore.Services.Notifications.SDK.Models.Support;
using XCore.Framework.Framework.Services.Rest;

using XCore.Services.Notifications.SDK.Models.DTOs;
using System.Collections.Generic;

namespace XCore.Services.Notifications.SDK.Client
{
    public class InternalNotificationClient : IInternalNotificationClient
    {
        #region prop
        public bool Initialized { get; private set; }
        protected virtual IRestHandler<NotificationsClientConfig> RestHandler { get; set; }
        protected virtual IConfigProvider<NotificationsClientConfig> ConfigProvider { get; set; }
        public IServiceBus ServiceBus { get; set; }
        #endregion
        #region cst 

        public InternalNotificationClient(IRestHandler<NotificationsClientConfig> restHandler) : this(restHandler, null, null)
        {
        }
        public InternalNotificationClient(IRestHandler<NotificationsClientConfig> restHandler, IConfigProvider<NotificationsClientConfig> configProvider) : this(restHandler, null, configProvider)
        {
        }
        public InternalNotificationClient(IRestHandler<NotificationsClientConfig> restHandler, IServiceBus serviceBus, IConfigProvider<NotificationsClientConfig> configProvider)
        {
            this.ServiceBus = serviceBus;
            this.ConfigProvider = configProvider;
            this.RestHandler = restHandler;
            this.Initialized = Initialize();
        }




        #endregion
        #region actions
        public async Task<RestResponse<ServiceExecutionResponseDTO<InternalNotificationDTO>>> Create(ServiceExecutionRequestDTO<InternalNotificationDTO> InternalNotification, RequestContext requestContext)
        {
            #region validate.

            if (!this.Initialized)
            {
                throw new Exception("Client is not properly Initialized. please check the client config configurations.");
            }

            #endregion

            return await this.RestHandler.CallAsync<ServiceExecutionRequestDTO<InternalNotificationDTO>, ServiceExecutionResponseDTO<InternalNotificationDTO>>(HttpMethod.POST, InternalNotification, "/api/v0.1/InternalNotification/Create");

        }
        public async Task<RestResponse<ServiceExecutionResponseDTO<SearchResultsDTO<InternalNotificationDTO>>>> Get(ServiceExecutionRequestDTO<InternalNotificationSearchCriteriaDTO> criteria, RequestContext requestContext)
        {
            #region validate.

            if (!this.Initialized)
            {
                throw new Exception("Client is not properly Initialized. please check the client config configurations.");
            }

            #endregion

            return await this.RestHandler.CallAsync<ServiceExecutionRequestDTO<InternalNotificationSearchCriteriaDTO>,ServiceExecutionResponseDTO<SearchResultsDTO<InternalNotificationDTO>>> (HttpMethod.POST, criteria, "/api/v0.1/InternalNotification/Get");

        }
        public async Task<RestResponse<ServiceExecutionResponseDTO<bool>>> MarkasRead(ServiceExecutionRequestDTO<List<int?>> id, RequestContext requestContext)
        {
            #region validate.

            if (!this.Initialized)
            {
                throw new Exception("Client is not properly Initialized. please check the client config configurations.");
            }

            #endregion

            return await this.RestHandler.CallAsync<ServiceExecutionRequestDTO<List<int?>>, ServiceExecutionResponseDTO<bool>>(HttpMethod.POST, id, "/api/v0.1/InternalNotification/MarkasReadInternalNotification");

        }

        public async Task<RestResponse<ServiceExecutionResponseDTO<bool>>> MarkasDismissed(ServiceExecutionRequestDTO<int> id, RequestContext requestContext)
        {
            #region validate.

            if (!this.Initialized)
            {
                throw new Exception("Client is not properly Initialized. please check the client config configurations.");
            }

            #endregion

            return await this.RestHandler.CallAsync<ServiceExecutionRequestDTO<int>, ServiceExecutionResponseDTO<bool>>(HttpMethod.POST, id, "/api/v0.1/InternalNotification/MarkasDismissedInternalNotification");

        }

        public async Task<RestResponse<ServiceExecutionResponseDTO<bool>>> MarkasDeleted(ServiceExecutionRequestDTO<int> id, RequestContext requestContext)
        {
            #region validate.

            if (!this.Initialized)
            {
                throw new Exception("Client is not properly Initialized. please check the client config configurations.");
            }

            #endregion

            return await this.RestHandler.CallAsync<ServiceExecutionRequestDTO<int>, ServiceExecutionResponseDTO<bool>>(HttpMethod.POST, id, "/api/v0.1/InternalNotification/MarkasDeletedInternalNotification");

        }

        public async Task<RestResponse<ServiceExecutionResponseDTO<bool>>> Delete(ServiceExecutionRequestDTO<int> id, RequestContext requestContext)
        {
            #region validate.

            if (!this.Initialized)
            {
                throw new Exception("Client is not properly Initialized. please check the client config configurations.");
            }

            #endregion

            return await this.RestHandler.CallAsync<ServiceExecutionRequestDTO<int>, ServiceExecutionResponseDTO<bool>>(HttpMethod.POST, id, "/api/v0.1/InternalNotification/Delete");

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
