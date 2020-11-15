using System;
using Mcs.Invoicing.Core.Framework.Infrastructure.Models.Events;
using Mcs.Invoicing.Services.Audit.Messaging.Contracts.Enums;

namespace Mcs.Invoicing.Services.Audit.Messaging.Contracts.Messages
{
    public interface IAuditMessage : IIntegrationEvent
    {
        public AuditsServiceTypes ServiceId { get; set; }
        public AuditsEventTypes EventTypeId { get; set; }
        public AuditsObjectTypes ObjectTypeId { get; set; }
        public string SourceIp { get; set; }
        public string Description { get; set; }
        public string ObjectTypeReferenceId { get; set; }
        public bool IsOperationSuccess { get; set; }
        public string MetaData { get; set; }

        public DateTime AuditDateTimeUtc { get; set; }
    }
}
