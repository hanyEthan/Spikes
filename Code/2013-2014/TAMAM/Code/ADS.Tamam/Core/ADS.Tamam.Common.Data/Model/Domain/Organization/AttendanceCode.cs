using System;
using System.Collections.Generic;
using System.Threading;
using ADS.Common.Models;
using ADS.Tamam.Common.Data.Model.Domain.Schedules;
using ADS.Tamam.Common.Data.Model.Enums;
using ADS.Common.Contracts;

namespace ADS.Tamam.Common.Data.Model.Domain.Organization
{
    [Serializable]
    public class AttendanceCode : IXSerializable , IBaseModel
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string NameCultureVarient { get; set; }
        public string Code { get; set; }
        public AttendanceCodesImportance Importance { get; set; }
        public bool IsActive { get; set; }
        public bool IsMajor
        {
            get
            {
                return Importance == AttendanceCodesImportance.Major;
            }
            set
            {
                Importance = value ? AttendanceCodesImportance.Major : Importance = AttendanceCodesImportance.Minor;
            }
        }

        public IList<ScheduleEvent> InStatusScheduleEvents { get; set; }
        public IList<ScheduleEvent> OutStatusScheduleEvents { get; set; }
        public IList<ScheduleEvent> TotalStatusScheduleEvents { get; set; }

        public AttendanceCode()
        {
            InStatusScheduleEvents = new List<ScheduleEvent>();
            OutStatusScheduleEvents = new List<ScheduleEvent>();
            TotalStatusScheduleEvents = new List<ScheduleEvent>();
        }

        # region Helpers

        public string GetLocalizedName()
        {
            return Thread.CurrentThread.CurrentCulture.Name == "ar-EG"
                ? this.NameCultureVarient
                : this.Name;
        }

        # endregion

        # region IBaseModel

        object IBaseModel.Id { get { return this.Id; } }
        [XDontSerialize]
        public string NameCultureVariant { get { return this.NameCultureVarient; } }

        # endregion
    }
}
