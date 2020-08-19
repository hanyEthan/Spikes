using System;
using System.Collections.Generic;
using System.Text;
using XCore.Framework.Infrastructure.Entities.Events.Domain;

namespace XCore.Services.Security.Core.Models.Events.Domain
{
   public class ClaimsAssociatedToActorDomainEvent : DomainEventBase, MediatR.INotification
    {
        public int ActorId { get; set; }
        public List<int> Claims { get; set; }
        #region cst.

        public ClaimsAssociatedToActorDomainEvent()
        {
            base.App = null;
            base.Module = null;
            base.Model = "Claims";
            base.Action = "AssociatedToActor";

            base.User = null;
        }

        #endregion
    }
}