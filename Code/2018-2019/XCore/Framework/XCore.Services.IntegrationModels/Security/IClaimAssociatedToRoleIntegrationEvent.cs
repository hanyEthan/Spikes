using System.Collections.Generic;
using XCore.Framework.Infrastructure.Entities.Events.Domain;

namespace XCore.Services.IntegrationModels.Security
{
    public interface IClaimAssociatedToRoleIntegrationEvent : IDomainEvent
    {
        int RoleId { get; set; }
        List<int> Claims { get; set; }
    }
}
