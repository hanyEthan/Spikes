using System.Collections.Generic;
using XCore.Framework.Infrastructure.Entities.Events.Domain;

namespace XCore.Services.Security.Models.Contracts
{
    public interface IClaimAssociatedToActorIntegrationEvent : IDomainEvent
    {
        int ActorId { get; set; }
        List<int> Claims { get; set; }
    }
}
