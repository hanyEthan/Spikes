﻿using XCore.Framework.Infrastructure.Entities.Repositories.Models;

namespace XCore.Services.Config.Core.Models.Support
{
    public class AppSearchCriteria : SearchCriteria
    {
        #region criteria.        

        public int? Id { get; set; }
        public string Name { get; set; }
        public string Code { get; set; }
        public bool? IsActive { get; set; } = true;

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
