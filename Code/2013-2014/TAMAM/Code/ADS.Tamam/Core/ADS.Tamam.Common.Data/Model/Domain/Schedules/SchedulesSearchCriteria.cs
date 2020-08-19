using System;
using System.Collections.Generic;

namespace ADS.Tamam.Common.Data.Model.Domain.Schedules
{
    public class SchedulesSearchCriteria 
    {
        #region props ...

        public List<Guid> ScheduleIds { get; set; }
        public List<Guid> PersonnelIds { get; set; }
        public List<Guid> DepartmentsIds { get; set; }
        
        #endregion
        #region Helpers

        public override string ToString()
        {
            return string.Format( "{0}_{1}_{2}" ,
            ScheduleIds ,
            DepartmentsIds ,
            PersonnelIds );
        }
        private string ListToString<T>( List<T> list )
        {
            return list != null ? string.Join( "," , list.ToArray() ) : "";
        }
        
        #endregion
    }
}