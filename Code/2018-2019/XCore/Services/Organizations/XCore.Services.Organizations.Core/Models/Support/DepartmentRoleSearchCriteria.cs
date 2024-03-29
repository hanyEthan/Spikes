﻿using System;
using System.Collections.Generic;
using System.Text;
using XCore.Framework.Infrastructure.Entities.Repositories.Models;

namespace XCore.Services.Organizations.Core.Models.Support
{
    public  class DepartmentRoleSearchCriteria : SearchCriteria
    {
        #region criteria.
        public int Id { get; set; }
        public int? RoleId { get; set; }
        public int? DepartmentId { get; set; }

        public string Code { get; set; }
        public bool? IsActive { get; set; } = true;
        
        #endregion
        #region order.

        public OrderByExpression? Order { get; set; }
        public enum OrderByExpression
        {
            CreationDate = 0,
        }

        #endregion
    }
}
