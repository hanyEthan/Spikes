using System.Collections.Generic;
using XCore.Framework.Infrastructure.Entities.Events.Domain;

namespace XCore.Services.Security.Core.Models.Events.Domain
{
    public class PrivilegeAssociatedToRoleDomainEvent : DomainEventBase, MediatR.INotification
    {
        public int RoleId { get; set; }
        public List<int> PrivilegeIds { get; set; }
        public PrivilegeAssociatedToRoleDomainEvent()
        {
            base.App = null;
            base.Module = null;
            base.Model = "Privileges";
            base.Action = "AssociatedToRole";
            base.User = null;
        }
    }
}
