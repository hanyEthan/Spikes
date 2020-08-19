using System;
using System.Collections.Generic;
using ADS.Common.Contracts;

namespace ADS.Tamam.Common.Data.Model.Domain.Leaves
{
    [Serializable]
    public class LeavePreCredit : IXSerializable
    {
        public Guid Id { get; set; }
        public Guid PersonId { get; set; }
        public DateTime EffectiveYearStart { get; set; }
        public bool IsDeleted { get; set; }

        public IList<LeaveTypePreCredit> LeaveTypePreCredits { get; set; }

        public LeavePreCredit()
        {
            LeaveTypePreCredits = new List<LeaveTypePreCredit>();
        }
    }
}