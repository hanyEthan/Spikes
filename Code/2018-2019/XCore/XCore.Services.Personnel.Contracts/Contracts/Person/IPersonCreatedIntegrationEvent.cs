using XCore.Framework.Infrastructure.Entities.Events.Domain;

namespace XCore.Services.Personnel.Contracts.Contracts
{
    public interface IPersonCreatedIntegrationEvent : IDomainEvent
    {
         int PersonId { get; set; }
    }
}
