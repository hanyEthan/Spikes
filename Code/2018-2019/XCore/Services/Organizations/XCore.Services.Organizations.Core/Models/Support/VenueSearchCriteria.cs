using System;
using System.Collections.Generic;
using System.Text;
using XCore.Framework.Infrastructure.Entities.Repositories.Models;

namespace XCore.Services.Organizations.Core.Models.Support
{
  public class VenueSearchCriteria : SearchCriteria
    {
        #region criteria.        

        public int? Id { get; set; }
        public bool IncludeRecursive { get; set; } = false;
        public string Code { get; set; }
        public string Name { get; set; }
        public bool? IsActive { get; set; }



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
