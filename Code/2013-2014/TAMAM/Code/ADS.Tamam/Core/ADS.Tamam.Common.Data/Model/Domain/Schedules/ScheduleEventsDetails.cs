using ADS.Common.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ADS.Tamam.Common.Data.Model.Domain.Schedules
{
    [Serializable]
    public class ScheduleEventsDetails : IXSerializable
    {
        public string Key { get; set; }
        public Guid SEId { get; set; }
        public DateTime SEEventDate { get; set; }
        public DateTime? SEExpectedIn { get; set; }
        public DateTime? SEExpectedOut { get; set; }
        public DateTime? SEActualIn { get; set; }
        public DateTime? SEActualOut { get; set; }
        public Guid SEPersonid { get; set; }
        public Guid SEScheduleid { get; set; }
        public Guid SEShiftid { get; set; }
        public int SEHours { get; set; }
        public int SECalculatedHours { get; set; }
        public int SEOffHours { get; set; }
        public int SEOvertime { get; set; }
        public int SECalculatedOvertime { get; set; }
        public Guid SEPayCodeStatusId { get; set; }
        public Guid SETotalStatusId { get; set; }
        public bool SEIsDirty { get; set; }
        public Guid SEInTimeRawId { get; set; }
        public bool SEInTimeIsOriginal { get; set; }
        public int SEInManualAttendanceStatus { get; set; }
        public Guid SEOutTimeRawId { get; set; }
        public bool SEOutTimeIsOriginal { get; set; }
        public int SEOutManualAttendanceStatus { get; set; }
        public int SEJustificationStatus { get; set; }
        public string SEStaffComments { get; set; }
        public string SEManagerComments { get; set; }
        public int SELeaveTypeId { get; set; }
        public int SEWorkingHoursExpected { get; set; }
        public int SEWorkingHoursTotal { get; set; }
        public int SEOffHoursCalculated { get; set; }
        public Guid SEHoursStatusId { get; set; }
        public string SEOutAttendanceSource { get; set; }
        public string SEInAttendanceSource { get; set; }
        public Guid AEId { get; set; }
        public Guid AEScheduleEventId { get; set; }
        public DateTime? AEInTime { get; set; }
        public DateTime? AEOutTime { get; set; }
        public int AEDuration { get; set; }
        public int AECalculatedDuration { get; set; }
        public Guid? AEStatusInId { get; set; }
        public Guid? AEStatusOutId { get; set; }
        public Guid? AEInTimeRawId { get; set; }
        public bool AEInTimeIsOriginal { get; set; }
        public Guid? AEOutTimeRawId { get; set; }
        public bool AEOutTimeIsOriginal { get; set; }
        public int AEManualAttendanceStatus { get; set; }
    }
}
