using System;

namespace XCore.Utilities.Infrastructure.Messaging.Pools.Models
{
    public class PoolMessageSearchCriteria
    {
        public string AppId { get; set; }
        public DateTime? CreatedDate { get; set; }
        public string MessageType { get; set; }
        public bool PopMessages { get; set; }
    }
}
