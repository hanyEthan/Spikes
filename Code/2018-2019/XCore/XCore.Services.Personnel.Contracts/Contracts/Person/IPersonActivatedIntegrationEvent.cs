using System;
using System.Collections.Generic;
using System.Text;
using XCore.Framework.Infrastructure.Entities.Events.Domain;

namespace XCore.Services.Personnel.Contracts.Contracts.Person
{
   public interface IPersonActivatedIntegrationEvent : IDomainEvent
    {
         string Code { get; set; }
    }
}
