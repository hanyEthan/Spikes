namespace XCore.Framework.Infrastructure.Entities.Repositories.Models
{
    public abstract class SearchCriteria
    {
        #region nested

        public enum OrderDirection { Ascending = 0, Descending = 1, }
        public enum OrderByCulture { Default = 0, Varient = 1, }

        #endregion
        
        #region paging

        public bool PagingEnabled { get; set; }
        public virtual int? PageSize { get; set; }
        public int? PageNumber { get; set; }

        #endregion
        #region ordering

        public OrderDirection? OrderByDirection { get; set; }
        public OrderByCulture OrderByCultureMode { get; set; } = OrderByCulture.Default;

        #endregion
    }
}
