using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Core.Components.Framework.Context.Contracts;
using XCore.Framework.Infrastructure.Context.Execution.Extensions;
using XCore.Framework.Infrastructure.Context.Execution.Models;
using XCore.Framework.Infrastructure.Context.Execution.Support;
using XCore.Framework.Utilities;

namespace XCore.Framework.Infrastructure.Context.Execution.Handler
{
    public class ExecutionContext<T>
    {
        #region props.

        public List<IContextStep> Steps { get; set; }
        public RequestContext Request { get; set; }
        public ExecutionResponse<T> Response { get; private set; }
        public ActionContext Action { get; private set; }

        #endregion
        #region cst.

        public ExecutionContext()
        {
        }

        #endregion
        #region publics.

        private async Task<ExecutionResponse<T>> Process()
        {
            try
            {
                Log();
                Culture();

                foreach (var step in this.Steps)
                {
                    var response = await step.Process(this.Action);

                    if (this.Response.State != ResponseState.Success) return this.Response;
                    if (response == null) continue;
                    if (response.State != ResponseState.Success)
                    {
                        this.Response.Set(response);
                        return this.Response;
                    }
                }

                return this.Response;
            }
            catch (Exception x)
            {
                Fail(x);
                return this.Response;
            }
        }
        public virtual async Task<ExecutionResponse<T>> Process(Func<Task<T>> func)
        {
            try
            {
                Initialize(func);
                return await Process();
            }
            catch (Exception x)
            {
                Fail(x);
                return this.Response;
            }
        }
        public virtual async Task<ExecutionResponse<T>> Process(Func<Task<T>> func, ActionContext context)
        {
            try
            {
                Initialize(func, context);
                return await Process();
            }
            catch (Exception x)
            {
                Fail(x);
                return this.Response;
            }
        }

        #endregion
        #region helpers

        private void Initialize(Func<Task<T>> func)
        {
            this.Response = new ExecutionResponse<T>() { State = ResponseState.Success, };
            this.Steps = new List<IContextStep>();

            this.Steps.Add(new LogicContext<T>(func));                                                 // logic
        }
        private void Initialize(Func<Task<T>> func, ActionContext context)
        {
            this.Request = context.Request;
            this.Response = new ExecutionResponse<T>() { State = ResponseState.Success, };
            this.Action = context;
            this.Action.Response = this.Response;
            this.Steps = new List<IContextStep>();

            if (context.Authorization != null) this.Steps.Add(context.Authorization);                  // authorize
            if (context.Validation != null) this.Steps.Add(context.Validation);                        // validate
            this.Steps.Add(new LogicContext<T>(func));                                                 // logic
            if (context.Auditing != null) this.Steps.Add(context.Auditing);                            // audit
        }

        private void Log()
        {
            //XLogger.Verbose("", XLogger.Enums.LogSettings.TransientCaller);
            XLogger.Verbose("");
        }
        private void Culture()
        {
            if (this.Request == null || string.IsNullOrEmpty(this.Request.Culture)) return;
            Thread.CurrentThread.CurrentUICulture = new CultureInfo(this.Request.Culture);
        }
        private void Fail(Exception x)
        {
            Response.Exception = x;
            Response.State = ResponseState.Error;
            XLogger.Error("Exception : " + x);

#if DEBUG
            //System.Diagnostics.Debugger.Break();    // TODO : remove in case of deployment / production.
#endif
        }

        #region cache

        #endregion
        #region mappings

        #endregion
        #region transactions

        #endregion

        #endregion
    }
}
