﻿using System.Collections.Generic;
using XCore.Framework.Infrastructure.Entities.Events.Domain;
using XCore.Services.IntegrationModels.Security;

namespace XCore.Services.Security.Core.Models.Events.Integration
{
    public class RoleDeassociatedFromActorIntegrationEvent : DomainEventBase, IRoleDeassociatedFromActorIntegrationEvent
    {
        public int ActorId { get; set; }
        public List<int> Roles { get; set; }
    }
}
