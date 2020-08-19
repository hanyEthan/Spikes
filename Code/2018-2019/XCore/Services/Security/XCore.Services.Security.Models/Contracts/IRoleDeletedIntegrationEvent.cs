using XCore.Framework.Infrastructure.Entities.Events.Domain;

namespace XCore.Services.Security.Models.Contracts
{
    public interface IRoleDeletedIntegrationEvent : IDomainEvent
    {
        int RoleId { get; set; }
    }
}
