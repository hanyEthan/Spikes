using System;
using System.Collections.Generic;
using System.Text;
using XCore.Framework.Infrastructure.Entities.Repositories.Models;

namespace XCore.Services.Security.Core.Models.Support
{
   public class ClaimSearchCriteria : SearchCriteria
    {
        #region criteria.        

        public int? Id { get; set; }
        public string Code { get; set; }
        public string Key { get; set; }
        public string Value { get; set; }
        public string Map { get; set; }

        public bool? IsActive { get; set; } = true;
        public bool IsRecursive { get; set; }

        public int? AppId { get; set; }
        public string AppCode { get; set; }
        public int? ActorId { get; set; }
        public string ActorCode { get; set; }
        public int? RoleId { get; set; }
        public string RoleCode { get; set; }
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
