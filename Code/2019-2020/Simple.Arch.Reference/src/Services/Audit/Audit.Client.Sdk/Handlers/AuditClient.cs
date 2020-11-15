using System;
using System.Threading.Tasks;
using Mcs.Invoicing.Core.Framework.Infrastructure.AsyncCommunication.Contracts;
using Mcs.Invoicing.Services.Audit.Client.Sdk.Contracts;
using Mcs.Invoicing.Services.Audit.Client.Sdk.IOC.Models;
using Mcs.Invoicing.Services.Audit.Messaging.Contracts.Messages;

namespace Mcs.Invoicing.Services.Audit.Client.Sdk.Handlers
{
    public class AuditClient : IAuditClient
    {
        #region props.

        public bool Initialized { get; private set; }

        protected IAsyncClient AsyncClient { get; set; }
        protected virtual AuditClientConfigurations Configurations { get; set; }

        #endregion
        #region cst.

        public AuditClient(IAsyncClient asyncClient, AuditClientConfigurations configurations)
        {
            this.AsyncClient = asyncClient;
            this.Configurations = configurations;

            this.Initialized = Initialize();
        }

        #endregion

        #region IAuditClient

        public async Task CreateAsync(IAuditMessage request)
        {
            Check();
            // var message = Map(request);
            await this.AsyncClient.Send(request, this.Configurations.ServiceEndpoint);
        }

        #endregion

        #region helpers.

        private bool Initialize()
        {
            bool isValid = true;

            isValid = isValid && this.AsyncClient != null;
            isValid = isValid && this.AsyncClient.Initialized;
            isValid = isValid && this.Configurations != null;
            isValid = isValid && this.Configurations.IsValid;

            return isValid;
        }

        private void Check()
        {
            if (!this.Initialized)
            {
                throw new Exception("Client is not properly Initialized. please check the client config configurations.");
            }
        }

        #endregion
    }
}
