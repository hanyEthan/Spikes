using System;
using System.Collections.Generic;

namespace ADS.Tamam.Common.Data.Model.Domain.Personnel
{
    public class PersonSearchCriteria 
    {
        #region Nested

        public enum OrderByExpression { Default = 0 , Name = 1 , Code = 2 , JobTitle = 3 , Department = 4 , Status = 5 , Manager = 6 , }
        public enum OrderDirection { Ascending = 0 , Descending = 1 , }
        public enum OrderByCulture { Default = 0 , Varient = 1 , }
        
        #endregion
        #region props ...

        // Parameters
        public List<Guid> Ids { get; set; }
        public string Name { get; set; }
        public string Code { get; set; }
        public List<string> Codes { get; set; }
        public List<Guid> Departments { get; set; }
        public List<int> Titles { get; set; }
        public List<int> EmployementTypes { get; set; }
        public List<int> Genders { get; set; }
        public bool? ActivationStatus { get; set; }

        // Paging..
        public bool AllowPaging { get; set; }
        public int PageIndex { get; set; }
        public int PageSize { get; set; }

        // Order By..
        public OrderByExpression? OrderBy { get; set; }
        public OrderDirection? OrderByDirection { get; set; }

        private OrderByCulture _OrderByCultureMode = OrderByCulture.Default;
        public OrderByCulture OrderByCultureMode { get { return _OrderByCultureMode; } set { _OrderByCultureMode = value; } }

        // Include Policy Group Support
        public bool IncludePolicyGroup { get; set; }
        
        #endregion
        #region Helpers

        public override string ToString()
        {
            return string.Format( "{0}_{1}_{2}_{3}_{4}_{5}_{6}_{7}_{8}_{9}_{10}_{11}_{12}_{13}_{14}_{15}" ,
            ListToString<Guid>( Ids ) ,
            Name ,
            Code ,
            ListToString<string>( Codes ) ,
            ListToString<Guid>( Departments ) ,
            ListToString<int>( Titles ) ,
            ListToString<int>( EmployementTypes ) ,
            ListToString<int>( Genders ) ,
            ActivationStatus ,
            AllowPaging ,
            PageIndex ,
            PageSize ,
            OrderBy ,
            OrderByDirection ,
            OrderByCultureMode ,
            IncludePolicyGroup );
        }
        private string ListToString<T>( List<T> list )
        {
            if ( list != null )
                return string.Join( "," , list.ToArray() );
            return string.Empty;
        }
        
        #endregion
    }
}