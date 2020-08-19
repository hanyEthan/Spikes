using System;
using System.Collections.Generic;
using ADS.Common.Contracts;

namespace ADS.Tamam.Common.Data.Model.Domain.Schedules
{
    [Serializable]
    public class ScheduleTemplateDays : IXSerializable
    {
        public Guid Id { get; set; }
        
        public bool IsDayOff { get; set; }
        public int Sequence { get; set; }
        public int? DayId { get; set; }
        
        public Guid ScheduleTemplateId { get; set; }
        [XDontSerialize]
        public ScheduleTemplate Template { get; set; }
        public IList<ScheduleTemplateDayShifts> DayShifts { get; set; }

        #region cst ...

        public ScheduleTemplateDays()
        {
            DayShifts = new List<ScheduleTemplateDayShifts>();
        }
        
        #endregion
    }
}

