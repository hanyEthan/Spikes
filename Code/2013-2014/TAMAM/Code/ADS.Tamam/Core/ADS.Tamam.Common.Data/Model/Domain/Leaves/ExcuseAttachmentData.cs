using System;
using ADS.Common.Contracts;

namespace ADS.Tamam.Common.Data.Model.Domain.Leaves
{
    [Serializable]
    public class ExcuseAttachmentData : IXSerializable
    {
        public Guid Id { get; set; }
        public byte[] AttachedDocument { get; set; }
    }
}
