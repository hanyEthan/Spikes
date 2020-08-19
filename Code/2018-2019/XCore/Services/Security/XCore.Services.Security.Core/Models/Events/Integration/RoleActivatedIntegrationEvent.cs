using XCore.Framework.Infrastructure.Entities.Events.Domain;
using XCore.Services.IntegrationModels.Security;

namespace XCore.Services.Security.Core.Models.Events.Integration
{
    public class RoleActivatedIntegrationEvent : DomainEventBase,IRoleActivatedIntegrationEvent
    {
        public string Code { get; set; }
    }
}
