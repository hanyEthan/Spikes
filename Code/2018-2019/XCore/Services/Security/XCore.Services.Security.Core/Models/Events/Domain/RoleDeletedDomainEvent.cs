using XCore.Framework.Infrastructure.Entities.Events.Domain;

namespace XCore.Services.Security.Core.Models.Events.Domain
{
    public class RoleDeletedDomainEvent : DomainEventBase, MediatR.INotification
    {
        public string Code { get; set; }
        public RoleDeletedDomainEvent()
        {
            base.App = null;
            base.Module = null;
            base.Model = "Role";
            base.Action = "Deleted";
            base.User = null;
        }
    }
}
