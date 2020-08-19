using System.Collections.Generic;
using XCore.Framework.Infrastructure.Messaging.Queues.Models.Enums;

namespace XCore.Framework.Infrastructure.Messaging.Queues.Models
{
    public class MQCriteria
    {
        public int? MaxRetialsCount { get; set; }
        public List<MQMessageStatus> Statuses { get; set; }
        public string Type { get; set; }
    }
}
