using System;
using Core.Components.Framework.Context.Contracts;
using XCore.Framework.Infrastructure.Context.Execution.Extensions;

namespace XCore.Framework.Infrastructure.Context.Execution.Support
{
    [Serializable]
    public class ActionContext : IActionContext
    {
        #region props.

        public RequestContext Request { get; set; }
        public IResponse Response { get; set; }

        public ValidationContextBase Validation { get; set; }
        public AuditingContext Auditing { get; set; }
        public AuthorizationContext Authorization { get; set; }

        #endregion
        #region cst ...

        public ActionContext()
        {
        }

        #endregion
    }
}
