using System;
using System.Collections.Generic;
using System.Text;
using XCore.Framework.Infrastructure.Entities.Repositories.Models;

namespace XCore.Services.Notifications.Core.Models.Support
{
    public class InternalNotificationSearchCriteria : SearchCriteria
    {

        #region criteria.        

        public List<int?> Id { get; set; }
        public List<string> Code { get; set; }
        public bool? IsActive { get; set; } = true;
        public InquiryMode InquiryMode { get; set; }
        #endregion
        #region order.

        public OrderByExpression? Order { get; set; }
        public enum OrderByExpression
        {
            Name = 0,
            CreationDate = 1,
        }

        #endregion
    }
}
