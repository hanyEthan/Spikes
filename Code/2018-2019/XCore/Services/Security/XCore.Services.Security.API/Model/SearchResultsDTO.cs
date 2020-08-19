using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace XCore.Services.Security.API.Model
{
    public class SearchResultsDTO<T> where T : class
    {
        public List<T> Results { get; set; }
        public int PageIndex { get; set; }
        public int TotalCount { get; set; }
    }
}
