using XCore.Framework.Infrastructure.Entities.Events.Domain;

namespace XCore.Services.Personnel.Models.Events.Domain
{
    public class PersonCreatedDomainEvent : DomainEventBase, MediatR.INotification
    {
        public int PersonId { get; set; }
        public PersonCreatedDomainEvent()
        {
            base.App = null;
            base.Module = null;
            base.Model = "Person";
            base.Action = "Created";
            base.User = null;
        }
    }
}
