using System;
using System.Threading;
using System.Threading.Tasks;
using XCore.Framework.Framework.ServiceBus.Contracts;
using XCore.Services.Docs.Core.Models.Events.Domain;
using XCore.Services.Docs.Core.Models.Events.Integration;
using XCore.Services.Docs.Models.Contracts.Docs;

namespace XCore.Services.Docs.API.Events.Publishers
{
    public class EventsPublisher : MediatR.INotificationHandler<DocumentCreatedDomainEvent>
    {
        #region props.

        public bool? Initialized { get; protected set; }
        protected IServiceBus SB { get; set; }

        #endregion
        #region cst.

        public EventsPublisher(IServiceBus sB)
        {
            this.SB = sB;

            this.Initialized = this.Initialize();
        }

        #endregion
        #region IDocumentEventsPublisher.

        public async Task Handle(DocumentCreatedDomainEvent Event, CancellationToken cancellationToken)
        {
            try
            {
                Check();
                await SB.Publish<IDocumentCreatedIntegrationEvent, DocumentCreatedIntegrationEvent>(Map(Event));
            }
            catch (Exception x)
            {
                // TODO : log
                //throw;
                return;
            }
        }

        #endregion
        #region helpers.

        private bool Initialize()
        {
            bool isValid = true;

            isValid = isValid && (this.SB?.Initialized ?? false);

            return isValid;
        }
        private void Check()
        {
            if (!this.Initialized.GetValueOrDefault())
            {
                throw new Exception("Events Publisher : not initialized correctly.");
            }
        }
        private DocumentCreatedIntegrationEvent Map(DocumentCreatedDomainEvent from)
        {
            if (from == null) return null;

            var to = new DocumentCreatedIntegrationEvent();
            to.Documents = from.Documents;
            return to;
        }
        #endregion
    }
}
