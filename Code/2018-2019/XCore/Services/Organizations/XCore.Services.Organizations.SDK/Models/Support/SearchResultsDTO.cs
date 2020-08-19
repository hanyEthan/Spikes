using System;
using System.Collections.Generic;
using System.Text;

namespace XCore.Services.Organizations.SDK.Models.Support
{
    public class SearchResultsDTO<T> where T : class
    {
        #region props.

        public List<T> Results { get; set; }
        public int? PageIndex { get; set; }
        public int? TotalCount { get; set; }

        #endregion
    }
}
