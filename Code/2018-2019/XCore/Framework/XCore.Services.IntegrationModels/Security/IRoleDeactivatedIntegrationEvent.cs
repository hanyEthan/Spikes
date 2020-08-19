using XCore.Framework.Infrastructure.Entities.Events.Domain;

namespace XCore.Services.IntegrationModels.Security
{
    public interface IRoleDeactivatedIntegrationEvent : IDomainEvent
    {
        string Code { get; set; }
    }
}
