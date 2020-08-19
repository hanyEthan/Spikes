using System.Collections.Generic;
using XCore.Framework.Infrastructure.Entities.Events.Domain;
using XCore.Services.Docs.Models.Contracts.Docs;
using XCore.Services.Docs.Models.Models.Docs;

namespace XCore.Services.Docs.Api.Models.Events.Integration
{
    public  class DocumentCreatedIntegrationEvent : DomainEventBase, IDocumentCreatedIntegrationEvent
    {
        public List<DocumentCreatedMetaData> Documents { get; set; }
    }
}
