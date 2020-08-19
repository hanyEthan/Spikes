using System;

namespace XCore.Services.Hiring.API.Models.Search
{
    public class CandidatesSearchCriteriaDTO : SearchCriteriaDTO<int>
    {
        #region criteria.        
        public DateTime? DateCreatedFrom { get; set; }
        public DateTime? DateCreatedTo { get; set; }     
        #endregion
    }
}
