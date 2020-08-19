using System;
using ADS.Common.Contracts;

namespace ADS.Tamam.Common.Data.Model.Domain.AttendanceStatistics
{
    [Serializable]
    public class AttendanceDivisionSummary :IXSerializable
    {
        public Guid Id { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public string NameCultureVarient { get; set; }

        public int PersonnelCount { get; set; }
        public int AbsentCount { get; set; }
        public int InLateCount { get; set; }
        public int EarlyLeaveCount { get; set; }
        public int OnTimeCount { get; set; }
        public int PersonnelLeavesCount { get; set; }
        public int PersonnelSickLeavesCount { get; set; }

        public int TotalWorkingDaysCount { get; set; }
        public int TotalLeaveDaysCount { get; set; }
        public int TotalSickLeaveDaysCount { get; set; }
    }
}
