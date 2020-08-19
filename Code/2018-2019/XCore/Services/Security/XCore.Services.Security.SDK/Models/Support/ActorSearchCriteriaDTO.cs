﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using XCore.Framework.Infrastructure.Entities.Repositories.Models;

namespace XCore.Services.Security.SDK.Models.Support
{
    public class ActorSearchCriteriaDTO : SearchCriteria
    {
        #region criteria.        

        public int? Id { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public bool? IsActive { get; set; } = true;

        public string AppCode { get; set; }
        public int? AppId { get; set; }
        public int? RoleId { get; set; }
        public string RoleCode { get; set; }


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
