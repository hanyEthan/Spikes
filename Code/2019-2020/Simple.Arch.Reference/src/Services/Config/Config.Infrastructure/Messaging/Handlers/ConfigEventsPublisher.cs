using System;
using System.Threading.Tasks;
using Config.Messaging.Contracts.Messages;
using Mcs.Invoicing.Core.Framework.Infrastructure.AsyncCommunication.Contracts;
using Mcs.Invoicing.Core.Framework.Infrastructure.Models.Common;
using Mcs.Invoicing.Services.Config.Application.Common.Contracts;
using Mcs.Invoicing.Services.Config.Domain.Events;
using Mcs.Invoicing.Services.Core.Framework.Infrastructure.Models.Mappers;

namespace Mcs.Invoicing.Services.Config.Infrastructure.Messaging.Handlers
{
    public class ConfigEventsPublisher : IConfigEventsPublisher
    {
        #region props.

        public bool Initialized { get; private set; }

        protected IAsyncClient AsyncClient { get; set; }
        protected readonly IModelMapper<BaseRequestContext, BaseRequestContext> _requestContextMapper;

        #endregion
        #region cst.

        public ConfigEventsPublisher(IAsyncClient asyncClient,
                                     IModelMapper<BaseRequestContext, BaseRequestContext> requestContextMapper)
        {
            this.AsyncClient = asyncClient;
            this._requestContextMapper = requestContextMapper;

            this.Initialized = Initialize();
        }

        #endregion
        #region ConfigEventsPublisher.

        public async Task PublishEvent(ConfigCreatedEvent @event)
        {
            Check();

            var message = Map(@event);  // context mapping.
            await this.AsyncClient.Publish(message);
        }

        #endregion

        #region helpers.

        private bool Initialize()
        {
            bool isValid = true;

            isValid = isValid && this.AsyncClient != null;
            isValid = isValid && this.AsyncClient.Initialized;

            return isValid;
        }
        private void Check()
        {
            if (!this.Initialized)
            {
                throw new Exception("ConfigEventsPublisher is not properly Initialized. please check the client config configurations.");
            }
        }

        private IConfigCreatedEventMessage Map(ConfigCreatedEvent from)
        {
            var to = from;
            to = this._requestContextMapper.Map<ConfigCreatedEvent>(to);  // unpack context

            return to;
        }

        #endregion
    }
}
