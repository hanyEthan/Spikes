using System;
using ADS.Common.Contracts;

namespace ADS.Tamam.Common.Data.Model.Domain.Leaves
{
    [Serializable]
    public class ExcuseAttachment : IXSerializable
    {
        public Guid Id { get; set; }
        public Guid ExcuseId { get; set; }
        public string Name { get; set; }
        public string Extension { get; set; }
        public byte[] AttachedDocument { get; set; }

        public Excuse Excuse { get; set; }
    }
}
