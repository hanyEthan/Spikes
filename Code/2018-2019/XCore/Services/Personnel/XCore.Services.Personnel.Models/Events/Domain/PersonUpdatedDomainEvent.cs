using System;
using System.Collections.Generic;
using System.Text;
using XCore.Framework.Infrastructure.Entities.Events.Domain;

namespace XCore.Services.Personnel.Models.Events.Domain
{
   public class PersonUpdatedDomainEvent : DomainEventBase, MediatR.INotification
    {
        public int PersonId { get; set; }
        public PersonUpdatedDomainEvent()
        {
            base.App = null;
            base.Module = null;
            base.Model = "Person";
            base.Action = "Updated";
            base.User = null;
        }
    }
}
