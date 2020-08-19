using System.Collections.Generic;
using XCore.Framework.Infrastructure.Entities.Events.Domain;

namespace XCore.Services.Security.Core.Models.Events.Domain
{
    public class PrivilegeDeassociatedFromRoleDomainEvent : DomainEventBase, MediatR.INotification
    {
        public int RoleId { get; set; }
        public List<int> Privileges { get; set; }
        public PrivilegeDeassociatedFromRoleDomainEvent()
        {
            base.App = null;
            base.Module = null;
            base.Model = "Privileges";
            base.Action = "DeassociatedFromRole";
            base.User = null;
        }
    }
}
