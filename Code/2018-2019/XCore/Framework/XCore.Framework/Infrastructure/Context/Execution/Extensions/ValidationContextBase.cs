using System.Threading.Tasks;
using Core.Components.Framework.Context.Contracts;

namespace XCore.Framework.Infrastructure.Context.Execution.Extensions
{
    public abstract class ValidationContextBase : IContextStep
    {
        #region IContextStep

        public abstract Task<IResponse> Process( IActionContext context );

        #endregion
    }
}
