using ADS.Common.Contracts;
using ADS.Tamam.Common.Data.Model.Domain.Personnel;
using System;

namespace ADS.Tamam.Common.Data.Model.Domain.Schedules
{
    [Serializable]
    public class SchedulePerson : IXSerializable
    {
        public Guid Id { get; set; }

        public Guid ScheduleId { get; set; }
        public Guid PersonId { get; set; }


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
        public Person Person { get; set; }
    }
}
