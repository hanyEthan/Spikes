namespace XCore.Utilities.Infrastructure.Entities.Repositories.Models
{
    public abstract class SearchCriteria
    {
        #region nested

        public enum OrderDirection { Ascending = 0, Descending = 1, }
        public enum OrderByCulture { Default = 0, Varient = 1, }

        #endregion

        public bool PagingEnabled { get; set; }
        public virtual int? PageSize { get; set; }
        public int? PageNumber { get; set; }

        public OrderDirection? OrderByDirection { get; set; }

        private OrderByCulture _OrderByCultureMode = OrderByCulture.Default;
        public OrderByCulture OrderByCultureMode { get { return _OrderByCultureMode; } set { _OrderByCultureMode = value; } }
    }
}
