using System;
using Mcs.Invoicing.Services.Audit.Messaging.Contracts.Enums;
using static Mcs.Invoicing.Core.Framework.Infrastructure.Models.Common.BaseRequestContext;

namespace Mcs.Invoicing.Services.Audit.Messaging.Contracts.Messages
{
    public interface IAuditMessage
    {
        public HeaderContent Header { get; set; }
        public ServiceTypes ServiceId { get; set; }
        public EventTypes EventTypeId { get; set; }
        public ObjectTypes ObjectTypeId { get; set; }
        public string SourceIp { get; set; }
        public string Description { get; set; }
        public string ObjectTypeReferenceId { get; set; }
        public bool IsOperationSuccess { get; set; }
        public string MetaData { get; set; }

        public DateTime AuditDateTimeUtc { get; set; }
    }
}
