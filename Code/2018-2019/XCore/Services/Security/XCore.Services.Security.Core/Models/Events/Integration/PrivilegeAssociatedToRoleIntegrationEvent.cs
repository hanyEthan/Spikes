using System.Collections.Generic;
using XCore.Framework.Infrastructure.Entities.Events.Domain;
using XCore.Services.IntegrationModels.Security;

namespace XCore.Services.Security.Core.Models.Events.Integration
{
    public class PrivilegeAssociatedToRoleIntegrationEvent : DomainEventBase, IPrivilegeAssociatedToRoleIntegrationEvent
    {
        public int RoleId { get ; set ; }
        public List<int> Privileges { get ; set ; }
    }
}
