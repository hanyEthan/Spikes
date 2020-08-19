using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ADS.Tamam.Common.Data.Model.Domain.Terminal
{
    public class TerminalStats
    {
        public string TerminalId { get; set; }
        public DateTime EventDate { get; set; }
        public int? AbsentCount { get; set; }
        public int? LateCount { get; set; }
        public int? LeaveCount { get; set; }
        public int? LeftEarlyCount { get; set; }
        public int? LeftLateCount { get; set; }
        public int? ComeOnTimeCount { get; set; }
        public int? MissedPunchesCount { get; set; }
        public int? WorkedLessCount { get; set; }
        public int? WorkedMoreCount { get; set; }
        public int? OvertimeCount { get; set; }
        public int? CurrentlyInCount { get; set; }
        public string LocationName { get; set; }
        public double? Longitude { get; set; }
        public double? Latitude { get; set; }
    }
}
