using XCore.Framework.Infrastructure.Entities.Events.Domain;
using XCore.Services.IntegrationModels.Person;

namespace XCore.Services.Personnel.Models.DTO.Events.Integration
{
    public class PersonCreatedIntegrationEvent: DomainEventBase, IPersonCreatedIntegrationEvent
    {
        public int PersonId { get; set; }

    }
}
