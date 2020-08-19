using System;
using ADS.Common.Contracts;

namespace ADS.Tamam.Common.Data.Model.Domain.Leaves
{
    [Serializable]
    public class ExcuseCredit : IXSerializable
    {
        public Guid PersonId { get; set; }
        public int MonthNo { get; set; }

        public int MaxExcusesPerMonth { get; set; }
        public double MaxHoursPerMonth { get; set; }

        public int ConsumedExcusesPerMonth { get; set; }
        public double ConsumedHoursPerMonth { get; set; }

        #region Formatted

        [XDontSerialize]public string BalanceFormatted
        {
            get { return MaxHoursPerMonth.ToString( "F2" ); }
        }
        [XDontSerialize]public string ConsumedFormatted
        {
            get { return ConsumedHoursPerMonth.ToString( "F2" ); }
        }
        [XDontSerialize]public string RemainingFormatted
        {
            get { return ( MaxHoursPerMonth - ConsumedHoursPerMonth ).ToString( "F2" ); }
        }

        #endregion
    }
}