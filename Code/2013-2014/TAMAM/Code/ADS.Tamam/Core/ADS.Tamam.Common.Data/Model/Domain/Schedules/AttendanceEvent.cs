using System;
using ADS.Tamam.Common.Data.Model.Domain.Organization;
using ADS.Tamam.Common.Data.Model.Enums;
using ADS.Common.Contracts;

namespace ADS.Tamam.Common.Data.Model.Domain.Schedules
{
    [Serializable]
    public class AttendanceEvent : IXSerializable
    {
        #region props ...

        public Guid Id { get; set; }
        public Guid ScheduleEventId { get; set; }


        public DateTime? InTime { get; set; }
        public Guid StatusInId { get; set; }
        public AttendanceCode StatusIn { get; set; }
        public Guid InTimeRawId { get; set; }
        public bool InTimeIsOriginal { get; set; }
        public string InAttendanceSource { get; set; }

        public string BreakId { get; set; }

        public DateTime? OutTime { get; set; }
        public Guid StatusOutId { get; set; }
        public AttendanceCode StatusOut { get; set; }
        public Guid OutTimeRawId { get; set; }
        public bool OutTimeIsOriginal { get; set; }
        public string OutAttendanceSource { get; set; }

        public int Duration { get; set; }
        public int CalculatedDuration { get; set; }

        public ManualAttendanceStatus ManualAttendanceStatus { get; set; }

        public ScheduleEvent ScheduleEvent { get; set; }

        public AttendanceRawData InAttendanceRawData { get; set; }
        public AttendanceRawData OutAttendanceRawData { get; set; }

        #endregion
        #region cst ...

        public AttendanceEvent()
        {
            //_InEvent = new TimeEvent( this , AttendanceEventType.In );
            //_OutEvent = new TimeEvent( this , AttendanceEventType.Out );
        }
        public AttendanceEvent( Guid id , ScheduleEvent scheduleEvent , DateTime? timeIn , DateTime? timeOut , int hours , int hoursCalculated , AttendanceCode statusIn , AttendanceCode statusOut ) : this()
        {
            Id = id;
            ScheduleEvent = scheduleEvent;
            ScheduleEventId = scheduleEvent == null ? Guid.Empty : scheduleEvent.Id;

            InTime = timeIn;
            OutTime = timeOut;
            Duration = hours;
            CalculatedDuration = hoursCalculated;

            StatusIn = statusIn;
            StatusOut = statusOut;
        }
        
        #endregion
    }
}
