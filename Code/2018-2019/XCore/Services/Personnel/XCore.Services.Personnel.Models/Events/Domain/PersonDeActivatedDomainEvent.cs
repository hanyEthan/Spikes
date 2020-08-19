using System;
using System.Collections.Generic;
using System.Text;
using XCore.Framework.Infrastructure.Entities.Events.Domain;

namespace XCore.Services.Personnel.Models.Events.Domain
{
   public class PersonDeActivatedDomainEvent : DomainEventBase, MediatR.INotification
    {
        public string Code { get; set; }
        public PersonDeActivatedDomainEvent()
        {
            base.App = null;
            base.Module = null;
            base.Model = "Person";
            base.Action = "Deactivated";
            base.User = null;
        }
    }
}
