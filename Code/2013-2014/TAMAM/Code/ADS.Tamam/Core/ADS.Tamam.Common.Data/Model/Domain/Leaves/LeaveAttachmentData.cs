using System;
using ADS.Common.Contracts;

namespace ADS.Tamam.Common.Data.Model.Domain.Leaves
{
    [Serializable]
    public class LeaveAttachmentData : IXSerializable
    {
        public Guid Id { get; set; }
        public byte[] AttachedDocument { get; set; }
    }
}
