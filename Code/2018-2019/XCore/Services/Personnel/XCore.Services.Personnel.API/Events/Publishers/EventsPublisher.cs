using System;
using System.Threading;
using System.Threading.Tasks;
using XCore.Framework.Framework.ServiceBus.Contracts;
using XCore.Services.IntegrationModels.Person;
using XCore.Services.Personnel.Models.DTO.Events.Integration;
using XCore.Services.Personnel.Models.Events.Domain;

namespace XCore.Services.Personnel.API.Events.Publishers
{
    public class EventsPublisher : MediatR.INotificationHandler<PersonCreatedDomainEvent>, MediatR.INotificationHandler<PersonActivatedDomainEvent>
        , MediatR.INotificationHandler<PersonDeActivatedDomainEvent>, MediatR.INotificationHandler<PersonUpdatedDomainEvent>,
        MediatR.INotificationHandler<PersonDeletedDomainEvent>
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
        #region IPersonEventsPublisher.

        public async Task Handle(PersonCreatedDomainEvent notification, CancellationToken cancellationToken)
        {
            try
            {
                Check();
                await SB.Publish<IPersonCreatedIntegrationEvent, PersonCreatedIntegrationEvent>(Map(notification));
            }
            catch (Exception x)
            {
                // TODO : log
                //throw;
                return;
            }
        }
        public async Task Handle(PersonUpdatedDomainEvent notification, CancellationToken cancellationToken)
        {
            try
            {
                Check();
                await SB.Publish<IPersonUpdatedIntegrationEvent, PersonUpdatedIntegrationEvent>(Map(notification));
            }
            catch (Exception x)
            {
                // TODO : log
                //throw;
                return;
            }
        }
        public async Task Handle(PersonDeletedDomainEvent notification, CancellationToken cancellationToken)
        {
            try
            {
                Check();
                await SB.Publish<IPersonDeletedIntegrationEvent, PersonDeletedIntegrationEvent>(Map(notification));
            }
            catch (Exception x)
            {
                // TODO : log
                //throw;
                return;
            }
        }
        public async Task Handle(PersonActivatedDomainEvent notification, CancellationToken cancellationToken)
        {
            try
            {
                Check();
                await SB.Publish<IPersonActivatedIntegrationEvent, PersonActivatedIntegrationEvent>(Map(notification));
            }
            catch (Exception x)
            {
                // TODO : log
                //throw;
                return;
            }
        }
        public async Task Handle(PersonDeActivatedDomainEvent notification, CancellationToken cancellationToken)
        {
            try
            {
                Check();
                await SB.Publish<IPersonDeActivatedIntegrationEvent, PersonDeActivatedIntegrationEvent>(Map(notification));
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
        private PersonCreatedIntegrationEvent Map(PersonCreatedDomainEvent from)
        {
            if (from == null) return null;

            var to = new PersonCreatedIntegrationEvent();
            to.App = from.App;
            to.Module = from.Module;
            to.PersonId = from.PersonId;
            to.Action = from.Action;
            to.EventCorrelationId = from.EventCorrelationId;
            to.EventCreatedDateTime = from.EventCreatedDateTime;
            to.EventId = from.EventId;
            to.EventResponse = from.EventResponse;
            to.Model = from.Model;
            to.Module = from.Module;
            to.User = from.User;

            return to;
        }
        private PersonActivatedIntegrationEvent Map(PersonActivatedDomainEvent from)
        {
            if (from == null) return null;

            var to = new PersonActivatedIntegrationEvent();
            to.App = from.App;
            to.Module = from.Module;
            to.Code = from.Code;
            to.Action = from.Action;
            to.EventCorrelationId = from.EventCorrelationId;
            to.EventCreatedDateTime = from.EventCreatedDateTime;
            to.EventId = from.EventId;
            to.EventResponse = from.EventResponse;
            to.Model = from.Model;
            to.Module = from.Module;
            to.User = from.User;

            return to;
        }
        private PersonDeActivatedIntegrationEvent Map(PersonDeActivatedDomainEvent from)
        {
            if (from == null) return null;

            var to = new PersonDeActivatedIntegrationEvent();
            to.App = from.App;
            to.Module = from.Module;
            to.Code = from.Code;
            to.Action = from.Action;
            to.EventCorrelationId = from.EventCorrelationId;
            to.EventCreatedDateTime = from.EventCreatedDateTime;
            to.EventId = from.EventId;
            to.EventResponse = from.EventResponse;
            to.Model = from.Model;
            to.Module = from.Module;
            to.User = from.User;
            return to;
        }
        private PersonUpdatedIntegrationEvent Map(PersonUpdatedDomainEvent from)
        {
            if (from == null) return null;

            var to = new PersonUpdatedIntegrationEvent();
            to.App = from.App;
            to.Module = from.Module;
            to.PersonId = from.PersonId;
            to.Action = from.Action;
            to.EventCorrelationId = from.EventCorrelationId;
            to.EventCreatedDateTime = from.EventCreatedDateTime;
            to.EventId = from.EventId;
            to.EventResponse = from.EventResponse;
            to.Model = from.Model;
            to.Module = from.Module;
            to.User = from.User;
            return to;
        }
        private PersonDeletedIntegrationEvent Map(PersonDeletedDomainEvent from)
        {
            if (from == null) return null;

            var to = new PersonDeletedIntegrationEvent();
            to.App = from.App;
            to.Module = from.Module;
            to.Code = from.Code;
            to.Action = from.Action;
            to.EventCorrelationId = from.EventCorrelationId;
            to.EventCreatedDateTime = from.EventCreatedDateTime;
            to.EventId = from.EventId;
            to.EventResponse = from.EventResponse;
            to.Model = from.Model;
            to.Module = from.Module;
            to.User = from.User;
            return to;
        }
        #endregion
    }
}
