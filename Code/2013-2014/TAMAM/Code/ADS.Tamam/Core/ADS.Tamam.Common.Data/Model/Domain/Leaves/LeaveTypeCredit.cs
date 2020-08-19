using System;
using ADS.Common.Contracts;
using ADS.Common.Models.Domain;

namespace ADS.Tamam.Common.Data.Model.Domain.Leaves
{
    [Serializable]
    public class LeaveTypeCredit : IXSerializable
    {
        public Guid Id { get; set; }

        public Guid PolicyId { get; set; }
        public Policy.Policy Policy { get; set; }

        public int LeaveTypeId { get; set; }
        public DetailCode LeaveType { get; set; }

        public Guid LeaveCreditId { get; set; }
        public LeaveCredit LeaveCredit { get; set; }

        public double Amount { get; set; }
        public double Consumed { get; set; }

        [XDontSerialize]
        public double ProgressiveAmount
        {
            get
            {
                if ( LeaveCredit.EffectiveYearStart.Date > DateTime.Today.Date ) return 0;
                else
                {
                    var percent = ( DateTime.Today.Date - LeaveCredit.EffectiveYearStart.Date ).TotalDays / 365;
                    return Math.Round( Amount * percent, 2 );
                }
            }
        }

        #region cst ...

        public LeaveTypeCredit()
        {

        }
        public LeaveTypeCredit( Guid id , Guid leaveCreditId , Guid policyId , int leaveType , double amount , double consumed )
        {
            Id = id;
            LeaveCreditId = leaveCreditId;
            LeaveTypeId = leaveType;
            Consumed = consumed;

            PolicyId = policyId;
            Amount = amount;
        }

        #endregion
    }
}
