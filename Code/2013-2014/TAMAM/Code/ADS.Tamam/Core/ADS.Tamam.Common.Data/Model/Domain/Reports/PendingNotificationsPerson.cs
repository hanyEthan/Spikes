using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ADS.Tamam.Common.Data.Model.Domain.Reports
{
    [Serializable]
    public class PendingNotificationsPerson
    {
        public Guid StaffId { get; set; }
        public string StaffCode { get; set; }
        public string StaffName { get; set; }
        public string StaffNameCultureVariant { get; set; }

        public Guid DepartmentId { get; set; }
        public string DepartmentCode { get; set; }
        public string DepartmentName { get; set; }
        public string DepartmentNameCultureVarient { get; set; }

        public Guid? ParentDepartmentId { get; set; }
        public string ParentDepartmentCode { get; set; }
        public string ParentDepartmentName { get; set; }
        public string ParentDepartmentNameCultureVarient { get; set; }

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

        #region cst ...
        public PendingNotificationsPerson()
        {

        }

        #endregion
    }
}
