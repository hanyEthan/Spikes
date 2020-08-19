using System;
using System.Threading.Tasks;
using Core.Components.Framework.Context.Contracts;

namespace XCore.Framework.Infrastructure.Context.Execution.Extensions
{
    [Serializable]
    public class LogicContext<T> : IContextStep
    {
        #region props.

        public Func<Task<T>> Func { get; set; }

        #endregion
        #region cst.

        public LogicContext() { }
        public LogicContext( Func<Task<T>> func ) : this()
        {
            this.Func = func;
        }

        #endregion
        #region IContextStep

        public async Task<IResponse> Process( IActionContext context )
        {
            await this.Func();
            return null;
        }

        #endregion
    }
}
