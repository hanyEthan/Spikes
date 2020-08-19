using System;
using ADS.Common.Contracts;
using ADS.Tamam.Common.Data.Model.Enums;

namespace ADS.Tamam.Common.Data.Model.Domain.Attendance
{
    [Serializable]
    public class AttendanceViolationReview : IXSerializable
    {
        public Guid ScheduleEventId { get; set; }
        public ReviewAttendanceStatus Status { get; set; }
        public Guid ReviewerId { get; set; }
        public string Comment { get; set; }
    }
}
