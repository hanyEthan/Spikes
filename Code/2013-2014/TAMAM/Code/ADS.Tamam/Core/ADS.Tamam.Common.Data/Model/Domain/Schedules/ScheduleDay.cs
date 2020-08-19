using System;
using ADS.Common.Contracts;

namespace ADS.Tamam.Common.Data.Model.Domain.Schedules
{
    [Serializable]
    public class ScheduleDay : ScheduleTemplateDays
    {
        public DateTime Day { get; set; }
        public Guid ScheduleId { get; set; }
        public Schedule Schedule { get; set; }

        #region cst ...
        
        public ScheduleDay()
        {
        }
        public ScheduleDay( ScheduleTemplateDays detail, Guid scheduleId )
        {
            this.Id = detail.Id;
            this.IsDayOff = detail.IsDayOff;
            this.ScheduleTemplateId = detail.ScheduleTemplateId;
            this.Sequence = detail.Sequence;
            this.DayShifts = detail.DayShifts;
            this.Template = detail.Template;
            this.DayId = detail.DayId;
            this.DayShifts = detail.DayShifts;

            this.ScheduleId = scheduleId;
        }
        
        #endregion
    }
}
