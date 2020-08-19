using System.Collections.Generic;

namespace XCore.Services.Config.API.Models
{
    public class SearchResultsDTO<T> where T : class
    {
        public List<T> Results { get; set; }
        public int PageIndex { get; set; }
        public int TotalCount { get; set; }
    }
}
