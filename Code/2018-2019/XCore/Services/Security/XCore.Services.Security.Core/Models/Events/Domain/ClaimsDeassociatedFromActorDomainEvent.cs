using System;
using System.Collections.Generic;
using System.Text;
using XCore.Framework.Infrastructure.Entities.Events.Domain;

namespace XCore.Services.Security.Core.Models.Events.Domain
{
   public class ClaimsDeassociatedFromActorDomainEvent : DomainEventBase, MediatR.INotification
    {
        public int ActorId { get; set; }
        public List<int> Claims { get; set; }
        public ClaimsDeassociatedFromActorDomainEvent()
        {
            base.App = null;
            base.Module = null;
            base.Model = "Claims";
            base.Action = "DeassociatedFromActor";
            base.User = null;
        }
    }
}
