using System;
using System.Collections.Generic;
using ADS.Common.Contracts;

namespace ADS.Tamam.Common.Data.Model.Domain.Attendance
{
    [Serializable]
    public class AttendanceRawDataFilters : IXSerializable
    {
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public bool? OnlyRawConsiderAsAttendance { get; set; }

        public List<Guid> Personnel { get; set; }
        public List<Guid> Departments { get; set; }
        public List<Guid> ScheduleEvents { get; set; }

        public AttendanceRawDataFilters()
        {
            Personnel = new List<Guid>();
            Departments = new List<Guid>();
            ScheduleEvents = new List<Guid>();
        }

        public override string ToString()
        {
            var startDateToken = StartDate.HasValue ? StartDate.Value.ToShortDateString() : string.Empty;
            var endDateToken = EndDate.HasValue ? EndDate.Value.ToShortDateString() : string.Empty;

            return string.Format("{0}_{1}_{2}_{3}_{4}",
                startDateToken,
                endDateToken,
                ListToString<Guid>(Personnel),
                ListToString<Guid>(Departments),
                ListToString<Guid>(ScheduleEvents) );
        }
        private string ListToString<T>(List<T> list)
        {
            if (list != null) return string.Join(",", list.ToArray());
            return string.Empty;
        }

    }
}
