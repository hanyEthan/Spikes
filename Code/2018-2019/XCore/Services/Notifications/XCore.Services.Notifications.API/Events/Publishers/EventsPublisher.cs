using System;
using System.Threading;
using System.Threading.Tasks;
using XCore.Framework.Framework.ServiceBus.Contracts;
using XCore.Services.Notifications.Core.Models.Events.Domain;
using XCore.Services.Notifications.Core.Models.Events.Integration;
using XCore.Services.Notifications.Models.Contracts.Notifications;

namespace XCore.Services.Notifications.API.Events.Publishers
{
    public class EventsPublisher : MediatR.INotificationHandler<MessageTemplateAttachmentsDeletedDomainEvent>
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
        #region MediatR.INotificationHandler<MessageTemplateAttachmentsDeletedDomainEvent>.

        public async Task Handle(MessageTemplateAttachmentsDeletedDomainEvent domainEvent, CancellationToken cancellationToken)
        {
            try
            {
                Check();
                await SB.Publish<INotificationMessageTemplateAttachmentsDeletedIntegrationEvent, MessageTemplateAttachmentsDeletedIntegrationEvent>(Map(domainEvent));
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
      
        private MessageTemplateAttachmentsDeletedIntegrationEvent Map(MessageTemplateAttachmentsDeletedDomainEvent from)
        {
            var to = new MessageTemplateAttachmentsDeletedIntegrationEvent()
            {
                Action = from.Action,
                App = from.App,
                EventCorrelationId = from.EventCorrelationId,
                EventCreatedDateTime = from.EventCreatedDateTime,
                User = from.User,
                Module = from.Module,
                EventId = from.EventId,
                Model= from.Model,
                
                Attachments = from.Attachments,
            };

            return to;
        }

        #endregion
    }
}
