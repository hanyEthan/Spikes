using XCore.Framework.Infrastructure.Entities.Events.Domain;

namespace XCore.Services.IntegrationModels.Security
{
    public interface IRoleActivatedIntegrationEvent : IDomainEvent
    {
        string Code { get; set; }
    }
}
