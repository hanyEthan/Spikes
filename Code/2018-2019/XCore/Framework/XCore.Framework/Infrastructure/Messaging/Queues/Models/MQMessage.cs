using XCore.Framework.Infrastructure.Entities.Repositories.Models;
using XCore.Framework.Infrastructure.Messaging.Queues.Models.Enums;

namespace XCore.Framework.Infrastructure.Messaging.Queues.Models
{
    public class MQMessage : Entity<int>
    {
        #region props.

        public string Type { get; set; }
        public MQMessageStatus Status { get; set; }
        public MQMessagePriority Priority { get; set; }
        public MQMessageComplexity Complexity { get; set; }
        public string TargetCode { get; set; }
        public string TargetType { get; set; }
        public string ContentType { get; set; }
        public string SubscribersTokens { get; set; }
        public int RetrialsCounter { get; set; }

        #endregion
    }
}
