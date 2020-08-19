using XCore.Framework.Infrastructure.Entities.Events.Domain;

namespace XCore.Services.Security.Models.Contracts
{
    public interface IRoleDeactivatedIntegrationEvent : IDomainEvent
    {
        int RoleId { get; set; }
    }
}
