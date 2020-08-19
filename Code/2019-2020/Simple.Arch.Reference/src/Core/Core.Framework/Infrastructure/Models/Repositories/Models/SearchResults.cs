using System.Collections.Generic;

namespace Mcs.Invoicing.Services.Core.Framework.Infrastructure.Models.Repositories.Models
{
    public class SearchResults<T> where T : class
    {
        #region props.

        public List<T> Results { get; set; }
        public MetadataHeader Metadata { get; set; }

        #endregion
        #region cst.

        public SearchResults()
        {
        }
        public SearchResults(List<T> results) : this()
        {
            this.Results = results;
        }

        #endregion
        #region nested.

        public class MetadataHeader
        {
            public int? PageIndex { get; set; }
            public int TotalCount { get; set; }
            public int? PageSize { get; set; }
        }

        #endregion
    }
}
