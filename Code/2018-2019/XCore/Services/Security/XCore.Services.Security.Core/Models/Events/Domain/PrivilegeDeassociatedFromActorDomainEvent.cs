using System.Collections.Generic;
using XCore.Framework.Infrastructure.Entities.Events.Domain;

namespace XCore.Services.Security.Core.Models.Events.Domain
{
    public class PrivilegeDeassociatedFromActorDomainEvent : DomainEventBase, MediatR.INotification
    {
        public int ActorId { get; set; }
        public List<int> Privileges { get; set;}
        public PrivilegeDeassociatedFromActorDomainEvent()
        {
            base.App = null;
            base.Module = null;
            base.Model = "Privileges";
            base.Action = "DeassociatedFromActor";
            base.User = null;
        }
    }
}
