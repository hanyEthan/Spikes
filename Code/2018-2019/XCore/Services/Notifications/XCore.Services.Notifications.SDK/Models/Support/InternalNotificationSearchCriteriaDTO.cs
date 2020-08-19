using System.Collections.Generic;

namespace XCore.Services.Notifications.SDK.Models.Support
{
    public class InternalNotificationSearchCriteriaDTO
    {
        #region criteria.        
        public List<int?> Id { get; set; }
        public List<string> Code { get; set; }
        public bool? IsActive { get; set; } = true;
        //public InquiryMode InquiryMode { get; set; }

        public bool PagingEnabled { get; set; }
        public int? PageSize { get; set; }
        public int? PageNumber { get; set; }
        public int? Order { get; set; }
        public int? OrderByDirection { get; set; }
        public int OrderByCultureMode { get; set; }
        #endregion
    }
}
