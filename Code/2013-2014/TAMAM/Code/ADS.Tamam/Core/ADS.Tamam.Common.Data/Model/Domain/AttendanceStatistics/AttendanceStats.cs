using System;
using System.Collections.Generic;
using ADS.Common.Contracts;

namespace ADS.Tamam.Common.Data.Model.Domain.AttendanceStatistics
{
    [Serializable]
    public class AttendanceStats : IXSerializable
    {
        public Guid? DepartmentId { get; set; }
        public DateTime? AttendanceDate { get; set; }

        public int? LateCount { get; set; }
        public int? LeftEarlyCount { get; set; }
        public int? LeftLateCount { get; set; }
        public int? LeaveCount { get; set; }
        public int? AbsentCount { get; set; }
        public int? ComeOnTimeCount { get; set; }
        public int? MissedPunchesCount { get; set; }
        public int? WorkedLessCount { get; set; }
        public int? WorkedMoreCount { get; set; }
        public int? OvertimeCount { get; set; }

        private Dictionary<string , int?> _Stats;
        [XDontSerialize] public Dictionary<string , int?> Stats
        {
            get
            {
                if (_Stats != null) return _Stats;

                _Stats = new Dictionary<string, int?>
                {
                    {Resources.Culture.Attendance.Late, LateCount},
                    {Resources.Culture.Attendance.LeftEarly, LeftEarlyCount},
                    {Resources.Culture.Attendance.LeftLate, LeftLateCount},
                    {Resources.Culture.Attendance.Leave, LeaveCount},
                    {Resources.Culture.Attendance.Absent, AbsentCount},
                    {Resources.Culture.Attendance.ComeOnTime, ComeOnTimeCount},
                    {Resources.Culture.Attendance.MissedPunches, MissedPunchesCount},
                    {Resources.Culture.Attendance.WorkedLess, WorkedLessCount},
                    {Resources.Culture.Attendance.WorkedMore, WorkedMoreCount},
                    {Resources.Culture.Attendance.Overtime, OvertimeCount}
                };

                return _Stats;
            }
        }
    }
}
