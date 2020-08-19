using XCore.Framework.Infrastructure.Entities.Repositories.Models;

namespace XCore.Services.Notifications.Core.Models.Domain
{
    public class MessageTemplateKey :Entity<int>
    {
        #region props.

        public string Key { get; set; }
        public string Description { get; set; }
        public int MessageTemplateId { get; set; }
        public virtual MessageTemplate MessageTemplate { get; set; }

        #endregion
    }
}
