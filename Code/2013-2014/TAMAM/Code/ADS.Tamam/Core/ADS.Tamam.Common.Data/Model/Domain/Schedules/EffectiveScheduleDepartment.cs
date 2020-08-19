using System;

using ADS.Common.Contracts;
using ADS.Tamam.Common.Data.Model.Domain.Organization;

namespace ADS.Tamam.Common.Data.Model.Domain.Schedules
{
    [Serializable]
    public class EffectiveScheduleDepartment : IEffectiveSchedule , IXSerializable
    {
        public Guid Id { get; set; }

        public Guid ScheduleId { get; set; }
        public Guid DepartmentId { get; set; }

        public DateTime StartDate { get; set; }
        public DateTime? EndDate { get; set; }

        public Department Department { get; set; }

        #region ITimeRange

        public DateTime? StartTime
        {
            get { return this.StartDate; }
            set { this.StartDate = value.GetValueOrDefault(); }
        }
        public DateTime? EndTime
        {
            get { return this.EndDate; }
            set { this.EndDate = value.GetValueOrDefault(); }
        }
        [XDontSerialize] public bool SpansMultipleDays
        {
            get { return false; }
        }

        #endregion
    }
}
