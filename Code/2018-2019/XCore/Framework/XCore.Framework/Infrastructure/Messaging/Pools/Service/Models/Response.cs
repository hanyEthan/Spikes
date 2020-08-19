using System.Runtime.Serialization;

namespace XCore.Framework.Infrastructure.Messaging.Pools.Service.Models
{
    [DataContract]
    public class Response<T>
    {
        [DataMember] public int state { get; set; }
        [DataMember] public T result { get; set; }
    }
}
