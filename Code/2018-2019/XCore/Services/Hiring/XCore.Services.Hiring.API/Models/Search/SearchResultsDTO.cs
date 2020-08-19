using System.Collections.Generic;

namespace XCore.Services.Hiring.API.Models.DTO
{
    public class SearchResultsDTO<T> where T : class
    {
        public List<T> Results { get; set; }
        public int PageIndex { get; set; }
        public int TotalCount { get; set; }
    }
}
