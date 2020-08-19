using System;
using System.Runtime.Serialization;

namespace XCore.Utilities.Infrastructure.Messaging.Pools.Service.Models
{
    [DataContract]
    public class MPoolCriteriaDataContract
    {
        [DataMember] public string AppId { get; set; }
        [DataMember] public DateTime? CreatedDate { get; set; }
        [DataMember] public string MessageType { get; set; }
        [DataMember] public bool PopMessages { get; set; }
    }
}
