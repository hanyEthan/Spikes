using System;
using System.Collections.Generic;
using ADS.Common.Models.Domain;
using ADS.Tamam.Common.Data.Model.Domain.Personnel;
using ADS.Common.Contracts;

namespace ADS.Tamam.Common.Data.Model.Domain.Leaves
{
    [Serializable]
    public class LeaveCredit : IXSerializable
    {
        public Guid Id { get; set; }

        public Guid PersonId { get; set; }
        public Person Person { get; set; }

        public DateTime EffectiveYearStart { get; set; }

        public int LeaveCreditStatusId { get; set; }
        public bool IsDeleted { get; set; }

        public DetailCode LeaveCreditStatus { get; set; }
        public IList<LeaveTypeCredit> LeaveTypeCredits { get; set; }
        public IList<LeaveTypeCarryOverCredit> LeaveTypeCarryOverCredits { get; set; }

        #region cst ...

        public LeaveCredit()
        {
            LeaveTypeCredits = new List<LeaveTypeCredit>();
            LeaveTypeCarryOverCredits = new List<LeaveTypeCarryOverCredit>();
        }
        public LeaveCredit(Guid id, Guid personId, DateTime effectiveYearStart,int leaveCreditStatusId)
        {
            Id = id;
            PersonId = personId;
            EffectiveYearStart = effectiveYearStart;
            LeaveCreditStatusId = leaveCreditStatusId;
            LeaveTypeCredits = new List<LeaveTypeCredit>();
            LeaveTypeCarryOverCredits = new List<LeaveTypeCarryOverCredit>();
        }

        #endregion
    }
}