using XCore.Framework.Infrastructure.Entities.Events.Domain;

namespace XCore.Services.Security.Core.Models.Events.Domain
{
    public class RoleDeactivatedDomainEvent : DomainEventBase, MediatR.INotification
    {
        public string Code { get; set; }
        public RoleDeactivatedDomainEvent()
        {
            base.App = null;
            base.Module = null;
            base.Model = "Role";
            base.Action = "Deactivated";
            base.User = null;
        }
    }
}
