using System;
using ADS.Common.Contracts;
using ADS.Tamam.Common.Data.Model.Enums;

namespace ADS.Tamam.Common.Data.Model.Domain.Schedules
{
    [Serializable]
    public class AttendanceEventMetadata : IXSerializable
    {
        public DateTime Date { get; set; }
        public DateTime? Time { get; set; }

        public AttendanceEventType Mode { get; set; }

        public Guid? PersonId { get; set; }
        public Guid? ScheduleId { get; set; }
        public Guid? AttendanceId { get; set; }
        public ShiftType? ShiftType { get; set; }
        public int? ShiftExpectedStartHour { get; set; }
        public int? ShiftExpectedEndHour { get; set; }

        public string Comment { get; set; }
    }
}
