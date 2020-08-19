using System;
using System.Collections.Generic;
using ADS.Common.Contracts;
using ADS.Tamam.Common.Data.Model.Domain.Organization;
using ADS.Tamam.Common.Data.Model.Domain.Personnel;

namespace ADS.Tamam.Common.Data.Model.Domain.Reports
{
    [Serializable]
    public class ScheduledReportEvent : IXSerializable
    {
        public Guid Id { get; set; }
        public Repeates Repeates { get; set; }
        public Guid ReportDefinitionId { get; set; }
        public ReportDefinition ReportDefinition { get; set; }
        public Guid? DepartmentId { get; set; }
        public Department Department { get; set; }
        public IList<Person> Personnel { get; set; }
        public DayOfWeek? Day { get; set; }
        public int? DayNumber { get; set; }
        public DateTime Time { get; set; }
        public string Email { get; set; }
        public string CCs { get; set; }
        public bool IncludeSupervisor { get; set; }

        #region cst ...

        public ScheduledReportEvent()
        {
            Personnel = new List<Person>();
        }
        
        #endregion
    }
}
