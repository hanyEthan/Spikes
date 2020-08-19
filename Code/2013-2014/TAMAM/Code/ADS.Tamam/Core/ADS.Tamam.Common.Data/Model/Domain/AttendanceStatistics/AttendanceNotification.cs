using System;
using ADS.Common.Contracts;

namespace ADS.Tamam.Common.Data.Model.Domain.AttendanceStatistics
{
    [Serializable]
    public class AttendanceNotification : IXSerializable
    {
        public Guid ManagerId { get; set; }
        public string ManagerCode { get; set; }
        public string ManagerName { get; set; }
        public string ManagerNameCultureVariant { get; set; }

        public Guid DepartmentId { get; set; }
        public string DepartmentCode { get; set; }
        public string DepartmentName { get; set; }
        public string DepartmentNameCultureVarient { get; set; }

        public Guid? ParentDepartmentId { get; set; }
        public string ParentDepartmentCode { get; set; }
        public string ParentDepartmentName { get; set; }
        public string ParentDepartmentNameCultureVarient { get; set; }

        public Guid StaffId { get; set; }
        public string StaffCode { get; set; }
        public string StaffName { get; set; }
        public string StaffNameCultureVariant { get; set; }

        public string Message { get; set; }
        public string MessageCultureVariant { get; set; }

        public Guid ScheduleEventId { get; set; }
        public DateTime EventDate { get; set; }
        public DateTime ActualIn { get; set; }
        public DateTime ActualOut { get; set; }
        public DateTime ExpectedIn { get; set; }
        public DateTime ExpectedOut { get; set; }
        public string TotalStatus { get; set; }
        public string TotalStatusCultureVariant { get; set; }

    }
}
