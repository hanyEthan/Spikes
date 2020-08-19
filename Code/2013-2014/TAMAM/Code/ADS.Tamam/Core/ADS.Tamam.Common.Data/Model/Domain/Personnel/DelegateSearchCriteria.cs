using System;
using System.Collections.Generic;

namespace ADS.Tamam.Common.Data.Model.Domain.Personnel
{
    public class PersonnelDelegatesSearchCriteria
    {
        #region Nested
        public enum OrderByExpression { Default = 0, PersonName = 1, DelegateName = 2, StartDate = 3, EndDate = 4 }
        public enum OrderDirection { Ascending = 0, Descending = 1, }
        public enum OrderByCulture { Default = 0, Varient = 1, }

        #endregion
        #region props ...

        // Parameters
        public List<Guid> PersonnelIds { get; set; }
        public List<Guid> DelegatesIds { get; set; }

        public List<string> Codes { get; set; }

        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }

        // Paging..
        public bool AllowPaging { get; set; }
        public int PageIndex { get; set; }
        public int PageSize { get; set; }

        // Order By..
        public OrderByExpression? OrderBy { get; set; }

        private OrderDirection _OrderByDirection = OrderDirection.Ascending;
        public OrderDirection OrderByDirection { get { return _OrderByDirection; } set { _OrderByDirection = value; } }

        private OrderByCulture _OrderByCultureMode = OrderByCulture.Default;
        public OrderByCulture OrderByCultureMode { get { return _OrderByCultureMode; } set { _OrderByCultureMode = value; } }
        
        #endregion
        #region Helpers

        public override string ToString()
        {
            // New Added to support caching in person delegates search 
            var startDateToken = StartDate.HasValue ? StartDate.Value.ToShortDateString() : string.Empty;
            var endDateToken = EndDate.HasValue ? EndDate.Value.ToShortDateString() : string.Empty;

            return string.Format( "{0}_{1}_{2}_{3}_{4}_{5}_{6}_{7}_{8}_{9}_{10}" ,

                ListToString<Guid>( PersonnelIds ) ,
                ListToString<Guid>( DelegatesIds ) ,
                ListToString<string>( Codes ) ,
                startDateToken ,
                endDateToken ,
                AllowPaging ,
                PageIndex ,
                PageSize ,
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
