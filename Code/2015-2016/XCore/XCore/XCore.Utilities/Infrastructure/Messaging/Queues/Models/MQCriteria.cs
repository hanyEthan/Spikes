using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XCore.Utilities.Infrastructure.Messaging.Queues.Models.Enums;

namespace XCore.Utilities.Infrastructure.Messaging.Queues.Models
{
    public class MQCriteria
    {
        public int? MaxRetialsCount { get; set; }
        public List<MQMessageStatus> Statuses { get; set; }
        public string Type { get; set; }
    }
}
