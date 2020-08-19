using System;
using System.Collections.Generic;
using System.Text;
using XCore.Framework.Infrastructure.Entities.Events.Domain;

namespace XCore.Services.Security.Models.Contracts
{
    public interface IClaimDeAssociatedToActorIntegrationEvent : IDomainEvent
    {
        int ActorId { get; set; }
        List<int> Claims { get; set; }
    }
}