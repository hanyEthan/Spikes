using System;
using ADS.Common.Contracts;
using ADS.Common.Models.Domain;

namespace ADS.Tamam.Common.Data.Model.Domain.Leaves
{
    [Serializable]
    public class LeaveTypePreCredit : IXSerializable
    {
        public Guid Id { get; set; }
        public Guid PreCreditId { get; set; }
        
        public int LeaveTypeId { get; set; }
        public DetailCode LeaveType { get; set; }

        public double Consumed { get; set; }
        public double Balance { get; set; }

        public LeavePreCredit PreCredit { get; set; }
    }
}