using System;
using System.Collections.Generic;
using System.Text;
using ADS.Common.Contracts;

namespace ADS.Tamam.Common.Data.Model.Domain.Schedules
{
    [Serializable]
    public class ScheduleTemplate : IXSerializable
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string NameCultureVarient { get; set; }
        public string NameCultureVarientAbstract { get; set; }
        public string Description { get; set; }
        public int Repeat { get; set; }
        public int RepeatType { get; set; }
        public bool IsActive { get; set; }
        [XDontSerialize]
        public string RepeatDescription
        {
            get
            {
                StringBuilder desc = new StringBuilder();
                desc.Append(Repeat.ToString());
                desc.Append(" ");
                if (Repeat > 1) desc.Append(RepeatTypeToString(RepeatType, true));
                else desc.Append(RepeatTypeToString(RepeatType, false));
                return desc.ToString();
            }
        }
       
        public IList<ScheduleTemplateDays> TemplateDetails { get; set; }
        [XDontSerialize]
        public IList<Schedule> Schedules { get; set; }

        private string RepeatTypeToString(int repeatType, bool plural)
        {
            if (!plural)
            {
                switch (repeatType)
                {
                    case 1: return ADS.Tamam.Resources.Culture.Common.Day;
                    case 2: return ADS.Tamam.Resources.Culture.Common.Week;
                    case 3: return ADS.Tamam.Resources.Culture.Common.Month;
                    default: return "";
                }
            }
            else
            {
                switch (repeatType)
                {
                    case 1: return ADS.Tamam.Resources.Culture.Common.Days;
                    case 2: return ADS.Tamam.Resources.Culture.Common.Weeks;
                    case 3: return ADS.Tamam.Resources.Culture.Common.Months; ;
                    default: return "";
                }
            }
        }

        #region cst ...

        public ScheduleTemplate()
        {
            TemplateDetails = new List<ScheduleTemplateDays>();
            Schedules = new List<Schedule>();
        }

        #endregion
    }
}
