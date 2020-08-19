using System;
using System.Collections.Generic;

namespace XCore.Services.Hiring.API.Models.Search
{
    public class ApplicationsSearchCriteriaDTO : SearchCriteriaDTO<int>
    {
        #region criteria.        
        public DateTime? DateCreatedFrom { get; set; }
        public DateTime? DateCreatedTo { get; set; }


        #endregion

    }
}
