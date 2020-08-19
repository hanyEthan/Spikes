using System;
using System.Runtime.Serialization;
using XCore.Framework.Infrastructure.Messaging.Pools.Models.Enums;

namespace XCore.Framework.Infrastructure.Messaging.Pools.Service.Models
{
    [DataContract]
    public class MPoolMessageDataContract
    {
        [DataMember] public string Code { get; set; }
        [DataMember] public string MessageContent { get; set; }
        [DataMember] public string AppId { get; set; }
        [DataMember] public string MessageType { get; set; }
        [DataMember] public string Periority { get; set; }
        [DataMember] public string Size { get; set; }
        [DataMember] public PoolMessageStatus? Status { get; set; }
        [DataMember] public DateTime? ModifiedDate { get; set; }
        [DataMember] public string CreatedBy { get; set; }
        [DataMember] public string ModifiedBy { get; set; }
        [DataMember] public string MetaData { get; set; }
        [DataMember] public bool IsActive { get; set; } = true;
        [DataMember] public DateTime? CreatedDate { get; set; }
    }
}
