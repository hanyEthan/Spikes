using System;
using System.Collections.Generic;
using ADS.Common.Contracts;

namespace ADS.Tamam.Common.Data.Model.Domain.Leaves
{
    [Serializable]
    public class LeaveSearchCriteria : IXSerializable
    {
        #region Nested

        public enum OrderByExpression { Default = 0, Name = 1, From = 2, To = 3, Days = 4, Type = 5, Status = 6, }
        public enum OrderDirection { Ascending = 0, Descending = 1, }
        public enum OrderByCulture { Default = 0, Varient = 1, }

        #endregion
        #region props ...

        // Parameters
        public List<Guid> Personnel { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public List<Guid> Departments { get; set; }
        public List<int> LeaveTypes { get; set; }
        public List<int> LeaveStatuses { get; set; }
        public List<string> Codes { get; set; }

        // Paging..
        public bool AllowPaging { get; set; }
        public int PageIndex { get; set; }
        public int PageSize { get; set; }

        // Order By..
        public OrderByExpression? OrderBy { get; set; }
        public OrderDirection? OrderByDirection { get; set; }

        private OrderByCulture _OrderByCultureMode = LeaveSearchCriteria.OrderByCulture.Default;
        public OrderByCulture OrderByCultureMode { get { return _OrderByCultureMode; } set { _OrderByCultureMode = value; } }

        #endregion
        # region cst...

        public LeaveSearchCriteria()
        {
            Personnel = new List<Guid>();
            Departments = new List<Guid>();
            LeaveTypes = new List<int>();
            LeaveStatuses = new List<int>();
            Codes = new List<string>();

        }
        public LeaveSearchCriteria(List<Guid> persons, List<string> codes, DateTime? startDate, DateTime? endDate, List<Guid> departments, List<int> leaveTypes, List<int> leaveStatuses, bool allowPaging, int pageIndex, int pageSize)
        {
            Personnel = persons;
            Codes = codes;
            StartDate = startDate;
            EndDate = endDate;
            Departments = departments;
            LeaveTypes = leaveTypes;
            LeaveStatuses = leaveStatuses;
            AllowPaging = allowPaging;
            PageIndex = pageIndex;
            PageSize = pageSize;
        }

        # endregion
        #region Helpers

        public override string ToString()
        {
            return string.Format("{0}_{1}_{2}_{3}_{4}_{5}_{6}_{7}_{8}_{9}_{10}_{11}_{12}",

                ListToString<Guid>(Personnel),
                ListToString<Guid>(Departments),
                ListToString<int>(LeaveTypes),
                ListToString<int>(LeaveStatuses),
                ListToString<string>(Codes),

                StartDate,
                EndDate,

                AllowPaging,
                PageIndex,
                PageSize,

                OrderBy,
                OrderByDirection,
                OrderByCultureMode);
        }
        private string ListToString<T>(List<T> list)
        {
            if (list != null) return string.Join(",", list.ToArray());
            return string.Empty;
        }

        #endregion
    }
}
