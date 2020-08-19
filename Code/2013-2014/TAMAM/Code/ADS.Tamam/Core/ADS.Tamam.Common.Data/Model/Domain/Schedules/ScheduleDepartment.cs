using ADS.Common.Contracts;
using ADS.Tamam.Common.Data.Model.Domain.Organization;
using System;

namespace ADS.Tamam.Common.Data.Model.Domain.Schedules
{
    [Serializable]
    public class ScheduleDepartment: IXSerializable
    {
        public Guid Id { get; set; }
     
        public Guid ScheduleId { get; set; }
        public Guid DepartmentId { get; set; }

        private DateTime startDate;
        public DateTime StartDate
        {
            get
            {
                return startDate;
            }
            set
            {
                startDate = value.Date;
            }
        }

        private DateTime? endDate;
        public DateTime? EndDate
        {
            get
            {
                return endDate;
            }
            set
            {
                endDate = value.HasValue ? (DateTime?)value.Value.Date : null;
            }
        }
        [XDontSerialize]
        public Schedule Schedule { get; set; }
        public Department Department { get; set; } 
    }
}
