using System.Collections.Generic;
using XCore.Framework.Infrastructure.Entities.Repositories.Models;

namespace XCore.Services.Notifications.Core.Models.Domain
{
    public class MessageTemplate : Entity<int>
    {
        #region props. 

        public string AppId { get; set; }
        public string ModuleId { get; set; }
        public string Title { get; set; }
        public string Body { get; set; }
        public virtual List<MessageTemplateKey> Keys { get; set; }
        public virtual List<MessageTemplateAttachment> Attachments { get; set; }

        #endregion
        #region cst.

        public MessageTemplate ()
        {
            this.Keys = new List<MessageTemplateKey>();
            this.Attachments = new List<MessageTemplateAttachment>();
        }

        #endregion
    }
}
