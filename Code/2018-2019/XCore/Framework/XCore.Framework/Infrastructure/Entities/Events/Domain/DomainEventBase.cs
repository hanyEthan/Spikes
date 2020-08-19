using System;

namespace XCore.Framework.Infrastructure.Entities.Events.Domain
{
    public abstract class DomainEventBase : IDomainEvent
    {
        public string EventId { get; set; } = Guid.NewGuid().ToString();
        public string EventCorrelationId { get; set; } = Guid.NewGuid().ToString();
        public DateTime EventCreatedDateTime { get; set; } = DateTime.Now;
        public string EventResponse { get; set; }

        public string App { get; set; }
        public string Module { get; set; }
        public string Model { get; set; }
        public string Action { get; set; }
        public string User { get; set; }
    }
}
