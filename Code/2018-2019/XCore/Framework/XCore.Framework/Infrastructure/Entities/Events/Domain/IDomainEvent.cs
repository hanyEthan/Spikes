using System;

namespace XCore.Framework.Infrastructure.Entities.Events.Domain
{
    public interface IDomainEvent
    {
        string EventId { get; set; }
        string EventCorrelationId { get; set; }
        DateTime EventCreatedDateTime { get; set; }
        string EventResponse { get; set; }

        string App { get; set; }
        string Module { get; set; }
        string Model { get; set; }
        string Action { get; set; }
        string User { get; set; }
    }
}
