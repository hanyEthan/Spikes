using System;
using System.Collections.Generic;
using System.Text;
using XCore.Framework.Infrastructure.Entities.Events.Domain;
using XCore.Services.IntegrationModels.Person;

namespace XCore.Services.Personnel.Models.DTO.Events.Integration
{
    public class PersonUpdatedIntegrationEvent : DomainEventBase,IPersonUpdatedIntegrationEvent
    {
        public int PersonId { get ; set ; }
    }
}
