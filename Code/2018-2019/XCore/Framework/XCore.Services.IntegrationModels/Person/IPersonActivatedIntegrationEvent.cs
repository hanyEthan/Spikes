using System;
using System.Collections.Generic;
using System.Text;
using XCore.Framework.Infrastructure.Entities.Events.Domain;

namespace XCore.Services.IntegrationModels.Person
{
   public interface IPersonActivatedIntegrationEvent : IDomainEvent
    {
         string Code { get; set; }
    }
}
