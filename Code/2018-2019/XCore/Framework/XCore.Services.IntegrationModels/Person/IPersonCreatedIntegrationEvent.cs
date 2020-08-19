using XCore.Framework.Infrastructure.Entities.Events.Domain;

namespace XCore.Services.IntegrationModels.Person
{
    public interface IPersonCreatedIntegrationEvent : IDomainEvent
    {
         int PersonId { get; set; }
    }
}
