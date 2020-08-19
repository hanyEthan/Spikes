using System;
using System.Collections.Generic;
using System.Security.Claims;

namespace Mcs.Invoicing.Core.Framework.Infrastructure.Models.Common
{
    public class BaseRequestContext : IBaseRequestContext
    {
        #region props.

        public virtual HeaderContent Header { get; set; }

        #endregion
        #region nested.

        public class HeaderContent
        {
            public virtual string UserId { get; set; }
            public virtual string UserLoginId { get; set; }
            public virtual IEnumerable<string> UserRoles { get; set; }
            public virtual string UserOrganizationId { get; set; }
            public virtual bool? IsOrganizationAdmin { get; set; }

            public virtual string SessionId { get; set; }
            public virtual string CorrelationId { get; set; }

            public virtual string Culture { get; set; }
            public virtual DateTime? RequestTimeUTC { get; set; }

            public virtual string JWT { get; set; }
        }

        #endregion
    }
}
