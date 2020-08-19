using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using ADS.Common.Contracts;
using ADS.Tamam.Common.Data.Model.Domain.Schedules;
using ADS.Tamam.Common.Data.Model.Enums;

namespace ADS.Tamam.Common.Data.Model.Domain.Attendance
{
    [Serializable]
    public class AttendanceFilters : IXSerializable
    {
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }

        public List<Guid> Personnel { get; set; }
        public List<Guid> Departments { get; set; }
        public List<Guid> Statuses { get; set; }
        public List<Guid> Shifts { get; set; }
        public bool HasViolations { get; set; }
        public bool WorkedLess { get; set; }
        public bool WorkedMore { get; set; }
        public bool HasOvertime { get; set; }
        public bool OnlyHasBreaks { get; set; }
        public bool OnlyHasManualEdits { get; set; }
        public JustificationStatus? JustificationStatus { get; set; }

        public bool ManualSearch { get; set; }

        public Expression<Func<ScheduleEvent , object>> OrderBy { get; set; }
        public FilterDirection OrderDirection { get; set; }
        public int? Count { get; set; }

        public DateTime? TimeInFrom { get; set; }
        public DateTime? TimeInTo { get; set; }
        public DateTime? TimeOutFrom { get; set; }
        public DateTime? TimeOutTo { get; set; }

        public List<int> PersonnelEmploymentType { get; set; }

        // Paging..
        public bool AllowPaging { get; set; }
        public int PageIndex { get; set; }
        public int PageSize { get; set; }

        public AttendanceFilters()
        {
            Personnel = new List<Guid>();
            Departments = new List<Guid>();
            Statuses = new List<Guid>();
            Shifts = new List<Guid>();
            PersonnelEmploymentType = new List<int>();
        }

        #region Helpers

        public override string ToString()
        {
            return string.Format( "{0}_{1}_{2}_{3}_{4}_{5}_{6}_{7}_{8}_{9}_{10}_{11}_{12}_{13}_{14}_{15}_{16}_{17}_{18}_{19}_{20}_{21}_{22}_{23}_{24}_{25}" ,

                StartDate ,
                EndDate ,

                ListToString<Guid>( Personnel ) ,
                ListToString<Guid>( Departments ) ,
                ListToString<Guid>( Statuses ) ,
                ListToString<Guid>( Shifts ) ,

                HasViolations ,
                WorkedLess ,
                WorkedMore ,
                HasOvertime ,
                OnlyHasBreaks ,
                OnlyHasManualEdits ,
                JustificationStatus ,
                ManualSearch ,
                ManualSearch ,

                ListToString<int>(PersonnelEmploymentType) ,

                TimeInFrom ,
                TimeInTo ,
                TimeOutFrom ,
                TimeOutTo ,

                AllowPaging ,
                PageIndex ,
                PageSize ,

                OrderBy ,
                OrderDirection ,
                Count );
        }
        private string ListToString<T>( List<T> list )
        {
            if ( list != null ) return string.Join( "," , list.ToArray() );
            return string.Empty;
        }

        #endregion
    }
}
