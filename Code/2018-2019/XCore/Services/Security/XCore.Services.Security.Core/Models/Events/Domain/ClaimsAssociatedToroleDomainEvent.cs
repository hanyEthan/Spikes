using System;
using System.Collections.Generic;
using System.Text;
using XCore.Framework.Infrastructure.Entities.Events.Domain;

namespace XCore.Services.Security.Core.Models.Events.Domain
{
   public class ClaimsAssociatedToroleDomainEvent : DomainEventBase, MediatR.INotification
    {
        public int RoleId { get; set; }
        public List<int> Claims { get; set; }
        #region cst.

        public ClaimsAssociatedToroleDomainEvent()
        {
            base.App = null;
            base.Module = null;
            base.Model = "Claims";
            base.Action = "AssociatedToRole";

            base.User = null;
        }

        #endregion
    }
}
