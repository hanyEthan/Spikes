using System;
using System.Linq;
using System.Threading.Tasks;
using Mcs.Invoicing.Core.Framework.Infrastructure.AsyncCommunication.Contracts;
using Mcs.Invoicing.Services.Audit.Client.Sdk.Configurations.Models;
using Mcs.Invoicing.Services.Audit.Client.Sdk.Contracts;
using Mcs.Invoicing.Services.Audit.Messaging.Contracts.Messages;

namespace Mcs.Invoicing.Services.Audit.Client.Sdk.Handlers
{
    public class AuditClient : IAuditClient
    {
        public AuditClient(IAsyncClient asyncClient, AuditClientConfigurations configurations)
        {
            this.AsyncClient = asyncClient;
            this.Configurations = configurations;

            this.Initialized = Initialize();
        }

        public bool Initialized { get; private set; }

        protected virtual AuditClientConfigurations Configurations { get; set; }
        protected IAsyncClient AsyncClient { get; set; }
        public static string RandomString(int length)
        {
            Random random = new Random();
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
            return new string(Enumerable.Repeat(chars, length)
                       .Select(s => s[random.Next(s.Length)]).ToArray());
        }

        public async Task CreateAsync(IAuditMessage request)
        {
              Check();
              // var message = Map(request);
              await this.AsyncClient.Send(request, this.Configurations.ServiceEndpoint);
        }

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
    }
}
