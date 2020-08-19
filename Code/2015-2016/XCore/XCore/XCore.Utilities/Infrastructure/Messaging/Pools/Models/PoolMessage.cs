using System;
using XCore.Utilities.Infrastructure.Entities.Repositories.Models;
using XCore.Utilities.Infrastructure.Messaging.Pools.Models.Enums;

namespace XCore.Utilities.Infrastructure.Messaging.Pools.Models
{
    public class PoolMessage : Entity<int>
    {
        public new string Code { get; set; } = Guid.NewGuid().ToString();
        public string MessageContent { get; set; }
        public string AppId { get; set; }
        public string MessageType { get; set; }
        public string Periority { get; set; }
        public string Size { get; set; }
        public PoolMessageStatus? Status { get; set; }
    }
}
