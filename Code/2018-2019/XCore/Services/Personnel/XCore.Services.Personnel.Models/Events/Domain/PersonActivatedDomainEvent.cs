using System;
using System.Collections.Generic;
using System.Text;
using XCore.Framework.Infrastructure.Entities.Events.Domain;

namespace XCore.Services.Personnel.Models.Events.Domain
{
   public class PersonActivatedDomainEvent : DomainEventBase, MediatR.INotification
    {
        public string Code { get; set; }
        public PersonActivatedDomainEvent()
        {
            base.App = null;
            base.Module = null;
            base.Model = "Person";
            base.Action = "Activated";
            base.User = null;
        }
    }
}
