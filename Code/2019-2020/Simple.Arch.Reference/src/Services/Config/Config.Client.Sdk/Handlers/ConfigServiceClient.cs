using System;
using System.Threading.Tasks;
using Config.Messaging.Contracts.Messages;
using Grpc.Net.Client;
using Mcs.Invoicing.Core.Framework.Infrastructure.AsyncCommunication.Contracts;
using Mcs.Invoicing.Core.Framework.Infrastructure.Context.Mappers;
using Mcs.Invoicing.Services.Config.Api.gRPC.Protos;
using Mcs.Invoicing.Services.Config.Client.Sdk.Clients.Async;
using Mcs.Invoicing.Services.Config.Client.Sdk.Clients.Async.Models;
using Mcs.Invoicing.Services.Config.Client.Sdk.Clients.gRPC;
using Mcs.Invoicing.Services.Config.Client.Sdk.Clients.Rest;
using Mcs.Invoicing.Services.Config.Client.Sdk.Contracts;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace Mcs.Invoicing.Services.Config.Client.Sdk.Handlers
{
    public class ConfigServiceClient : IConfigServiceClient, IDisposable
    {
        #region props.

        public bool Initialized { get; private set; }

        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly ILogger<ConfigServiceClient> _logger;

        protected virtual AsyncClientConfigurations AsyncConfigurations { get; set; }
        protected IAsyncClient AsyncClient { get; set; }
        protected ConfigRestClient RestClient { get; set; }

        private GrpcChannel _grpcBackChannel { get; set; }
        protected ConfigItemsProtoAPI.ConfigItemsProtoAPIClient GrpcClient { get; set; }

        #endregion
        #region cst.

        public ConfigServiceClient(ILogger<ConfigServiceClient> logger,
                                   IAsyncClient asyncClient,
                                   AsyncClientConfigurations asyncConfigurations,
                                   GrpcClientConfig grpcClientConfig,
                                   RestConfig restConfig)
        {
            this._logger = logger;
            this.AsyncClient = asyncClient;
            this.AsyncConfigurations = asyncConfigurations;

            this.Initialized = Initialize(grpcClientConfig, restConfig);
        }


        public ConfigServiceClient(IHttpContextAccessor httpContextAccessor,
                                   ILogger<ConfigServiceClient> logger,
                                   IAsyncClient asyncClient,
                                   AsyncClientConfigurations asyncConfigurations,
                                   GrpcClientConfig grpcClientConfig,
                                   RestConfig restConfig)
                                   : this(logger, asyncClient, asyncConfigurations, grpcClientConfig, restConfig)
        {
            this._httpContextAccessor = httpContextAccessor;
        }

        #endregion

        #region IConfigServiceClient

        // [Async Message] Sample SDK entry.
        public async Task CreateAsync(ConfigCreateCommandMessage request)
        {
            Check();

            var message = MapAsyncMessage(request);
            await this.AsyncClient.Send(message, this.AsyncConfigurations.ServiceEndpoint);
        }

        // [gRPC] Sample SDK entry.
        public async Task<CreateConfigItemResponseProto> CreateProto(ConfigCreateCommandMessage request, string jwtToken = null)
        {
            Check();

            var message = MapProto(request);
            var header = MapProtoHttpHeader(jwtToken);

            return await this.GrpcClient.CreateAsync(message, header);
        }

        // [Rest] Sample SDK entry.
        public async Task<bool?> IsHealthy(string jwtToken = null)
        {
            Check();
            return await this.RestClient.IsHealthy(jwtToken);
        }

        #endregion
        #region IDisposable

        public void Dispose()
        {
            try
            {
                if (this._grpcBackChannel != null)
                {
                    this._grpcBackChannel.Dispose();
                }
            }
            catch (Exception x)
            {
                // todo : log
                //throw;
            }
        }

        #endregion

        #region helpers.

        private bool Initialize(GrpcClientConfig grpcConfig, RestConfig restConfig)
        {
            bool isValid = true;

            isValid = isValid && this.AsyncClient != null;
            isValid = isValid && this.AsyncClient.Initialized;

            isValid = isValid && this.AsyncConfigurations != null;
            isValid = isValid && this.AsyncConfigurations.IsValid;
            
            isValid = isValid && InitializeGRPC(grpcConfig);
            isValid = isValid && InitializeRest(restConfig);

            return isValid;
        }
        private bool InitializeGRPC(GrpcClientConfig config)
        {
            this._grpcBackChannel = GrpcChannel.ForAddress(config.ServiceEndpoint);
            this.GrpcClient = new ConfigItemsProtoAPI.ConfigItemsProtoAPIClient(this._grpcBackChannel);

            return true;
        }
        private bool InitializeRest(RestConfig config)
        {
            this.RestClient = new ConfigRestClient(config?.ServiceEndpoint);

            return true;
        }

        private void Check()
        {
            if (!this.Initialized)
            {
                throw new Exception("Client is not properly Initialized. please check the client config configurations.");
            }
        }

        private IConfigCreateCommandMessage MapAsyncMessage(ConfigCreateCommandMessage from)
        {
            var to = from;
            to = RequestContextMapper.Instance.Map<ConfigCreateCommandMessage>(to);
            return to;
        }
        private CreateConfigItemCommandProto MapProto(ConfigCreateCommandMessage from)
        {
            var to = from == null
                   ? null
                   : new CreateConfigItemCommandProto()
                   {
                       Key = from.Key,
                       Value = from.Value,
                       Description = from.Description,
                       ModuleId = from.ModuleId,
                   };

            return to;
        }
        private Grpc.Core.Metadata MapProtoHttpHeader(string jwtToken = null)
        {
            var header = new Grpc.Core.Metadata();

            #region jwt

            if (!string.IsNullOrWhiteSpace(jwtToken))
            {
                header.Add("Authorization", $"Bearer {jwtToken}");
            }

            #endregion

            return header;
        }

        #endregion
    }
}
