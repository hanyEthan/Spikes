using System;
using System.Threading.Tasks;
using XCore.Framework.Framework.ServiceBus.Contracts;
using XCore.Framework.Framework.Services.Rest;
using XCore.Framework.Infrastructure.Config.Contracts;
using XCore.Framework.Infrastructure.Context.Services.Models;
using XCore.Services.Audit.Models.Contracts;
using XCore.Services.Audit.SDK.Contracts;
using XCore.Services.Audit.SDK.Models;

namespace XCore.Services.Audit.SDK.Client
{
    public class AuditClient : IAuditClient
    {
        #region props.

        public bool Initialized { get; private set; }

        protected virtual IRestHandler<AuditClientConfig> RestHandler { get; set; }
        protected virtual IConfigProvider<AuditClientConfig> ConfigProvider { get; set; }
        protected IServiceBus ServiceBus { get; set; }

        #endregion
        #region cst.

        public AuditClient(IRestHandler<AuditClientConfig> restHandler) : this(restHandler, null, null)
        {
        }
        public AuditClient(IRestHandler<AuditClientConfig> restHandler, IConfigProvider<AuditClientConfig> configProvider) : this(restHandler, null, configProvider)
        {
        }
        public AuditClient(IRestHandler<AuditClientConfig> restHandler, IServiceBus serviceBus, IConfigProvider<AuditClientConfig> configProvider)
        {
            this.ServiceBus = serviceBus;
            this.ConfigProvider = configProvider;
            this.RestHandler = restHandler;

            this.Initialized = Initialize();
        }

        #endregion

        #region IAuditClient

        public async Task<RestResponse<ServiceExecutionResponseDTO<bool>>> Create(ServiceExecutionRequestDTO<AuditTrailDTO> request)
        {
            #region validate.

            if (!this.Initialized)
            {
                throw new Exception("Client is not properly Initialized. please check the client config configurations.");
            }

            #endregion

            return await this.RestHandler.CallAsync<ServiceExecutionRequestDTO<AuditTrailDTO>, ServiceExecutionResponseDTO<bool>>(HttpMethod.POST, request, "/api/v0.1/audit/create/");
        }
        public async Task<RestResponse<ServiceExecutionResponseDTO<SearchResultsDTO<AuditTrailDTO>>>> Get(ServiceExecutionRequestDTO<AuditSearchCriteriaDTO> request)
        {
            #region validate.

            if (!this.Initialized)
            {
                throw new Exception("Client is not properly Initialized. please check the client config configurations.");
            }

            #endregion

            return await this.RestHandler.CallAsync<ServiceExecutionRequestDTO<AuditSearchCriteriaDTO>, ServiceExecutionResponseDTO<SearchResultsDTO<AuditTrailDTO>>>(HttpMethod.POST, request, "/api/v0.1/audit/get/");
        }

        public async Task CreateAsync(ServiceExecutionRequestDTO<IAuditMessage> request)
        {
            #region validate.

            if (!this.Initialized)
            {
                // throw new Exception("Client is not properly Initialized. please check the client config configurations.");
            }

            #endregion

            var message = Map(request);
            await this.ServiceBus.Send<IAuditMessage, AuditTrailDTO>(message, Constants.AuditAsyncEndppoint);  // note : please consider setting the target endpoint through config, instead of being a constant.
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
        private AuditTrailDTO Map(ServiceExecutionRequestDTO<IAuditMessage> from)
        {
            if (from == null || from.Content == null) return null;

            var to = new AuditTrailDTO()
            {
                Action = from.Content.Action,
                App = from.Content.App,
                ConnectionMethod = from.Content.ConnectionMethod,
                CreatedBy = from.Content.CreatedBy,
                DestinationAddress = from.Content.DestinationAddress,
                DestinationIP = from.Content.DestinationIP,
                DestinationPort = from.Content.DestinationPort,
                Entity = from.Content.Entity,
                Level = from.Content.Level,
                MetaData = from.Content.MetaData,
                ModifiedBy = from.Content.ModifiedBy,
                Module = from.Content.Module,
                SourceClient = from.Content.SourceClient,
                SourceIP = from.Content.SourceIP,
                SourceOS = from.Content.SourceOS,
                SourcePort = from.Content.SourcePort,
                SyncStatus = from.Content.SyncStatus,
                Text = from.Content.Text,
                UserId = from.Content.UserId,
                UserName = from.Content.UserName,
            };

            return to;

        }

        #endregion
    }
}
