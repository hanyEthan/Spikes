using System;
using ADS.Common.Contracts;
using ADS.Tamam.Common.Data.Model.Enums;

namespace ADS.Tamam.Common.Data.Model.Domain.Leaves
{
    [Serializable]
    public class LeaveReview : IXSerializable
    {
        public Guid LeaveId { get; set; }
        public ReviewLeaveStatus Status { get; set; }
        public Guid ReviewerId { get; set; }
        public string Comment { get; set; }
        public Guid RequestId { get; set; }
    }
}
