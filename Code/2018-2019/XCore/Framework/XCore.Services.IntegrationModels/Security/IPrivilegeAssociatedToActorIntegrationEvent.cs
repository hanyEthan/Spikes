using System.Collections.Generic;
using XCore.Framework.Infrastructure.Entities.Events.Domain;

namespace XCore.Services.IntegrationModels.Security
{
    public interface IPrivilegeAssociatedToActorIntegrationEvent : IDomainEvent
    {
        int ActorId { get; set; }
        List<int> Privileges { get; set; }
    }
}
