using System.Collections.Generic;

namespace XCore.Framework.Infrastructure.Entities.Repositories.Models
{
    public class SearchResults<T> where T : class
    {
        #region props.

        public List<T> Results { get; set; }
        public int PageIndex { get; set; }
        public int TotalCount { get; set; }

        #endregion
        #region cst.

        public SearchResults()
        {
        }
        public SearchResults( List<T> results ) : this()
        {
            this.Results = results;
        }

        #endregion
    }
}
