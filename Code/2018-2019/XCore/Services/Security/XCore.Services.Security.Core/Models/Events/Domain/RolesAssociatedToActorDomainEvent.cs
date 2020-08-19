using System.Collections.Generic;
using XCore.Framework.Infrastructure.Entities.Events.Domain;

namespace XCore.Services.Security.Core.Models.Events.Domain
{
    public class RolesAssociatedToActorDomainEvent : DomainEventBase, MediatR.INotification
    {
        public int ActorId { get; set; }
        public List<int> Roles { get; set; }
        #region cst.

        public RolesAssociatedToActorDomainEvent()
        {
            base.App = null;
            base.Module = null;
            base.Model = "Roles";
            base.Action = "AssociatedToActor";
            base.User = null;
        }

        #endregion
    }
}
