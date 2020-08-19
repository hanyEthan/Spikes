using System;
using System.Collections.Generic;
using System.Text;
using XCore.Framework.Infrastructure.Entities.Events.Domain;

namespace XCore.Services.Security.Core.Models.Events.Domain
{
   public class ClaimsDeassociatedFromRoleDomainEvent : DomainEventBase, MediatR.INotification
    {
        public int RoleId { get; set; }
        public List<int> Claims { get; set; }
        public ClaimsDeassociatedFromRoleDomainEvent()
        {
            base.App = null;
            base.Module = null;
            base.Model = "Claims";
            base.Action = "DeassociatedFromRole";
            base.User = null;
        }
    }
}
