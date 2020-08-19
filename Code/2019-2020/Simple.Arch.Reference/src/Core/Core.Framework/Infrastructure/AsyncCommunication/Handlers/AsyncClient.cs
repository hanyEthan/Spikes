using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using MassTransit;
using Mcs.Invoicing.Core.Framework.Infrastructure.AsyncCommunication.Contracts;
using Mcs.Invoicing.Core.Framework.Infrastructure.AsyncCommunication.Support;

namespace Mcs.Invoicing.Core.Framework.Infrastructure.AsyncCommunication.Handlers
{
    public class AsyncClient : IAsyncClient
    {
        #region props.

        public bool Initialized { get; protected set; }

        protected virtual IBusControl Bus { get; set; }
        protected virtual AsyncConfiguration Config { get; set; }

        #endregion
        #region cst.

        public AsyncClient(AsyncConfiguration config, IBusControl busControl)
        {
            this.Bus = busControl;
            this.Config = config;

            this.Initialized = Initialize();
        }

        #endregion

        #region publics.

        public async Task Send<TContract>(TContract message, string endpoint, CancellationToken cancellationToken = default) where TContract : class
        {

            Check();
            var sendEndPoint = await this.GetSenderEndpoint(endpoint);
            await sendEndPoint.Send(message, cancellationToken);
        }
        public async Task Send<TContract>(TContract message, string endpoint, Action<SendContext<TContract>> context, CancellationToken cancellationToken = default) where TContract : class
        {

            Check();
            var sendEndPoint = await this.GetSenderEndpoint(endpoint);
            await sendEndPoint.Send(message, context, cancellationToken);
        }

        public async Task Publish<TContract>(TContract message, CancellationToken cancellationToken = default) where TContract : class
        {
            Check();
            await this.Bus.Publish(message, cancellationToken);
        }
        public async Task Publish<TContract>(TContract message, Action<PublishContext<TContract>> context, CancellationToken cancellationToken = default) where TContract : class
        {
            Check();
            await this.Bus.Publish(message, context, cancellationToken);
        }

        #endregion
        #region helpers.

        protected virtual bool Initialize()
        {
            bool state = true;

            state = state && this.Bus != null;
            state = state && this.Config != null;

            return state;
        }
        protected void Check()
        {
            if (!this.Initialized) throw new Exception("Service Bus : is not initialized correctly.");
        }

        protected virtual async Task<ISendEndpoint> GetSenderEndpoint(string endpoint)
        {
            var uri = new Uri(GetComponentEndPoint(this.Config?.Transport?.Uri, endpoint));
            return await this.Bus.GetSendEndpoint(uri);
        }
        private string GetComponentEndPoint(string baseUri, string ComponentRelativeUri)
        {
            #region validate.

            if (string.IsNullOrWhiteSpace(this.Config?.Transport?.Uri)) throw new Exception("Service Bus : invalid configured transport Uri.");

            #endregion

            baseUri = baseUri.EndsWith("/") ? baseUri : baseUri + "/";
            ComponentRelativeUri = ComponentRelativeUri ?? "";

            return string.Concat(baseUri, ComponentRelativeUri);
        }

        #endregion
    }
}
