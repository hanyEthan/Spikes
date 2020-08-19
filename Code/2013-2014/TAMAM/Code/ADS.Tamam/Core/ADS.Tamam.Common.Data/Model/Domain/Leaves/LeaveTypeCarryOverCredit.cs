using System;
using ADS.Common.Contracts;
using ADS.Common.Models.Domain;

namespace ADS.Tamam.Common.Data.Model.Domain.Leaves
{
    [Serializable]
    public class LeaveTypeCarryOverCredit : IXSerializable
    {
        public Guid Id { get; set; }

        public Guid LeaveCreditId { get; set; }
        public LeaveCredit LeaveCredit { get; set; }

        public int LeaveTypeId { get; set; }
        public DetailCode LeaveType { get; set; }

        public double Credit { get; set; }
        public bool OverridePolicies { get; set; }

        #region cst ...

        public LeaveTypeCarryOverCredit()
        {
        }
        public LeaveTypeCarryOverCredit( Guid id , Guid leaveCreditId , int leaveTypeId , double credit , bool overridePolicies )
        {
            Id = id;
            LeaveCreditId = leaveCreditId;
            LeaveTypeId = leaveTypeId;
            Credit = credit;
            OverridePolicies = overridePolicies;
        }

        #endregion
    }
}
