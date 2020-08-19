using System.Collections.Generic;
using XCore.Framework.Infrastructure.Entities.Events.Domain;
using XCore.Services.Docs.Models.Models.Docs;
using XCore.Services.Notifications.Models.Models.Notifications.MessageTemplate;

namespace XCore.Services.Docs.Api.Models.Events.Domain
{
    public class DocumentCreatedDomainEvent : DomainEventBase, MediatR.INotification
    {
        #region props.

        public List<DocumentCreatedMetaData> Documents { get; set; }

        #endregion
        #region cst.

        public DocumentCreatedDomainEvent()
        {
            base.App = null;
            base.Module = null;
            base.Model = "DocumentCreated";
            base.Action = "Created";
            base.User = null;
        }

        #endregion

    }
}
