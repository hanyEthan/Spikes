using System.Collections.Generic;
using XCore.Framework.Infrastructure.Entities.Events.Domain;

namespace XCore.Services.Security.Core.Models.Events.Domain
{
    public class RolesDeassociatedFromActorDomainEvent : DomainEventBase, MediatR.INotification
    {
        public int ActorId { get; set; }
        public List<int> Roles { get; set; }
        public RolesDeassociatedFromActorDomainEvent()
        {
            base.App = null;
            base.Module = null;
            base.Model = "Roles";
            base.Action = "DeassociatedFromActor";
            base.User = null;
        }
    }
}
