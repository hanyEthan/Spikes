﻿using System.Collections.Generic;

namespace XCore.Services.Geo.API.Models
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
