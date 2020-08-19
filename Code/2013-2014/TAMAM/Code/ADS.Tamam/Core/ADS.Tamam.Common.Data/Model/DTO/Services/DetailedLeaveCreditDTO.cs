using ADS.Common.Contracts;
using ADS.Common.Models.Domain;
using ADS.Tamam.Common.Data.Model.Domain.Leaves;
using ADS.Tamam.Common.Data.Model.Domain.Personnel;
using ADS.Tamam.Common.Data.Model.Domain.Policy.Specialized;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ADS.Tamam.Common.Data.Model.DTO.Services
{
    [Serializable]
    public class DetailedLeaveCreditDTO : IXSerializable
    {
        public Person Person { get; set; }
        public DateTime EffectiveDate { get; set; }

        public List<DetailedLeaveTypeCreditDTO> DetailedCredits { get; set; }

        public DetailedLeaveCreditDTO() { DetailedCredits = new List<DetailedLeaveTypeCreditDTO>(); }
        public DetailedLeaveCreditDTO( LeaveCredit Credit , LeavePreCredit PreCredit )
        {
            DetailedCredits = new List<DetailedLeaveTypeCreditDTO>();

            if ( Credit == null || Credit.LeaveTypeCredits == null || Credit.LeaveTypeCredits.Count == 0 ) return;

            this.Person = Credit == null ? null : Credit.Person;
            this.EffectiveDate = Credit == null ? DateTime.MinValue : Credit.EffectiveYearStart;

            foreach ( var TypCredit in Credit.LeaveTypeCredits )
            {
                var leaveTypeId = ( int ) TypCredit.LeaveType.Id;

                var LVTypeCarryOver = Credit == null ? null : Credit.LeaveTypeCarryOverCredits.FirstOrDefault( x => x.LeaveTypeId == leaveTypeId );
                var LVTypePreCredit = PreCredit == null ? null : PreCredit.LeaveTypePreCredits.FirstOrDefault( x => x.LeaveTypeId == leaveTypeId );

                var leaveType = TypCredit.LeaveType;

                // CarryOver
                double CarryOverBalance = LVTypeCarryOver == null ? 0 : LVTypeCarryOver.Credit;

                // PreCredit
                double PreCreditBalance = LVTypePreCredit == null ? 0 : LVTypePreCredit.Balance;
                double PreCreditConsumed = LVTypePreCredit == null ? 0 : LVTypePreCredit.Consumed;

                // Credit
                double CreditBalance = ( TypCredit.Amount + CarryOverBalance ) - PreCreditBalance;
                double CreditConsumed = TypCredit.Consumed - PreCreditConsumed;

                DetailedCredits.Add( new DetailedLeaveTypeCreditDTO( leaveTypeId , leaveType , CreditBalance , CreditConsumed , CarryOverBalance , PreCreditBalance , PreCreditConsumed ) );
            }
        }
        public DetailedLeaveCreditDTO( LeaveCredit Credit , LeavePreCredit PreCredit , List<int> leaveTypes )
        {
            DetailedCredits = new List<DetailedLeaveTypeCreditDTO>();

            if ( Credit == null || Credit.LeaveTypeCredits == null || Credit.LeaveTypeCredits.Count == 0 ) return;

            this.Person = Credit == null ? null : Credit.Person;
            this.EffectiveDate = Credit == null ? DateTime.MinValue : Credit.EffectiveYearStart;

            foreach ( var TypCredit in Credit.LeaveTypeCredits )
            {
                var leaveTypeId = ( int ) TypCredit.LeaveType.Id;
                if ( leaveTypes != null && leaveTypes.Count > 0 && !leaveTypes.Contains( leaveTypeId ) ) continue;

                var LVTypeCarryOver = Credit == null ? null : Credit.LeaveTypeCarryOverCredits.FirstOrDefault( x => x.LeaveTypeId == leaveTypeId );
                var LVTypePreCredit = PreCredit == null ? null : PreCredit.LeaveTypePreCredits.FirstOrDefault( x => x.LeaveTypeId == leaveTypeId );

                var leaveType = TypCredit.LeaveType;

                // CarryOver
                double CarryOverBalance = LVTypeCarryOver == null ? 0 : LVTypeCarryOver.Credit;

                // PreCredit
                double PreCreditBalance = LVTypePreCredit == null ? 0 : LVTypePreCredit.Balance;
                double PreCreditConsumed = LVTypePreCredit == null ? 0 : LVTypePreCredit.Consumed;

                // Credit
                double CreditBalance = ( TypCredit.Amount + CarryOverBalance ) - PreCreditBalance;
                double CreditConsumed = TypCredit.Consumed - PreCreditConsumed;

                DetailedCredits.Add( new DetailedLeaveTypeCreditDTO( leaveTypeId , leaveType , CreditBalance , CreditConsumed , CarryOverBalance , PreCreditBalance , PreCreditConsumed ) );
            }
        }
        public DetailedLeaveCreditDTO( List<LeavePolicy> LeavePolicies , LeaveCredit Credit , LeavePreCredit PreCredit )
        {
            DetailedCredits = new List<DetailedLeaveTypeCreditDTO>();

            if ( LeavePolicies == null || LeavePolicies.Count == 0 ) return;

            this.Person = Credit == null ? null : Credit.Person;
            this.EffectiveDate = Credit == null ? DateTime.MinValue : Credit.EffectiveYearStart;

            foreach ( var LVPolicy in LeavePolicies )
            {
                var leaveTypeId = ( int ) LVPolicy.LeaveType.Value;

                var LVTypeCredit = Credit == null ? null : Credit.LeaveTypeCredits.FirstOrDefault( x => x.LeaveTypeId == leaveTypeId );
                var LVTypeCarryOver = Credit == null ? null : Credit.LeaveTypeCarryOverCredits.FirstOrDefault( x => x.LeaveTypeId == leaveTypeId );
                var LVTypePreCredit = PreCredit == null ? null : PreCredit.LeaveTypePreCredits.FirstOrDefault( x => x.LeaveTypeId == leaveTypeId );

                var leaveType = LVTypeCredit == null ? null : LVTypeCredit.LeaveType;

                // CarryOver
                double CarryOverBalance = LVTypeCarryOver == null ? 0 : LVTypeCarryOver.Credit;

                // PreCredit
                double PreCreditBalance = LVTypePreCredit == null ? 0 : LVTypePreCredit.Balance;
                double PreCreditConsumed = LVTypePreCredit == null ? 0 : LVTypePreCredit.Consumed;

                // Credit
                double CreditBalance = ( LVTypeCredit.Amount + CarryOverBalance ) - PreCreditBalance;
                double CreditConsumed = LVTypeCredit.Consumed - PreCreditConsumed;

                DetailedCredits.Add( new DetailedLeaveTypeCreditDTO( leaveTypeId , leaveType , CreditBalance , CreditConsumed , CarryOverBalance , PreCreditBalance , PreCreditConsumed ) );
            }
        }
    }

    [Serializable]
    public class DetailedLeaveTypeCreditDTO : IXSerializable
    {
        public int LeaveTypeId { get; set; }
        public DetailCode LeaveType { get; set; }

        public double CreditBalance { get; set; }
        public double CreditConsumed { get; set; }

        public double CarryOverBalance { get; set; }

        public double PreCreditBalance { get; set; }
        public double PreCreditConsumed { get; set; }

        public double TotalBalance { get { return CreditBalance + PreCreditBalance; } }
        public double TotalConsumed { get { return CreditConsumed + PreCreditConsumed; } }

        public DetailedLeaveTypeCreditDTO() { }
        public DetailedLeaveTypeCreditDTO( int leaveTypeId , DetailCode leaveType , double creditBalance , double creditConsumed , double carryOverBalance , double preCreditBalance , double preCreditConsumed )
        {
            this.LeaveTypeId = leaveTypeId;
            this.LeaveType = leaveType;

            this.CreditBalance = creditBalance;
            this.CreditConsumed = creditConsumed;

            this.CarryOverBalance = carryOverBalance;

            this.PreCreditBalance = preCreditBalance;
            this.PreCreditConsumed = preCreditConsumed;
        }
    }
}