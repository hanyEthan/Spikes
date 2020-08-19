using System;
using ADS.Common.Contracts;

namespace ADS.Tamam.Common.Data.Model.Domain.AttendanceStatistics
{
    [Serializable]
    public class AttendanceDivisionLostWorkingHoursStatistics : IXSerializable
    {
        public Guid Id { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public string NameCultureVarient { get; set; }

        public int PersonnelCount { get; set; }
        public int ConsideredPersonnelCount { get; set; }

        public int TotalWorkingDaysCount { get; set; }
        public int TotalActualWorkingHours { get; set; }
        public int TotalExpectedWorkingHours { get; set; }

        public int TotalAbsentHours { get; set; }
        public int TotalSickLeaveHours { get; set; }
        public int TotalLateHours { get; set; }
        public int TotalEarlyLeaveHours { get; set; }
        public int TotalLostHours { get; set; }
    }
}