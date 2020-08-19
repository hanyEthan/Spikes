using System.Collections.Generic;
using XCore.Framework.Infrastructure.Entities.Events.Domain;

namespace XCore.Services.Security.Models.Contracts
{
    public interface IClaimAssociatedToRoleIntegrationEvent : IDomainEvent
    {
        int RoleId { get; set; }
        List<int> Claims { get; set; }
    }
}
