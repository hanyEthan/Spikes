using System.Collections.Generic;
using XCore.Framework.Infrastructure.Entities.Events.Domain;

namespace XCore.Services.IntegrationModels.Security
{
    public interface IRoleAssociatedToActorIntegrationEvent : IDomainEvent
    {
        int ActorId { get; set; }
        List<int> Roles { get; set; }
    }
}
