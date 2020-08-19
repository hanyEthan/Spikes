using System.Runtime.Serialization;
using ADS.Common.Workflow.Models;

namespace ADS.Tamam.Common.Handlers.Workflow.Attendance.Violations.Data
{
    [DataContract(IsReference = true)]
    public class AttendanceFinalizationWorkflowData : WorkflowBaseData
    {
        [DataMember] public bool Justified { get; set; }

        #region cst ...

        public AttendanceFinalizationWorkflowData() { }
        public AttendanceFinalizationWorkflowData( bool justified ) : this()
        {
            Justified = justified;
        }

        #endregion
    }
}
