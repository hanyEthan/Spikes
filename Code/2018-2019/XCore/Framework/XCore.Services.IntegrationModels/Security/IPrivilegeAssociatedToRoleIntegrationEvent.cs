using System.Collections.Generic;
using XCore.Framework.Infrastructure.Entities.Events.Domain;

namespace XCore.Services.IntegrationModels.Security
{
    public interface IPrivilegeAssociatedToRoleIntegrationEvent : IDomainEvent
    {
        int RoleId { get; set; }
        List<int> Privileges { get; set; }
    }
}
