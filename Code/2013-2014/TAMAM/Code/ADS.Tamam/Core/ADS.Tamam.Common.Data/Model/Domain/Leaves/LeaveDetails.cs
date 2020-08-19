using ADS.Common.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ADS.Tamam.Common.Data.Model.Domain.Leaves
{
    [Serializable]
    public class LeaveDetails : IXSerializable
    {
        public Leave Leave { get; set; }
        public double BalanceBefore { get; set; }
        public double BalanceAfter { get; set; }
    }
}
