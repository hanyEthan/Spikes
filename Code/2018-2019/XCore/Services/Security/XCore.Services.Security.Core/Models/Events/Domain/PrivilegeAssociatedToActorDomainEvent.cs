using System.Collections.Generic;
using XCore.Framework.Infrastructure.Entities.Events.Domain;

namespace XCore.Services.Security.Core.Models.Events.Domain
{
    public class PrivilegeAssociatedToActorDomainEvent  : DomainEventBase, MediatR.INotification
    {
        public int ActorId { get; set; }
        public List<int> Privileges { get; set; }
        public PrivilegeAssociatedToActorDomainEvent()
        {
            base.App = null;
            base.Module = null;
            base.Model = "Privileges";
            base.Action = "AssociatedToActor";
            base.User = null;
        }
    }
}
