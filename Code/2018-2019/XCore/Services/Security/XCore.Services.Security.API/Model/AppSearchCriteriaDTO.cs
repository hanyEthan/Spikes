using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using XCore.Services.Security.Core.Models.Support;

namespace XCore.Services.Security.API.Model
{
    public class AppSearchCriteriaDTO
    {
        #region criteria.        

        public int? Id { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public bool? IsActive { get; set; }
        public bool PagingEnabled { get; set; }
        public int? PageSize { get; set; }
        public int? PageNumber { get; set; }
        public int? Order { get; set; }
        public int? OrderByDirection { get; set; }
        public int OrderByCultureMode { get; set; }
        public InquiryMode InquiryMode { get; set; }

        #endregion
    }
}
