using XCore.Framework.Infrastructure.Entities.Events.Domain;
using XCore.Services.IntegrationModels.Security;

namespace XCore.Services.Security.Core.Models.Events.Integration
{
    public class RoleDeletedIntegrationEvent : DomainEventBase, IRoleDeletedIntegrationEvent
    {
        public string Code { get; set; }
    }
}
