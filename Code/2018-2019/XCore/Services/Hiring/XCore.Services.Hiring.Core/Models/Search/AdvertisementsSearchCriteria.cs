using System;
using System.Collections.Generic;
using XCore.Framework.Infrastructure.Entities.Repositories.Models;

namespace XCore.Services.Hiring.Core.Models.Search
{
    public class AdvertisementsSearchCriteria : SearchCriteria
    {
        #region criteria.        
        public DateTime? DateCreatedFrom { get; set; }
        public DateTime? DateCreatedTo { get; set; }
        public string Title { get; set; }
        public int? Id { get; set; }
        public List<string> Apps { get; set; }
        public List<string> Modules { get; set; }
        public List<string> Codes { get; set; }
        public bool? IsActive { get; set; }
        public SearchIncludes SearchIncludes { get; set; }

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
