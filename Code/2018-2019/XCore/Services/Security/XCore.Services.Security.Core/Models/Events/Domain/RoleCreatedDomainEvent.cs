using XCore.Framework.Infrastructure.Entities.Events.Domain;

namespace XCore.Services.Security.Core.Models.Events.Domain
{
    public class RoleCreatedDomainEvent : DomainEventBase, MediatR.INotification
    {
        public int RoleId { get; set; }
        public RoleCreatedDomainEvent()
        {
            base.App = null;
            base.Module = null;
            base.Model = "Role";
            base.Action = "Created";
            base.User = null;
        }
    }
}
