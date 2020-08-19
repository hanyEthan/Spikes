using System;
using System.Collections.Generic;
using ADS.Common.Contracts;

namespace ADS.Tamam.Common.Data.Model.Domain.Leaves
{
    [Serializable]
    public class ExcuseSearchCriteria : IXSerializable
    {
        #region Nested

        public enum OrderByExpression { Default = 0 , Name = 1 , From = 2 , To = 3 , Hours = 4 , Status = 5 , Date = 6 , }
        public enum OrderDirection { Ascending = 0 , Descending = 1 , }
        public enum OrderByCulture { Default = 0 , Varient = 1 , }

        #endregion
        #region props ...

        // Parameters
        public List<Guid> Personnel { get; set; }
        public List<string> Codes { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public List<Guid> Departments { get; set; }
        public List<int> ExcuseStatuses { get; set; }
        public int? ExcuseTypeId { get; set; }
        public bool? ActivePersonnelStatus { get; set; }
        public double? Duration { get; set; }
        // Paging..
        public bool AllowPaging { get; set; }
        public int PageIndex { get; set; }
        public int PageSize { get; set; }

        // Order By..
        public OrderByExpression? OrderBy { get; set; }
        public OrderDirection? OrderByDirection { get; set; }

        private OrderByCulture _OrderByCultureMode = ExcuseSearchCriteria.OrderByCulture.Default;
        public OrderByCulture OrderByCultureMode { get { return _OrderByCultureMode; } set { _OrderByCultureMode = value; } }
        
        #endregion
        # region cst..

        public ExcuseSearchCriteria()
        {
            Personnel = new List<Guid>();
            Codes = new List<string>();
            Departments = new List<Guid>();
            ExcuseStatuses = new List<int>();
        }
        public ExcuseSearchCriteria(List<Guid> persons, List<string> codes, DateTime? startDate, DateTime? endDate, List<Guid> departments, List<int> excuseStatuses, bool? activePersonnelStatus, bool allowPaging, int pageIndex, int pageSize)
        {
            Personnel = persons;
            Codes = codes;
            StartDate = startDate;
            EndDate = endDate;
            Departments = departments;
            ExcuseStatuses = excuseStatuses;
            ActivePersonnelStatus = activePersonnelStatus;
            AllowPaging = allowPaging;
            PageIndex = pageIndex;
            PageSize = pageSize;
        }

        public ExcuseSearchCriteria(List<Guid> persons, List<string> codes, DateTime? startDate, DateTime? endDate, List<Guid> departments, List<int> excuseStatuses, bool? activePersonnelStatus, double duration, bool allowPaging, int pageIndex, int pageSize)
        {
            Personnel = persons;
            Codes = codes;
            StartDate = startDate;
            EndDate = endDate;
            Departments = departments;
            ExcuseStatuses = excuseStatuses;
            ActivePersonnelStatus = activePersonnelStatus;
            AllowPaging = allowPaging;
            PageIndex = pageIndex;
            PageSize = pageSize;
            Duration = duration;
        }

        # endregion
        #region Helpers

        public override string ToString()
        {
            return string.Format( "{0}_{1}_{2}_{3}_{4}_{5}_{6}_{7}_{8}_{9}_{10}_{11}_{12}_{13}_{14}" ,

                ListToString<Guid>( Personnel ) ,
                ListToString<Guid>( Departments ) ,
                ListToString<int>( ExcuseStatuses ) ,
                ListToString<string>( Codes ) ,

                ExcuseTypeId ,
                ActivePersonnelStatus ,

                StartDate ,
                EndDate ,

                AllowPaging ,
                PageIndex ,
                PageSize ,
                Duration,

                OrderBy ,
                OrderByDirection ,
                OrderByCultureMode );
        }
        private string ListToString<T>( List<T> list )
        {
            if ( list != null ) return string.Join( "," , list.ToArray() );
            return string.Empty;
        }

        #endregion
    }
}
