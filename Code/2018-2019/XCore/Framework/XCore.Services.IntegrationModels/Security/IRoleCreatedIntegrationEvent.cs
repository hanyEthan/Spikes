using XCore.Framework.Infrastructure.Entities.Events.Domain;

namespace XCore.Services.IntegrationModels.Security
{
    public interface IRoleCreatedIntegrationEvent : IDomainEvent
    {
        int RoleId { get; set; }
    }
}
