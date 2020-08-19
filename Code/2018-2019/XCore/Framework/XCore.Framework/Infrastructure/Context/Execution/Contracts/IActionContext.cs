using XCore.Framework.Infrastructure.Context.Execution.Extensions;
using XCore.Framework.Infrastructure.Context.Execution.Support;

namespace Core.Components.Framework.Context.Contracts
{
    public interface IActionContext
    {
        RequestContext Request { get; set; }
        IResponse Response { get; set; }

        AuditingContext Auditing { get; set; }
        AuthorizationContext Authorization { get; set; }
    }
}
