using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using ADS.Common.Models.Domain;

namespace ADS.Common.Models.DTO
{
    public class DetailCodeCriteria
    {
        public string Code { get; set; }
        public string Name { get; set; }
        public string NameCultureVariant { get; set; }
        public int? ParentId { get; set; }
        public int? MasterCodeId { get; set; }

        public string FieldOneValue { get; set; }
        public string FieldTwoValue { get; set; }
        public string FieldThreeValue { get; set; }

        public Expression<Func<DetailCode, object>> OrderBy { get; set; }
        public int TotalCount { get; set; }

        // Paging Support
        public int PageSize { get; set; }
        public int PageIndex { get; set; }
    }
}
