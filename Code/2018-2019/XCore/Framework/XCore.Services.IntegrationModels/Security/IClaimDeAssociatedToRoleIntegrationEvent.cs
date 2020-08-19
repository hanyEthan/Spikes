using System;
using System.Collections.Generic;
using System.Text;
using XCore.Framework.Infrastructure.Entities.Events.Domain;

namespace XCore.Services.IntegrationModels.Security
{
    public interface IClaimDeAssociatedToRoleIntegrationEvent : IDomainEvent
    {
        int RoleId { get; set; }
        List<int> Claims { get; set; }
    }
}