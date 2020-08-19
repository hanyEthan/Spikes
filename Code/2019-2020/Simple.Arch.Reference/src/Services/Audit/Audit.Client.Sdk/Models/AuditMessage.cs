using System;
using Mcs.Invoicing.Core.Framework.Infrastructure.Models.Common;
using Mcs.Invoicing.Services.Audit.Messaging.Contracts.Enums;
using Mcs.Invoicing.Services.Audit.Messaging.Contracts.Messages;

namespace Mcs.Invoicing.Services.Audit.Client.Sdk.Models
{
    public class AuditMessage : BaseRequestContext, IAuditMessage
    {
        public string Description { get; set; }
        public ServiceTypes ServiceId { get; set; }
        public EventTypes EventTypeId { get; set; }
        public ObjectTypes ObjectTypeId { get; set; }
        public string SourceIp { get; set; }
        public string ObjectTypeReferenceId { get; set; }
        public bool IsOperationSuccess { get; set; }
        public string MetaData { get; set; }
        public DateTime AuditDateTimeUtc { get; set; }
    }
}
