using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using XCore.Services.Docs.Core.Models.Events.Domain;

namespace XCore.Services.Docs.Core.Contracts.Events
{
    public interface IDocumentEventsPublisher
    {
        bool? Initialized { get; }
        Task DocumentCreatedEvent(DocumentCreatedDomainEvent Event);
    }
}
