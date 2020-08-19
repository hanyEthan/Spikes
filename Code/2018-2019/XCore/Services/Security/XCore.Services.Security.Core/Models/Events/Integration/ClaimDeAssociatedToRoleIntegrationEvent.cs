using System;
using System.Collections.Generic;
using System.Text;
using XCore.Framework.Infrastructure.Entities.Events.Domain;
using XCore.Services.IntegrationModels.Security;

namespace XCore.Services.Security.Core.Models.Events.Integration
{
    public class ClaimDeAssociatedToActorIntegrationEvent : DomainEventBase, IClaimDeAssociatedToActorIntegrationEvent
    {
        public int ActorId { get; set; }
        public List<int> Claims { get; set; }
    }
}
