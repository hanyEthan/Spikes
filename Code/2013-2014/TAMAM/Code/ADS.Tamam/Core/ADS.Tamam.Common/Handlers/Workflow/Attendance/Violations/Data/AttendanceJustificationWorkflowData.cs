using System.Runtime.Serialization;
using ADS.Common.Workflow.Models;

namespace ADS.Tamam.Common.Handlers.Workflow.Attendance.Violations.Data
{
    [DataContract(IsReference = true)]
    public class AttendanceJustificationWorkflowData : WorkflowBaseData
    {
        [DataMember]
        public string ScheduleEventId { get; set; }

        #region cst ...

        public AttendanceJustificationWorkflowData() { }
        public AttendanceJustificationWorkflowData(string scheduleEventId)
            : this()
        {
            ScheduleEventId = scheduleEventId;
        }

        #endregion
    }
}
