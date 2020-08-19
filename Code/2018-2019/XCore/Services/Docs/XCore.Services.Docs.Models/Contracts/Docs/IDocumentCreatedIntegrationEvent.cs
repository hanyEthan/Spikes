using System.Collections.Generic;
using XCore.Framework.Infrastructure.Entities.Events.Domain;
using XCore.Services.Docs.Models.Models.Docs;

namespace XCore.Services.Docs.Models.Contracts.Docs
{
    public interface IDocumentCreatedIntegrationEvent : IDomainEvent
    {
        List<DocumentCreatedMetaData> Documents { get; set; }
    }
}
