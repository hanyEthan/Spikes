using System;
using System.Collections.Generic;
using ADS.Common.Contracts;

namespace ADS.Tamam.Common.Data.Model.Domain.Reports
{
    [Serializable]
    public class ScheduledReportEventsSearchCriteria : IXSerializable
    {
        public List<Guid> Reports { get; set; }
        public List<Guid> Departments { get; set; }
        public DateTime? Date { get; set; }
        public DateTime? TimeFrom { get; set; }
        public DateTime? TimeTo { get; set; }

        #region cst ...

        public ScheduledReportEventsSearchCriteria()
        {
            Reports = new List<Guid>();
            Departments = new List<Guid>();
        }
        
        #endregion
        #region Helpers

        public override string ToString()
        {
            return string.Format( "{0}_{1}_{2}_{3}",

                ListToString<Guid>( Reports ),
                ListToString<Guid>( Departments ),
                Date,
                TimeFrom,
                TimeTo );
        }
        private string ListToString<T>( List<T> list )
        {
            if ( list != null ) return string.Join( "," , list.ToArray() );
            return string.Empty;
        }

        #endregion
    }
}
