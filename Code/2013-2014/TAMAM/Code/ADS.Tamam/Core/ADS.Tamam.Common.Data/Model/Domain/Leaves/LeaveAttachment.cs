using System;
using ADS.Common.Contracts;

namespace ADS.Tamam.Common.Data.Model.Domain.Leaves
{
    [Serializable]
    public class LeaveAttachment : IXSerializable
    {
        public Guid Id { get; set; }
        public Guid LeaveId { get; set; }
        public string Name { get; set; }
        public string Extension { get; set; }
        public byte[] AttachedDocument { get; set; }

        public Leave Leave { get; set; }
    }
}
