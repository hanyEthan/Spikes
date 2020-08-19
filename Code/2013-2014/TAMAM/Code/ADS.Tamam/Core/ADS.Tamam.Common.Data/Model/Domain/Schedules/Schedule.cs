using System;
using System.Collections.Generic;
using ADS.Common.Contracts;

namespace ADS.Tamam.Common.Data.Model.Domain.Schedules
{
    [Serializable]
    public class Schedule : IXSerializable
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string NameCultureVarient { get; set; }
        public string NameCultureVarientAbstract { get; set; }

        public DateTime StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public bool Active { get; set; }

        public Guid ScheduleTemplateId { get; set; }
        public ScheduleTemplate ScheduleTemplate { get; set; }

        [XDontSerialize] public IList<SchedulePerson> SchedulePersonnel { get; set; }
        [XDontSerialize] public IList<ScheduleDepartment> ScheduleDepartments { get; set; }
        [XDontSerialize] public IList<EffectiveSchedulePerson> EffectiveSchedulePersonnel { get; set; }
        [XDontSerialize] public IList<EffectiveScheduleDepartment> EffectiveScheduleDepartments { get; set; }

        #region cst ...

        public Schedule()
        {
            SchedulePersonnel = new List<SchedulePerson>();
            ScheduleDepartments = new List<ScheduleDepartment>();
            EffectiveSchedulePersonnel = new List<EffectiveSchedulePerson>();
            EffectiveScheduleDepartments = new List<EffectiveScheduleDepartment>();
        }
        
        #endregion
    }
}
