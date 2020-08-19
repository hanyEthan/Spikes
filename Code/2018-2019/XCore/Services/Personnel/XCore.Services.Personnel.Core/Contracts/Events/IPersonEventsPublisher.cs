using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using XCore.Services.Personnel.Models.Events.Domain;

namespace XCore.Services.Personnel.Core.Contracts.Events
{
    public interface IPersonEventsPublisher
    {
        bool? Initialized { get; }
        Task PersonCreatedEvent(PersonCreatedDomainEvent Event);
        Task PersonUpdatedEvent(PersonUpdatedDomainEvent Event);
        Task PersonDeletedEvent(PersonDeletedDomainEvent Event);
        Task PersonActivatedEvent(PersonActivatedDomainEvent Event);
        Task PersonDeActivatedEvent(PersonDeActivatedDomainEvent Event);
    }
}
