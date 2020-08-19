using ADS.Common.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ADS.Tamam.Common.Data.Model.DTO.Composite
{
    [Serializable]
    public class LeaveDTO : IXSerializable
    {
        public Guid Id { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public double EffectiveDaysCount { get; set; }
        public Guid PersonID { get; set; }
        public string PersonName { get; set; }
        public string PersonNameCultureVariant { get; set; }
        public Guid DepartmentID { get; set; }
        public string DepartmentName { get; set; }
        public string DepartmentNameCultureVariant { get; set; }
        public int LeaveTypeID { get; set; }
        public string LeaveTypeName { get; set; }
        public string LeaveTypeNameCultureVariant { get; set; }
        public string LeaveTypeColor { get; set; }
        public int LeaveStatusID { get; set; }
        public string LeaveStatusName { get; set; }
        public string LeaveStatusNameCultureVariant { get; set; }
        public string LeaveStatusColor { get; set; }

        [XDontSerialize]
        public string DateFormatted
        {
            get
            {
                return string.Format("{0} - {1}", StartDate.ToString("dd/MM/yyyy"), EndDate.ToString("dd/MM/yyyy"));
            }
        }
    }
}
