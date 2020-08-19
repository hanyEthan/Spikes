using XCore.Framework.Infrastructure.Entities.Events.Domain;

namespace XCore.Services.Security.Core.Models.Events.Domain
{
    public class RoleActivatedDomainEvent : DomainEventBase, MediatR.INotification
    {
        public string Code { get; set; }
        public RoleActivatedDomainEvent()
        {
            base.App = null;
            base.Module = null;
            base.Model = "Role";
            base.Action = "Activated";
            base.User = null;
        }
    }
}
