using System;

namespace XCore.Services.Hiring.SDK.Models.Search
{
    public class ApplicationsSearchCriteriaDTO : SearchCriteriaDTO<int>
    {
        #region criteria.        
        public DateTime? DateCreatedFrom { get; set; }
        public DateTime? DateCreatedTo { get; set; }

        #endregion
        
    }
}
